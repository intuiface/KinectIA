using IntuiLab.Kinect.DataUserTracking;
using IntuiLab.Kinect.Enums;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Linq;

namespace IntuiLab.Kinect.GestureRecognizer.Gestures
{
    internal class SwipeCondition : Condition
    {
        #region Fields

        /// <summary>
        /// Instance of Checker
        /// </summary>
        private readonly Checker m_refChecker;

        /// <summary>
        /// Hand treated
        /// </summary>
        private readonly JointType m_refHand;

        /// <summary>
        /// Movement direction to hand
        /// </summary>
        private EnumKinectDirectionGesture m_refDirection;

        /// <summary>
        /// Consecutive numbers of frame where the condition is satisfied
        /// </summary>
        private int m_nIndex;

        /// <summary>
        /// Informe if the gesture is begin
        /// </summary>
        private bool m_GestureBegin;

        /// <summary>
        /// Hand velocity in each frame
        /// </summary>
        private List<double> m_handVelocity;
                
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="refUser">User data</param>
        /// <param name="leftOrRightHand">Hand treated</param>
        public SwipeCondition(UserData refUser, JointType leftOrRightHand)
            : base(refUser)
        {
            m_nIndex = 0;
            m_refHand = leftOrRightHand;
            m_refChecker = new Checker(refUser, PropertiesPluginKinect.Instance.SwipeCheckerTolerance);
            m_handVelocity = new List<double>();
            m_GestureBegin = false;
        }

