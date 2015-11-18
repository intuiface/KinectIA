using IntuiLab.Kinect.DataUserTracking;
using IntuiLab.Kinect.DataUserTracking.Events;
using IntuiLab.Kinect.TUIO;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Interaction;
using System;
using System.Collections.Generic;
using System.Windows;

namespace IntuiLab.Kinect
{
    /// <summary>
    /// Mange the user in front of the kinect sensor
    /// </summary>
    public partial class KinectModule
    {
        #region Fields

        /// <summary>
        /// User list tracked by the kinect sensor
        /// </summary>
        private Dictionary<int, UserData> m_refListUsers;

        /// <summary>
        /// Insatnce of TuioManager for send message to the MGRE
        /// </summary>
        private TuioManager m_refTuioManager; 

        #endregion

        #region Events
        
        #region NewHandActive

        /// <summary>
        /// Event triggered when a new hand is active
        /// </summary>
        public event EventHandler<HandActiveEventArgs> NewHandActive;

        /// <summary>
        /// Raise event NewHandActive
        /// </summary>
        protected void RaiseNewHandActive(object sender, HandActiveEventArgs e)
        {
            if (NewHandActive != null)
            {
                NewHandActive(this,e);
            }
        }
        #endregion

        #region UserHandMove

        /// <summary>
        /// Event triggered when user's hand moved
        /// </summary>
        public event EventHandler<HandMoveEventArgs> UserHandMove;

        /// <summary>
        /// Raise event UserHandMove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RaiseUserHandMove(object sender, HandMoveEventArgs e)
        {
            if (UserHandMove != null)
            {
                UserHandMove(this, e);
            }
        }

        #endregion

        #region UserHandGripStateChanged

        /// <summary>
        /// Event triggered when user's hand grip state changed
        /// </summary>
        public event EventHandler<HandGripStateChangeEventArgs> UserHandGripStateChanged;

        /// <summary>
        /// Raise event UserHandGripStateChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RaiseUserHandGripStateChanged(object sender, HandGripStateChangeEventArgs e)
        {
            if (UserHandGripStateChanged != null)
            {
                UserHandGripStateChanged(this, e);
            }
        }

        #endregion

        #region NewUserPointing

        /// <summary>
        /// Event triggered when a new user in pointing mode is detected.
        /// This event permit to attribute the hand's feedback id in the Pointingfacade class.
        /// </summary>
        public event EventHandler<NewUserPointingEventArgs> NewUserPointing;

        /// <summary>
        /// Raise event NewUserPointing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RaiseNewUserPointing(object sender, NewUserPointingEventArgs e)
        {
            if (NewUserPointing != null)
            {
                NewUserPointing(this, e);
            }
        }

        #endregion

        #region DeleteUserPointing

        /// <summary>
        /// Event triggered when a user in pointing mode is deleted.
        /// This event permit to liberate the hand's feedback id in the Pointingfacade class.
        /// </summary>
        public event EventHandler<DeleteUserPointingEventArgs> DeleteUserPointing;

        /// <summary>
        /// Raise event DeleteUserPointing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RaiseDeleteUserPointing(object sender, DeleteUserPointingEventArgs e)
        {
            if (DeleteUserPointing != null)
            {
                DeleteUserPointing(this, e);
            }
        }

        #endregion

        #endregion

        #region Add User Data

