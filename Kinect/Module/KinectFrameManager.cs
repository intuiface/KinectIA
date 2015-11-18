using IntuiLab.Kinect.DataUserTracking;
using IntuiLab.Kinect.Utils;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntuiLab.Kinect
{
    /// <summary>
    /// Manage all frame of kinect sensor.
    /// </summary>
    public partial class KinectModule
    {
        #region Event's Handlers
        /// <summary>
        /// Callback when all kinect frames are ready
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            lock (this)
            {
                ManageAllFrame(e);
            }
        }
        #endregion

        #region All Frame Managers
        /// <summary>
        /// Manage frames of kinect sensor according to the services activated
        /// </summary>
        /// <param name="e"></param>
        private void ManageAllFrame(AllFramesReadyEventArgs e)
        {
            if (!IsRunning)
            {
                return;
            }

            // SkeletonTracking Frame Manager
            using (SkeletonFrame SFrame = e.OpenSkeletonFrame())
            {
                try
                {
                    ManageSkeletonFrame(SFrame);
                }
                catch (Exception ex)
                {
                    // Just log the error   
                    Console.Error.WriteLine("Error with skeleton frame : " + ex.Message + " _ " + ex.StackTrace);
                }
            }

            // Color Frame Manager
            if (PropertiesPluginKinect.Instance.EnableColorFrameService)
            {
                using (ColorImageFrame CFrame = e.OpenColorImageFrame())
                {
                    try
                    {
                        ManageColorFrame(CFrame);
                    }
                    catch (Exception ex)
                    {
                        // Just log the error   
                        Console.Error.WriteLine("Error with color frame : " + ex.Message + " _ " + ex.StackTrace);
                    }
                }
            }

            // Depth Frame Manager
            if (PropertiesPluginKinect.Instance.EnableDepthFrameService || 
                PropertiesPluginKinect.Instance.KinectPointingModeEnabled ||
                PropertiesPluginKinect.Instance.EnableGestureGrip)
            {
                using (DepthImageFrame DFrame = e.OpenDepthImageFrame())
                {
                    try
                    {
                        ManageDepthFrame(DFrame);
                    }
                    catch (Exception ex)
                    {
                        // Just log the error   
                        Console.Error.WriteLine("Error with depth frame : " + ex.Message + " _ " + ex.StackTrace);
                    }
                        
                }
            }
        }
        #endregion

        #region Color Frame Manager
        /// <summary>
        /// Color Frame manager
        /// </summary>
        /// <param name="refColorImage">New Frame</param>
        private void ManageColorFrame(ColorImageFrame refColorImage)
        {
            // If system is locked, draw system lock feedback
            if (IsLocked)
            {
                PropertiesPluginKinect.Instance.ColorFrame = Feedback.FeedbackSystemLocked(PropertiesPluginKinect.Instance.KinectResolutionWidth, PropertiesPluginKinect.Instance.KinectResolutionHeight);
            }
            else // Draw the color frame feedback
            {
                byte[] pixelDataRGB = null;

                // Get the datas pixel
                if (refColorImage != null)
                {
                    pixelDataRGB = new byte[refColorImage.PixelDataLength];
                    refColorImage.CopyPixelDataTo(pixelDataRGB);
                }

                if (pixelDataRGB != null)
                {
                    // If the skeleton feedback on color frame is activated, draw skeleton on color frame.
                    if (!PropertiesPluginKinect.Instance.EnableSkeletonOnColorDepth)
                    {
                        PropertiesPluginKinect.Instance.ColorFrame = Feedback.FeedbackColorFrame(pixelDataRGB, PropertiesPluginKinect.Instance.KinectResolutionWidth, PropertiesPluginKinect.Instance.KinectResolutionHeight);
                    }
                    else // Draw only color frame
                    {
                        PropertiesPluginKinect.Instance.ColorFrame = Feedback.FeedbackColorFrameWithSkeleton(m_refKinectsensor, m_refListUsers.Values.ToList(), pixelDataRGB, PropertiesPluginKinect.Instance.KinectResolutionWidth, PropertiesPluginKinect.Instance.KinectResolutionHeight);
                    }
                }
            }
        }
        #endregion

        #region Depth Frame Manager
        /// <summary>
        /// DepthFrame Manager
        /// </summary>
        /// <param name="refDepthImage"></param>
        private void ManageDepthFrame(DepthImageFrame refDepthImage)
        {
            // If system is locked, draw system lock feedback
            if (IsLocked && PropertiesPluginKinect.Instance.EnableDepthFrameService)
            {
                PropertiesPluginKinect.Instance.DepthFrame = Feedback.FeedbackSystemLocked(PropertiesPluginKinect.Instance.KinectResolutionWidth, PropertiesPluginKinect.Instance.KinectResolutionHeight);
            }
            else // Draw the depth frame feedback
            {
                short[] pixelDataDepth = null;

                // Get the datas pixel
                if (refDepthImage != null)
                {
                    pixelDataDepth = new short[refDepthImage.PixelDataLength];
                    refDepthImage.CopyPixelDataTo(pixelDataDepth);
                   
                    if (m_refKinectInteraction != null)
                    {
                        lock (m_refKinectInteraction)
                        {
                            if (m_refKinectInteraction != null)
                            {
                                m_refKinectInteraction.ProcessDepth(refDepthImage.GetRawPixelData(), refDepthImage.Timestamp);
                            }
                        }
                    }
                }

                if (pixelDataDepth != null && PropertiesPluginKinect.Instance.EnableDepthFrameService)
                {
                    // If the skeleton feedback on depth frame is activated, draw skeleton on depth frame.
                    if (!PropertiesPluginKinect.Instance.EnableSkeletonOnColorDepth)
                    {
                        PropertiesPluginKinect.Instance.DepthFrame = Feedback.FeedbackDepthFrame(pixelDataDepth, PropertiesPluginKinect.Instance.KinectResolutionWidth, PropertiesPluginKinect.Instance.KinectResolutionHeight);
                    }
                    else // Draw only depth frame
                    {
                        PropertiesPluginKinect.Instance.DepthFrame = Feedback.FeedbackDepthFrameWithSkeleton(m_refKinectsensor, m_refListUsers.Values.ToList(), pixelDataDepth, PropertiesPluginKinect.Instance.KinectResolutionWidth, PropertiesPluginKinect.Instance.KinectResolutionHeight);
                    }
                }
            }
        }
        #endregion

        #region Skeleton Frame Manager
        /// <summary>
        /// SkeletonFrame Manager
        /// </summary>
        /// <param name="refSkeletonFrame"></param>
        private void ManageSkeletonFrame(SkeletonFrame refSkeletonFrame)
        {
            // Save id user tracked
            List<int> lstTempUserID = m_refListUsers.Keys.ToList();
            List<int> lstCurrentUserID = new List<int>();

            int nUserIDNearest = -1;
            double dNearestDistance = double.MaxValue;

            // Get Skeletons in SkeletonFrame
            Skeleton[] skeletons = new Skeleton[0];
            if (refSkeletonFrame != null)
            {
                skeletons = new Skeleton[refSkeletonFrame.SkeletonArrayLength];
                refSkeletonFrame.CopySkeletonDataTo(skeletons);
                                
                if (m_refKinectInteraction != null)
                {
                    lock (m_refKinectInteraction)
                    {
                        if (m_refKinectInteraction != null)
                        {
                            try
                            {
                                var accelerometerReading = m_refKinectsensor.AccelerometerGetCurrentReading();
                                m_refKinectInteraction.ProcessSkeleton(skeletons, accelerometerReading, refSkeletonFrame.Timestamp);
                            }
                            catch (InvalidOperationException)
                            { }
                        }
                    }
                }
            }

            // For each skeleton 
            foreach (Skeleton skeleton in skeletons)
            {
                // If the skeleton is stracked
                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                {
                    // Calculate the User/Kinect distance
                    double dDistance = skeleton.Position.Z;

                    if (dDistance < dNearestDistance)
                    {
                        dNearestDistance = dDistance;
                        nUserIDNearest = skeleton.TrackingId;
                    }

                    // If the User is detect for the first time
                    if (!m_refListUsers.ContainsKey(skeleton.TrackingId))
                    {
                        // Create User
                        AddUserData(skeleton, dDistance, refSkeletonFrame.Timestamp);
                    }
                    else
                    {
                        // Update User
                        ModifyUserData(skeleton.TrackingId, skeleton, dDistance, refSkeletonFrame.Timestamp);
                    }

                    lstCurrentUserID.Add(skeleton.TrackingId);
                }
            }

            // Remove User data if is not present
            foreach (int nOldUserID in lstTempUserID)
            {
                if (!lstCurrentUserID.Contains(nOldUserID))
                {
                    DeleteUserData(nOldUserID);
                }
            }

            // Update of nearest user
            foreach (KeyValuePair<int, UserData> user in m_refListUsers)
            {
                if (user.Key == nUserIDNearest)
                {
                    user.Value.IsNearest = true;
                    PropertiesPluginKinect.Instance.KinectUserDistance = user.Value.UserDepth.ToString();
                }
                else
                {
                    user.Value.IsNearest = false;
                }
            }

            // If system is locked, draw system lock feedback
            if (IsLocked)
            {
                PropertiesPluginKinect.Instance.SkeletonFrameAlone = Feedback.FeedbackSystemLocked(PropertiesPluginKinect.Instance.KinectResolutionWidth, PropertiesPluginKinect.Instance.KinectResolutionHeight);
            }
            // If the skeleton feedback is activated, draw skeleton.
            else if (PropertiesPluginKinect.Instance.EnableSkeletonFrameService)
            {
                PropertiesPluginKinect.Instance.SkeletonFrameAlone = Feedback.FeedbackSkeletonFrameAlone(m_refKinectsensor, m_refListUsers.Values.ToList(), PropertiesPluginKinect.Instance.KinectResolutionWidth, PropertiesPluginKinect.Instance.KinectResolutionHeight);
            }
        }
        #endregion

        #region KinectInteraction Frame Manager
        /// <summary>
        /// KinectInteraction Frame Manager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKinectInteractionFrameReady(object sender, InteractionFrameReadyEventArgs e)
        {
            // get the users KinectInteraction frame
            UserInfo[] userInfos = new UserInfo[InteractionFrame.UserInfoArrayLength]; 
            using (var iaf = e.OpenInteractionFrame())
            {
                if (iaf == null)
                {
                    return;
                }

                iaf.CopyInteractionDataTo(userInfos);
            }

            // For each user, treated the kinectInteraction
            foreach (var userInfo in userInfos)
            {
                if (m_refListUsers.ContainsKey(userInfo.SkeletonTrackingId))
                {
                    (m_refListUsers[userInfo.SkeletonTrackingId] as UserDataPointing).UpdateDataHands(userInfo);
                }
            }
        }
        #endregion
    }
}