        /// <summary>
        /// See description in Condition class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Check(object sender, NewSkeletonEventArgs e)
        {
            // Relative position between HipCenter and Hand
            List<EnumKinectDirectionGesture> handToHipOrientation = m_refChecker.GetRelativePosition(JointType.HipCenter, m_refHand).ToList();

            // Relative position between Head and Hand
            List<EnumKinectDirectionGesture> handToHeadOrientation = m_refChecker.GetRelativePosition(JointType.Head, m_refHand).ToList();

            // Movement directions to hand
            List<EnumKinectDirectionGesture> handMovement = m_refChecker.GetAbsoluteMovement(m_refHand).ToList();

            // Relative velocity of hand
            m_handVelocity.Add(m_refChecker.GetRelativeVelocity(JointType.HipCenter, m_refHand));
            double handVelocity = m_refChecker.GetRelativeVelocity(JointType.HipCenter, m_refHand);

            if (m_refHand == JointType.HandRight)
            {
                IntuiLab.Kinect.Utils.DebugLog.DebugTraceLog("Swipe Left speed = " + handVelocity, false);
            }
            else if (m_refHand == JointType.HandLeft)
            {
                IntuiLab.Kinect.Utils.DebugLog.DebugTraceLog("Swipe Right speed = " + handVelocity, false);
            }

            // Speed condition
            if (handVelocity < PropertiesPluginKinect.Instance.SwipeLowerBoundForVelocity)
            {
                Reset();
            }
            // Condition : Hand is in front of the body and between HipCenter and Head
            else if (handToHipOrientation.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_FORWARD)
                && handToHeadOrientation.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_DOWNWARD))
            {
                // Movement did not start yet, initializing
                if (m_refDirection == EnumKinectDirectionGesture.KINECT_DIRECTION_NONE)
                {
                    // Condition : Hand is right && Movement direction hand => left
                    if (PropertiesPluginKinect.Instance.EnableGestureSwipeLeft &&
                        handMovement.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_LEFT) 
                        && !handMovement.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_UPWARD))
                    {
                        m_refDirection = EnumKinectDirectionGesture.KINECT_DIRECTION_LEFT;
                        m_GestureBegin = true;
                        // Notify the gesture swipe left is begin
                        RaiseGestureBegining(this, new BeginGestureEventArgs
                        {
                            Gesture = EnumGesture.GESTURE_SWIPE_LEFT,
                            Posture = EnumPosture.POSTURE_NONE
                        });
                    }
                    // Condition : Hand is left && Movement direction hand => right
                    else if (PropertiesPluginKinect.Instance.EnableGestureSwipeRight &&
                        handMovement.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_RIGHT) && 
                        !handMovement.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_UPWARD))
                    {
                        m_refDirection = EnumKinectDirectionGesture.KINECT_DIRECTION_RIGHT;
                        m_GestureBegin = true;
                        // Notify the gesture swipe right is begin
                        RaiseGestureBegining(this, new BeginGestureEventArgs
                        {
                            Gesture = EnumGesture.GESTURE_SWIPE_RIGHT,
                            Posture = EnumPosture.POSTURE_NONE
                        });
                    }
                    else
                    {
                        // Take other direction
                        Reset();
                    }
                }
                // Movement direction hand changed
                else if (!handMovement.Contains(m_refDirection))
                {
                    Reset();
                }
                else
                {
                    // Gesture Swipe is complete
                    if (m_nIndex >= PropertiesPluginKinect.Instance.SwipeLowerBoundForSuccess)
                    {
                        // Calculate mean velocity
                        double meanVelocity = 0;
                        foreach (double velocity in m_handVelocity)
                        {
                            meanVelocity += velocity;
                        }

                        meanVelocity = meanVelocity / (double)m_handVelocity.Count;

                        IntuiLab.Kinect.Utils.DebugLog.DebugTraceLog("Mean Velocity = " + meanVelocity, false);

                        if (m_refDirection == EnumKinectDirectionGesture.KINECT_DIRECTION_LEFT)
                        {
                            // Notify Gesture Swipe Left is detected
                            FireSucceeded(this, new SuccessGestureEventArgs
                            {
                                Gesture = EnumGesture.GESTURE_SWIPE_LEFT,
                                Posture = EnumPosture.POSTURE_NONE
                            });
                            IntuiLab.Kinect.Utils.DebugLog.DebugTraceLog("Condition Swipe Left complete", false);
                        }
                        else if (m_refDirection == EnumKinectDirectionGesture.KINECT_DIRECTION_RIGHT)
                        {
                            // Notify Gesture Swipe Right is detected
                            FireSucceeded(this, new SuccessGestureEventArgs
                            {
                                Gesture = EnumGesture.GESTURE_SWIPE_RIGHT,
                                Posture = EnumPosture.POSTURE_NONE
                            });
                            IntuiLab.Kinect.Utils.DebugLog.DebugTraceLog("Condition Swipe Right complete", false);
                        }

                        m_nIndex = 0;

                        m_refDirection = EnumKinectDirectionGesture.KINECT_DIRECTION_NONE;
                    }
                    // Step successful, waiting for next
                    else
                    {
                        m_nIndex++;
                    }
                }
            }
        }

        /// <summary>
        /// Restart detecting
        /// </summary>
        private void Reset()
        {
            // If gesture begin, notify gesture is end
            if (m_GestureBegin)
            {
                m_GestureBegin = false;
                if (m_refDirection == EnumKinectDirectionGesture.KINECT_DIRECTION_LEFT)
                {
                    RaiseGestureEnded(this, new EndGestureEventArgs
                    {
                        Gesture = EnumGesture.GESTURE_SWIPE_LEFT,
                        Posture = EnumPosture.POSTURE_NONE
                    });
                }
                else if (m_refDirection == EnumKinectDirectionGesture.KINECT_DIRECTION_RIGHT)
                {
                    RaiseGestureEnded(this, new EndGestureEventArgs
                    {
                        Gesture = EnumGesture.GESTURE_SWIPE_RIGHT,
                        Posture = EnumPosture.POSTURE_NONE
                    });
                }
            }

            m_nIndex = 0;
            m_handVelocity.Clear();
            m_refDirection = EnumKinectDirectionGesture.KINECT_DIRECTION_NONE;
            FireFailed(this, new FailedGestureEventArgs
            {
                refCondition = this
            });
        }
    }
}
