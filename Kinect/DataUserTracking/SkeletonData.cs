using Microsoft.Kinect;
using System.Collections.Generic;
using System.Windows;

namespace IntuiLab.Kinect.DataUserTracking
{
    internal class SkeletonData
    {
        #region Fields

        /// <summary>
        /// Skeleton's joints
        /// </summary>
        private Dictionary<JointType, Joint> m_refJoints;

        #endregion

        #region Properties

        /// <summary>
        /// Skeleton
        /// </summary>
        public Skeleton refSkeleton { get; private set; }

        /// <summary>
        /// Skeleton Timestamp
        /// </summary>
        public long TimesTamp { get; set; }

        /// <summary>
        /// skeleton position
        /// </summary>
        internal SkeletonPoint Position { get; set; }

        /// <summary>
        /// Skeleton ID
        /// </summary>
        internal int TrackingID { get; set; }

        /// <summary>
        /// skeleton tracking state
        /// </summary>
        internal SkeletonTrackingState TrackingState { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="skeleton">Skeleton</param>
        /// <param name="timesTamp">Skeleton Timestamp</param>
        public SkeletonData(Skeleton skeleton, long timesTamp)
        {
            TimesTamp = timesTamp;
            Position = skeleton.Position;
            TrackingID = skeleton.TrackingId;
            TrackingState = skeleton.TrackingState;
            refSkeleton = skeleton;

            if (TrackingState == SkeletonTrackingState.Tracked)
            {
                m_refJoints = new Dictionary<JointType, Joint>();
                foreach (Joint refJoint in skeleton.Joints)
                {
                    m_refJoints.Add(refJoint.JointType, refJoint);
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the Joint position
        /// </summary>
        /// <param name="refJoint">Joint treated</param>
        /// <returns>Joint position</returns>
        public SkeletonPoint GetJointPosition(JointType refJoint)
        {
            if (TrackingState == SkeletonTrackingState.Tracked)
            {
                return m_refJoints[refJoint].Position;
            }
            return new SkeletonPoint();
        }

        /// <summary>
        /// Get the tracking state of a joint
        /// </summary>
        /// <param name="refJoint">Joint treated</param>
        /// <returns>tracking state of joint</returns>
        public JointTrackingState GetJointState(JointType refJoint)
        {
            if (TrackingState == SkeletonTrackingState.Tracked)
            {
                return m_refJoints[refJoint].TrackingState;
            }
            return JointTrackingState.NotTracked;
        }

        /// <summary>
        /// Get joint position on screen
        /// </summary>
        /// <param name="refJoint">Joint treated</param>
        /// <returns>Joint position on screen</returns>
        public Point GetJointPositionOnScreen(JointType refJoint)
        {
            if (TrackingState == SkeletonTrackingState.Tracked)
            {
                return Utils.Feedback.SkeletonPointToScreen(m_refJoints[refJoint].Position);
            }
            return new Point();
        }
        #endregion
    }
}
