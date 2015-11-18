using IntuiLab.Kinect.DataUserTracking;
using IntuiLab.Kinect.Enums;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Linq;

namespace IntuiLab.Kinect.GestureRecognizer.Gestures
{
    /// <summary>
    /// The condition when waving left
    /// </summary>
    internal class WaveRightCondition : Condition
    {
        #region Fields

        /// <summary>
        /// Instance of Checker
        /// </summary>
        private Checker m_refChecker;

        /// <summary>
        /// Hand treated
        /// </summary>
        private JointType m_refHand;

        /// <summary>
        /// Movement direction to hand
        /// </summary>
        private EnumKinectDirectionGesture m_refDirection;

        /// <summary>
        /// Consecutive numbers of frame where the condition is satisfied
        /// </summary>
        private int m_nIndex;

        /// <summary>
        /// Informe if gesture is begin
        /// </summary>
        private bool m_GestureBegin;

        /// <summary>
        /// Number of trial before reset detection
        /// </summary>
        private int m_nTryCondition;

        /// <summary>
        /// Hand velocity in each frame
        /// </summary>
        private List<double> m_handVelocity;


        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="refUser">User data</param>
        /// <param name="hand">Hand treated</param>
        public WaveRightCondition(UserData refUser, JointType hand)
            : base(refUser)
        {
            m_nIndex = 0;
            m_refChecker = new Checker(refUser, PropertiesPluginKinect.Instance.WaveCheckerTolerance);
            m_nTryCondition = 0;
            m_handVelocity = new List<double>();
            m_refDirection = EnumKinectDirectionGesture.KINECT_DIRECTION_NONE;
            m_refHand = hand;
            m_GestureBegin = false;
        }

        /// <summary>
        /// See description in Condition class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Check(object sender, NewSkeletonEventArgs e)
        {
            // Relative position between Shoulder and hand 
            List<EnumKinectDirectionGesture> handToShoulderDirections = m_refChecker.GetRelativePosition(JointType.ShoulderRight, m_refHand).ToList();
            
            // Relative position between HipCenter and hand
            List<EnumKinectDirectionGesture> handToHipOrientation = m_refChecker.GetRelativePosition(JointType.HipCenter, m_refHand).ToList();

            // Movement directions of hand
            List<EnumKinectDirectionGesture> handMovement = m_refChecker.GetAbsoluteMovement(m_refHand).ToList();

            // Relative velocity of hand
            m_handVelocity.Add(m_refChecker.GetRelativeVelocity(JointType.HipCenter, m_refHand));
            double handspeed = m_refChecker.GetRelativeVelocity(JointType.HipCenter, m_refHand);

            IntuiLab.Kinect.Utils.DebugLog.DebugTraceLog("Wave speed = " + handspeed, false);

            // Speed condition
            if (handspeed < PropertiesPluginKinect.Instance.WaveLowerBoundForVelocity)
            {
                Reset();
            }
            // Condition : Hand is upward the shoulder
            else if (/*handToHipOrientation.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_FORWARD)
                && */handToShoulderDirections.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_UPWARD))
            {
                // Movement did not start yet, initializing
                if (m_refDirection == EnumKinectDirectionGesture.KINECT_DIRECTION_NONE)
                {
                    // Condition : Movement direction hand => right
                    if (handMovement.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_RIGHT) && !handMovement.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_UPWARD))
                    {
                        m_refDirection = EnumKinectDirectionGesture.KINECT_DIRECTION_RIGHT;
                        m_GestureBegin = true;
                        // Notify the gesture Wave is begin
                        RaiseGestureBegining(this, new BeginGestureEventArgs
                        {
                            Gesture = EnumGesture.GESTURE_WAVE,
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
                    // Gesture Wave Right is complete
                    if (m_nIndex >= PropertiesPluginKinect.Instance.WaveLowerBoundForSuccess)
                    {
                        // Calculate mean velocity
                        double meanVelocity = 0;
                        foreach (double velocity in m_handVelocity)
                        {
                            meanVelocity += velocity;
                        }

                        meanVelocity = meanVelocity / (double)m_handVelocity.Count;

                        IntuiLab.Kinect.Utils.DebugLog.DebugTraceLog("Mean Velocity = " + meanVelocity, false);
                        
                        // Notify Condition Wave right is complete
                        FireSucceeded(this, new SuccessGestureEventArgs
                        {
                            Gesture = EnumGesture.GESTURE_WAVE,
                            Posture = EnumPosture.POSTURE_NONE
                        });
                        IntuiLab.Kinect.Utils.DebugLog.DebugTraceLog("Condition Wave Right complete", false);

                        m_nIndex = 0;                        

                        m_refDirection = EnumKinectDirectionGesture.KINECT_DIRECTION_NONE;
                    }
                    // Step succesful, waiting for next
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
            // If gesture begin, notify is end
            if (m_GestureBegin)
            {
                m_GestureBegin = false;
                RaiseGestureEnded(this, new EndGestureEventArgs
                {
                    Gesture = EnumGesture.GESTURE_WAVE,
                    Posture = EnumPosture.POSTURE_NONE
                });
            }
            m_nIndex = 0;
            m_handVelocity.Clear();
            
            m_refDirection = EnumKinectDirectionGesture.KINECT_DIRECTION_NONE;
            if (m_nTryCondition >= 10)
            {
                m_nTryCondition = 0;
                FireFailed(this, new FailedGestureEventArgs
                {
                    refCondition = this
                });
            }
            else
            {
                m_nTryCondition++;
            }
        }
    }
}
