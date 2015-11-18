using IntuiLab.Kinect.DataUserTracking;
using IntuiLab.Kinect.DataUserTracking.Events;
using IntuiLab.Kinect.Enums;
using IntuiLab.Kinect.Utils;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace IntuiLab.Kinect.DataRecording
{
    public class DataUserReplay
    {
        /// <summary>
        /// Instance of UserData
        /// </summary>
        private UserData m_refUserData;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataUserReplay()
        {
            m_refUserData = null;
        }

        /// <summary>
        /// Create a UserData to replay
        /// The joints position represent the first frame for the replay.
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="jointPosition">Joints position</param>
        /// <param name="depth">Depth</param>
        /// <param name="timesTamp">TimesTamp</param>
        public void CreateUser(int userID, Dictionary<string, Point3D> jointPosition, double depth, long timesTamp)
        {
            // Create Skeleton
            Skeleton newSkeleton = CreateSkeleton(jointPosition);

            // Create UserData
            m_refUserData = new UserData(userID, newSkeleton, depth, timesTamp);

            // Conect event
            m_refUserData.UserGestureDetected += OnUserGestureDetected;
        }

        /// <summary>
        /// Update the UserData for create a new frame.
        /// </summary>
        /// <param name="jointPosition">Joints position</param>
        /// <param name="depth">Depth</param>
        /// <param name="timesTamp">TimesTamp</param>
        public void ReceiveNewFrame(Dictionary<string, Point3D> jointPosition, double depth, long timesTamp)
        {
            // Create Skeleton
            Skeleton newSkeleton = CreateSkeleton(jointPosition);

            // Add Skeleton to UserData
            m_refUserData.AddSkeleton(newSkeleton, timesTamp);
            m_refUserData.UserDepth = depth;
        }

        /// <summary>
        /// Terminates the replay
        /// </summary>
        public void EndReplay()
        {
            m_refUserData = null;
        }

        /// <summary>
        /// Create a Skeleton with joints position
        /// </summary>
        /// <param name="jointPosition">Joints position</param>
        /// <returns>Skeleton</returns>
        private Skeleton CreateSkeleton(Dictionary<string, Point3D> jointPosition)
        {
            // Create a Skeleton
            Skeleton refSkeleton = new Skeleton();
            refSkeleton.TrackingState = SkeletonTrackingState.Tracked;

            // Inform position of every joints 
            foreach (KeyValuePair<string, Point3D> joint in jointPosition)
            {
                SkeletonPoint refSkeletonPoint = new SkeletonPoint();
                refSkeletonPoint.X = (float)joint.Value.X;
                refSkeletonPoint.Y = (float)joint.Value.Y;
                refSkeletonPoint.Z = (float)joint.Value.Z;

                Joint newJoint = refSkeleton.Joints[GetJoinType(joint.Key)];
                newJoint.Position = refSkeletonPoint;
                refSkeleton.Joints[GetJoinType(joint.Key)] = newJoint;
            }

            return refSkeleton;       
        }

        /// <summary>
        /// Get the JointType whith her name in string.
        /// </summary>
        /// <param name="jointName">Joint name</param>
        /// <returns>JointType</returns>
        private JointType GetJoinType(string jointName)
        {
            switch (jointName)
            {
                case "Head":
                    return JointType.Head;
                case "ShoulderCenter":
                    return JointType.ShoulderCenter;
                case "HandRight":
                    return JointType.HandRight;
                case "WristRight":
                    return JointType.WristRight;
                case "ElbowRight":
                    return JointType.ElbowRight;
                case "ShoulderRight":
                    return JointType.ShoulderRight;
                case "HandLeft":
                    return JointType.HandLeft;
                case "WristLeft":
                    return JointType.WristLeft;
                case "ElbowLeft":
                    return JointType.ElbowLeft;
                case "ShoulderLeft":
                    return JointType.ShoulderLeft;
                case "Spine":
                    return JointType.Spine;
                case "HipCenter":
                    return JointType.HipCenter;
                case "HipLeft":
                    return JointType.HipLeft;
                case "KneeLeft":
                    return JointType.KneeLeft;
                case "AnkleLeft":
                    return JointType.AnkleLeft;
                case "FootLeft":
                    return JointType.FootLeft;
                case "HipRight":
                    return JointType.HipRight;
                case "KneeRight":
                    return JointType.KneeRight;
                case "AnkleRight":
                    return JointType.AnkleRight;
                case "FootRight":
                    return JointType.FootRight;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Callback when a gesture is detected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUserGestureDetected(object sender, UserGestureDetectedEventArgs e)
        {
            switch (e.Gesture)
            {
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_SWIPE_LEFT :
                    DebugLog.DebugTraceLog("SwipeLeft detected", false);
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_SWIPE_RIGHT :
                    DebugLog.DebugTraceLog("SwipeRight detected", false);
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_WAVE :
                    DebugLog.DebugTraceLog("Wave detected", false);
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_PUSH :
                    DebugLog.DebugTraceLog("Push detected", false);
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_MAXIMIZE:
                    DebugLog.DebugTraceLog("Maximize detected", false);
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_MINIMIZE:
                    DebugLog.DebugTraceLog("Minimize detected", false);
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_A :
                    DebugLog.DebugTraceLog("Posture A detected", false);
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_HOME :
                    DebugLog.DebugTraceLog("Posture Home detected", false);
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_STAY :
                    DebugLog.DebugTraceLog("Posture Stay detected", false);
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_T :
                    DebugLog.DebugTraceLog("Posture T detected", false);
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_U :
                    DebugLog.DebugTraceLog("Posture U detected", false);
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_V :
                    DebugLog.DebugTraceLog("Posture V detected", false);
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_WAIT :
                    DebugLog.DebugTraceLog("Posture Wait detected", false);
                    break;
            }
        }
    }
}
