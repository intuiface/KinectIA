using IntuiLab.Kinect.DataUserTracking;
using IntuiLab.Kinect.Enums;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Linq;

namespace IntuiLab.Kinect.GestureRecognizer.Gestures
{
    internal class WaveLeftCondition : Condition
    {
        #region Fields
        
        /// <summary>
        /// Instance of Checker
        /// </summary>
        private readonly Checker m_refChecker;

        /// <summary>
        /// Hand treated
        /// </summary>
        private JointType m_refHand;

        /// <summary>
        /// Movement direction to hand
        /// </summary>
        private EnumKinectDirectionGesture m_refDirection;

        /// <summary>
        /// Consecutive numbers of frame where the condition is satidfied
        /// </summary>
        private int m_nIndex;

        /// <summary>
        /// Informe if the gesture is begin
        /// </summary>
        private bool m_GestureBegin;

        /// <summary>
        /// Number of trial before reset detection
        /// </summary>
        private int m_nTryCondition;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="refUser">User data</param>
        /// <param name="hand"> hand treated</param>
        public WaveLeftCondition(UserData refUser, JointType hand)
            : base(refUser)
        {
            m_nIndex = 0;
            m_nTryCondition = 0;
            m_refChecker = new Checker(refUser, PropertiesPluginKinect.Instance.WaveCheckerTolerance);
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
            // Relative position between Shoulder and Hand
            List<EnumKinectDirectionGesture> handToShoulderDirections = m_refChecker.GetRelativePosition(JointType.ShoulderRight, m_refHand).ToList();

            // Relative position between HipCenter and Hand
            List<EnumKinectDirectionGesture> handToHipOrientation = m_refChecker.GetRelativePosition(JointType.HipCenter, m_refHand).ToList();

            // Movement directions of hand
            List<EnumKinectDirectionGesture> handMovement = m_refChecker.GetAbsoluteMovement(m_refHand).ToList();

            // Relative velocity of hand
            double handspeed = m_refChecker.GetRelativeVelocity(JointType.HipCenter, m_refHand);

            IntuiLab.Kinect.Utils.DebugLog.DebugTraceLog("Wave speed = " + handspeed, false);

            // Speed condition
            if (handspeed < PropertiesPluginKinect.Instance.WaveLowerBoundForVelocity)
            {
                Reset();
            }
            // Condition : Hand is forward the shouler
            else if (handToShoulderDirections.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_FORWARD))
            {
                // Movement did not start yet, initializing
                if (m_refDirection == EnumKinectDirectionGesture.KINECT_DIRECTION_NONE)
                {
                    // Condition : Movement direction hand => left
                    if (handMovement.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_LEFT) && !handMovement.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_UPWARD))
                    {
                        m_refDirection = EnumKinectDirectionGesture.KINECT_DIRECTION_LEFT;
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
                    // Gesture Wave Left is complete
                    if (m_nIndex >= PropertiesPluginKinect.Instance.WaveLowerBoundForSuccess)
                    {                   
                        m_nIndex = 0;

                        IntuiLab.Kinect.Utils.DebugLog.DebugTraceLog("Condition Wave Left complete", false);

                        // Notify Condition Wave left part is complete
                        FireSucceeded(this, null);
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
