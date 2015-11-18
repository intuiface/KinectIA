using IntuiLab.Kinect.DataUserTracking;
using IntuiLab.Kinect.Enums;
using Microsoft.Kinect;
using System.Windows.Media.Media3D;

namespace IntuiLab.Kinect.GestureRecognizer.Postures
{
    internal class PostureTCondition : Condition
    {
        #region Fields

        /// <summary>
        /// Instance od Checker
        /// </summary>
        private readonly Checker m_refChecker;
        
        /// <summary>
        /// Consecutive numbers of frame where the conditions is satisfied
        /// </summary>
        private int m_nIndex;

        /// <summary>
        /// Inform if the gesture is begin or not
        /// </summary>
        private bool m_GestureBegin;

        /// <summary>
        /// Number of frame where the conditions is satisfied during the initialization.
        /// </summary>
        private int m_frameInitCounter;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="refUser">User Data</param>
        public PostureTCondition(UserData refUser)
            : base(refUser)
        {
            m_nIndex = 0;
            m_frameInitCounter = 0;
            m_refChecker = new Checker(refUser, PropertiesPluginKinect.Instance.PostureCheckerTolerance);
            m_GestureBegin = false;
        }

        /// <summary>
        /// See description in Condition class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Check(object sender, NewSkeletonEventArgs e)
        {
            // Calculate vector between Shoulder center and hands
            Vector3D Vector_ShoulderC_HandR = new Vector3D(
                e.m_refSkeletonData.refSkeleton.Joints[JointType.WristRight].Position.X - e.m_refSkeletonData.refSkeleton.Joints[JointType.ShoulderCenter].Position.X,
                e.m_refSkeletonData.refSkeleton.Joints[JointType.WristRight].Position.Y - e.m_refSkeletonData.refSkeleton.Joints[JointType.ShoulderCenter].Position.Y,
                e.m_refSkeletonData.refSkeleton.Joints[JointType.WristRight].Position.Z - e.m_refSkeletonData.refSkeleton.Joints[JointType.ShoulderCenter].Position.Z);

            Vector3D Vector_ShoulderC_HandL = new Vector3D(
                e.m_refSkeletonData.refSkeleton.Joints[JointType.WristLeft].Position.X - e.m_refSkeletonData.refSkeleton.Joints[JointType.ShoulderCenter].Position.X,
                e.m_refSkeletonData.refSkeleton.Joints[JointType.WristLeft].Position.Y - e.m_refSkeletonData.refSkeleton.Joints[JointType.ShoulderCenter].Position.Y,
                e.m_refSkeletonData.refSkeleton.Joints[JointType.WristLeft].Position.Z - e.m_refSkeletonData.refSkeleton.Joints[JointType.ShoulderCenter].Position.Z);

            // Calculate vector between Shoulder, elbow and hand right
            Vector3D Vector_ElbowR_ShoulderR = new Vector3D(
                e.m_refSkeletonData.refSkeleton.Joints[JointType.ShoulderRight].Position.X - e.m_refSkeletonData.refSkeleton.Joints[JointType.ElbowRight].Position.X,
                e.m_refSkeletonData.refSkeleton.Joints[JointType.ShoulderRight].Position.Y - e.m_refSkeletonData.refSkeleton.Joints[JointType.ElbowRight].Position.Y,
                e.m_refSkeletonData.refSkeleton.Joints[JointType.ShoulderRight].Position.Z - e.m_refSkeletonData.refSkeleton.Joints[JointType.ElbowRight].Position.Z);

            Vector3D Vector_ElbowR_HandR = new Vector3D(
                e.m_refSkeletonData.refSkeleton.Joints[JointType.WristRight].Position.X - e.m_refSkeletonData.refSkeleton.Joints[JointType.ElbowRight].Position.X,
                e.m_refSkeletonData.refSkeleton.Joints[JointType.WristRight].Position.Y - e.m_refSkeletonData.refSkeleton.Joints[JointType.ElbowRight].Position.Y,
                e.m_refSkeletonData.refSkeleton.Joints[JointType.WristRight].Position.Z - e.m_refSkeletonData.refSkeleton.Joints[JointType.ElbowRight].Position.Z);

            // Calculate vector between Shoulder, elbow and hand left
            Vector3D Vector_ElbowL_ShoulderL = new Vector3D(
                e.m_refSkeletonData.refSkeleton.Joints[JointType.ShoulderLeft].Position.X - e.m_refSkeletonData.refSkeleton.Joints[JointType.ElbowLeft].Position.X,
                e.m_refSkeletonData.refSkeleton.Joints[JointType.ShoulderLeft].Position.Y - e.m_refSkeletonData.refSkeleton.Joints[JointType.ElbowLeft].Position.Y,
                e.m_refSkeletonData.refSkeleton.Joints[JointType.ShoulderLeft].Position.Z - e.m_refSkeletonData.refSkeleton.Joints[JointType.ElbowLeft].Position.Z);

            Vector3D Vector_ElbowL_HandL = new Vector3D(
                e.m_refSkeletonData.refSkeleton.Joints[JointType.WristLeft].Position.X - e.m_refSkeletonData.refSkeleton.Joints[JointType.ElbowLeft].Position.X,
                e.m_refSkeletonData.refSkeleton.Joints[JointType.WristLeft].Position.Y - e.m_refSkeletonData.refSkeleton.Joints[JointType.ElbowLeft].Position.Y,
                e.m_refSkeletonData.refSkeleton.Joints[JointType.WristLeft].Position.Z - e.m_refSkeletonData.refSkeleton.Joints[JointType.ElbowLeft].Position.Z);

            // Calculate angle
            double angleBetweenShoulder_Hand = Vector3D.AngleBetween(Vector_ShoulderC_HandR, Vector_ShoulderC_HandL);
            double angleBetweenShoulderR_HandR = Vector3D.AngleBetween(Vector_ElbowL_HandL, Vector_ElbowL_ShoulderL);
            double angleBetweenShoulderL_HandL = Vector3D.AngleBetween(Vector_ElbowR_HandR, Vector_ElbowR_ShoulderR);

            // Relative velocity of HandLeft
            double handLeftVelocity = m_refChecker.GetRelativeVelocity(JointType.HipCenter, JointType.WristLeft);

            // RElative velocity of HandRight
            double handRightVelocity = m_refChecker.GetRelativeVelocity(JointType.HipCenter, JointType.WristRight);

            // Speed Condition
            if (handLeftVelocity > PropertiesPluginKinect.Instance.PostureLowerBoundForVelocity || handRightVelocity > PropertiesPluginKinect.Instance.PostureLowerBoundForVelocity)
            {
                Reset();
            }
            // Condition : Angle between HandRight/ShoulderCenter/HandLeft = 160° && arms extended
            else if ((angleBetweenShoulder_Hand <= (PropertiesPluginKinect.Instance.TAngleShoulderHands + PropertiesPluginKinect.Instance.TAngleThreshold) && angleBetweenShoulder_Hand >= (PropertiesPluginKinect.Instance.TAngleShoulderHands - PropertiesPluginKinect.Instance.TAngleThreshold))
                && (angleBetweenShoulderR_HandR <= (PropertiesPluginKinect.Instance.TAngleShoulderElbowHand + PropertiesPluginKinect.Instance.PostureAngleTresholdShoulderElBowHand) && angleBetweenShoulderR_HandR >= (PropertiesPluginKinect.Instance.TAngleShoulderElbowHand - PropertiesPluginKinect.Instance.PostureAngleTresholdShoulderElBowHand))
                && (angleBetweenShoulderL_HandL <= (PropertiesPluginKinect.Instance.TAngleShoulderElbowHand + PropertiesPluginKinect.Instance.PostureAngleTresholdShoulderElBowHand) && angleBetweenShoulderL_HandL >= (PropertiesPluginKinect.Instance.TAngleShoulderElbowHand - PropertiesPluginKinect.Instance.PostureAngleTresholdShoulderElBowHand)))
            {
                // Posture T is complete
                if (m_nIndex > PropertiesPluginKinect.Instance.TLowerBoundForSuccess)
                {
                    // Notify the posture T is complete
                    FireSucceeded(this, new SuccessGestureEventArgs
                    {
                        Gesture = EnumGesture.GESTURE_NONE,
                        Posture = EnumPosture.POSTURE_T
                    });
                    m_nIndex = 0;
                    m_frameInitCounter = 0;
                }
                else
                {
                    // Posture T in initialization
                    if (m_frameInitCounter < PropertiesPluginKinect.Instance.PostureNumberFrameInitialisation)
                    {
                        m_frameInitCounter++;
                        // If initialization is complete
                        if (m_frameInitCounter == PropertiesPluginKinect.Instance.PostureNumberFrameInitialisation)
                        {
                            // Notify the posture T is begin
                            RaiseGestureBegining(this, new BeginGestureEventArgs
                            {
                                Gesture = EnumGesture.GESTURE_NONE,
                                Posture = EnumPosture.POSTURE_T
                            });
                            m_GestureBegin = true;
                        }
                    }
                    // Step successful, waiting for next
                    else
                    {
                        m_nIndex++;
                        // Notify posture T progress
                        RaiseGestureProgressed(this, new ProgressGestureEventArgs
                        {
                            Gesture = EnumGesture.GESTURE_NONE,
                            Posture = EnumPosture.POSTURE_T,
                            Percent = ((float)m_nIndex / (float)PropertiesPluginKinect.Instance.TLowerBoundForSuccess) * 100
                        });
                    }
                }
            }
            // Posture is broken
            else
            {
                Reset();
            }
        }

        /// <summary>
        /// Restart detecting
        /// </summary>
        private void Reset()
        {
            // If posture begin, notify posture is end
            if (m_GestureBegin)
            {
                m_GestureBegin = false;
                RaiseGestureEnded(this, new EndGestureEventArgs
                {
                    Gesture = EnumGesture.GESTURE_NONE,
                    Posture = EnumPosture.POSTURE_T
                });
            }
            m_nIndex = 0;
            m_frameInitCounter = 0;
            FireFailed(this, new FailedGestureEventArgs
            {
                refCondition = this
            });
        }
    }
}