        /// <summary>
        /// Add a User
        /// </summary>
        /// <param name="refSkeleton">New User's Skeleton</param>
        /// <param name="depth">Distance User/sensor Kinect</param>
        /// <param name="timesTamp">Skeleton TimesTamp</param>
        private void AddUserData(Skeleton refSkeleton, double depth, long timesTamp)
        {
            UserDataPointing refNewUser = new UserDataPointing(refSkeleton.TrackingId, refSkeleton, depth, timesTamp); 

            if (PropertiesPluginKinect.Instance.KinectPointingModeEnabled)
            {
                // If the user is the first, create the TuioManager
                if (m_refListUsers.Count == 0)
                {
                    m_refTuioManager = new TuioManager();
                }

                RaiseNewUserPointing(this, new NewUserPointingEventArgs
                {
                    UserID = refSkeleton.TrackingId
                });

                refNewUser.UserHandActive += OnUserHandActive;
                refNewUser.UserHandMove += OnUserHandMove;
                refNewUser.UserHandGripStateChanged += OnUserHandGripStateChanged;
            }
            else
            {
                // Grip is also used for Gesture Recognition
                refNewUser.UserHandGripStateChanged += OnUserHandGripStateChanged;
                
                // If recording mode is activated, send the user to the DataUserRecorder
                if (m_bRecData)
                {
                    lock (this)
                    {
                        m_refDataUserRecorder.RecordData(refNewUser);
                    }
                }
            }

            // Connect events
            refNewUser.UserGestureDetected += OnUserGestureDetected;
            refNewUser.UserGestureProgress += OnUsergestureProgress;
            refNewUser.UserGestureLost += OnUserGestureLost;

            refNewUser.UserPositionInColorFrame = SkeletonPointToColorImage(refSkeleton.Joints[JointType.ShoulderCenter].Position);
            refNewUser.UserPositionInDepthFrame = SkeletonPointToDepthImage(refSkeleton.Joints[JointType.ShoulderCenter].Position);

            // Add User in system
            lock (m_refListUsers)
            {
                m_refListUsers.Add(refSkeleton.TrackingId, refNewUser);    
            }
                
            // Notify new user
            PropertiesPluginKinect.Instance.UserCounter++;
        }
        #endregion

        #region Modify User Data
        /// <summary>
        /// Modify a User
        /// </summary>
        /// <param name="nUserID">User ID</param>
        /// <param name="refSkeleton">New User's Skeleton</param>
        /// <param name="depth">Distance User/sensor Kinect</param>
        /// <param name="timesTamp">Skeleton TimesTamp</param>
        private void ModifyUserData(int nUserID, Skeleton refSkeleton, double depth, long timesTamp)
        {
            // Update the DataUser
            m_refListUsers[nUserID].AddSkeleton(refSkeleton, timesTamp);
            m_refListUsers[nUserID].UserPositionInColorFrame = SkeletonPointToColorImage(refSkeleton.Joints[JointType.ShoulderCenter].Position);
            m_refListUsers[nUserID].UserPositionInDepthFrame = SkeletonPointToDepthImage(refSkeleton.Joints[JointType.ShoulderCenter].Position);
            m_refListUsers[nUserID].UserDepth = depth;

            // If recording mode is activating, send the user to the DataUserRecorder
            if (m_bRecData)
            {
                lock (this)
                {
                    m_refDataUserRecorder.RecordData(m_refListUsers[nUserID]);
                }
            }
        }
        #endregion

        #region Delete User Data
        /// <summary>
        /// Delete a User
        /// </summary>
        /// <param name="nUserID">User ID</param>
        private void DeleteUserData(int nUserID)
        {
            // Disconect user events
            m_refListUsers[nUserID].UserGestureDetected -= OnUserGestureDetected;
            m_refListUsers[nUserID].UserGestureProgress -= OnUsergestureProgress;
            m_refListUsers[nUserID].UserGestureLost -= OnUserGestureLost;

            // If the Pointing mode is enabled, disconnect event of tjis mode
            if ((m_refListUsers[nUserID] as UserDataPointing) != null)
            {
                (m_refListUsers[nUserID] as UserDataPointing).UserHandActive -= OnUserHandActive;
                (m_refListUsers[nUserID] as UserDataPointing).UserHandMove -= OnUserHandMove;
                (m_refListUsers[nUserID] as UserDataPointing).UserHandGripStateChanged -= OnUserHandGripStateChanged;

                (m_refListUsers[nUserID] as UserDataPointing).DisposeUserDataPointing();

                // If this user is the last in fornt of the Kinect sensor, destroy the TuioManager
                if (m_refListUsers.Count == 1)
                {
                    if (m_refTuioManager != null)
                    {
                        m_refTuioManager.Dispose();
                    }
                }

                RaiseDeleteUserPointing(this, new DeleteUserPointingEventArgs
                {
                    UserID = nUserID
                });
            }
            else 
            {
                // Dispose the UserData
                m_refListUsers[nUserID].Dispose();
            }

            // Remove user in system
            lock (m_refListUsers)
            {
                m_refListUsers.Remove(nUserID);    
            }
            
            // Notify remove user
            PropertiesPluginKinect.Instance.UserCounter--;
        }
        #endregion

        #region Skeleton Point Converter
        /// <summary>
        /// Get back the SkeletonPoint position in the ColorFrame
        /// </summary>
        /// <param name="point">The SkeletonPoint to treat</param>
        /// <returns></returns>
        private Point SkeletonPointToColorImage(SkeletonPoint point)
        {
            ColorImagePoint colorPoint = m_refKinectsensor.CoordinateMapper.MapSkeletonPointToColorPoint(point, ColorImageFormat.RgbResolution640x480Fps30);

            return new Point(colorPoint.X, colorPoint.Y);
        }

        /// <summary>
        /// Get back the SkeletonPoint position in the DepthFrame
        /// </summary>
        /// <param name="point">The SkeletonPoint to treat</param>
        /// <returns></returns>
        private Point SkeletonPointToDepthImage(SkeletonPoint point)
        {
            DepthImagePoint depthPoint = m_refKinectsensor.CoordinateMapper.MapSkeletonPointToDepthPoint(point, DepthImageFormat.Resolution640x480Fps30);

            return new Point(depthPoint.X, depthPoint.Y);
        }

        #endregion

        #region Event's Handler
        /// <summary>
        /// Callback when a hand's user is active.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUserHandActive(object sender, HandActiveEventArgs e)
        {
            RaiseNewHandActive(this, e);
            // If the hand is became inactive and it's in grip state,
            // we delete the hand in TuioManager
            if (!e.IsActive)
            {
                if (e.HandType == InteractionHandType.Left)
                {
                    m_refTuioManager.DeleteHandTuioKinect(e.userID);
                }
                else if (e.HandType == InteractionHandType.Right)
                {
                    m_refTuioManager.DeleteHandTuioKinect(e.userID * 100);
                }
            }
        }

        /// <summary>
        /// Callback when an user's hand moved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUserHandMove(object sender, HandMoveEventArgs e)
        {
            RaiseUserHandMove(this, e);
            // If the hand is in grip state, send TuioMessage for the MGRE
            if (m_refTuioManager != null && e.IsGrip)
            {
                if(e.HandType == InteractionHandType.Left)
                {
                    m_refTuioManager.UpdateHandTuioKinect(e.userID, e.RawPosition);
                }
                else if (e.HandType == InteractionHandType.Right)
                {
                    m_refTuioManager.UpdateHandTuioKinect(e.userID * 100, e.RawPosition);
                }
            }
        }

        /// <summary>
        /// Callback when an user's hand grip state changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUserHandGripStateChanged(object sender, HandGripStateChangeEventArgs e)
        {
            RaiseUserHandGripStateChanged(this, e);
            if (m_refTuioManager != null)
            {
                // If the the hand is grip, send message to MGRE for update the data
                if (e.IsGrip)
                {
                    if (e.HandType == InteractionHandType.Left)
                    {
                        m_refTuioManager.AddTuioHandKinect(e.userID, e.RawPosition);
                    }
                    else if (e.HandType == InteractionHandType.Right)
                    {
                        m_refTuioManager.AddTuioHandKinect(e.userID * 100, e.RawPosition);
                    }
                }
                else // if the hand is not grip, send message to MGRE for stop the interaction
                {
                    if (e.HandType == InteractionHandType.Left)
                    {
                        m_refTuioManager.DeleteHandTuioKinect(e.userID);
                    }
                    else if (e.HandType == InteractionHandType.Right)
                    {
                        m_refTuioManager.DeleteHandTuioKinect(e.userID * 100);
                    }
                }
            }
        }
        #endregion
    }
}
