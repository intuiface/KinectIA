            // ****************************************************************************
            // <copyright file="MaximizeCondition.cs" company="IntuiLab">
            // INTUILAB CONFIDENTIAL
			//_____________________
			// [2002] - [2015] IntuiLab SA
			// All Rights Reserved.
			// NOTICE: All information contained herein is, and remains
			// the property of IntuiLab SA. The intellectual and technical
			// concepts contained herein are proprietary to IntuiLab SA
			// and may be covered by U.S. and other country Patents, patents
			// in process, and are protected by trade secret or copyright law.
			// Dissemination of this information or reproduction of this
			// material is strictly forbidden unless prior written permission
			// is obtained from IntuiLab SA.
            // </copyright>
            // ****************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using IntuiLab.Kinect.Enums;
using IntuiLab.Kinect.DataUserTracking;

namespace IntuiLab.Kinect.GestureRecognizer.Gestures
{
    internal class MaximizeCondition : Condition
    {
        #region Field
        
        /// <summary>
        /// Instance of Checker
        /// </summary>
        private readonly Checker m_refChecker;
        
        /// <summary>
        /// Movement direction to hand left
        /// </summary>
        private EnumKinectDirectionGesture m_refLeftDirection;

        /// <summary>
        /// Movement direction to hand right
        /// </summary>
        private EnumKinectDirectionGesture m_refRightDirection;

        /// <summary>
        /// Consecutive numbers of frame where the condition is satisfied
        /// </summary>
        private int m_nIndex;

        /// <summary>
        /// Informe if the gesture is begin or not
        /// </summary>
        private bool m_GestureBegin;        

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="refUser">User data</param>
        public MaximizeCondition(UserData refUser)
            : base(refUser)
        {
            m_nIndex = 0;
            m_refChecker = new Checker(refUser, PropertiesPluginKinect.Instance.MaximizeCheckerTolerance);
            m_GestureBegin = false;
        }

        /// <summary>
        /// See description in Condition class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Check(object sender, NewSkeletonEventArgs e)
        {
            // Relative position between HipCenter and HandLeft
            List<EnumKinectDirectionGesture> handLeftToHipOrientation = m_refChecker.GetRelativePosition(JointType.HipCenter, JointType.HandLeft).ToList();
            
            // Relative position between Head and Hand Left
            List<EnumKinectDirectionGesture> handLeftToHeadOrientation = m_refChecker.GetRelativePosition(JointType.Head, JointType.HandLeft).ToList();
            
            // Movement directions to HandLeft
            List<EnumKinectDirectionGesture> handLeftMovement = m_refChecker.GetAbsoluteMovement(JointType.HandLeft).ToList();
            
            // Relative velocity of HandLeft
            double handLeftVelocity = m_refChecker.GetRelativeVelocity(JointType.HipCenter, JointType.HandLeft);

            // Relative position between HipCenter and HandRight
            List<EnumKinectDirectionGesture> handRightToHipOrientation = m_refChecker.GetRelativePosition(JointType.HipCenter, JointType.HandRight).ToList();

            // Relative position between Head and HandRight
            List<EnumKinectDirectionGesture> handRightToHeadOrientation = m_refChecker.GetRelativePosition(JointType.Head, JointType.HandRight).ToList();

            // Movement directions to HandRight
            List<EnumKinectDirectionGesture> handRightMovement = m_refChecker.GetAbsoluteMovement(JointType.HandRight).ToList();

            // Relative velocity of HandRight
            double handRightVelocity = m_refChecker.GetRelativeVelocity(JointType.HipCenter, JointType.HandRight);

            // Speed condition
            if (handLeftVelocity < PropertiesPluginKinect.Instance.MaximizeLowerBoundForVelocity || handRightVelocity < PropertiesPluginKinect.Instance.MaximizeLowerBoundForVelocity)
            {
                Reset();
            }
            // Condition : Hand is in front of the body and between HipCenter and Head
            else if (handLeftToHipOrientation.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_FORWARD)
                && handLeftToHeadOrientation.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_DOWNWARD)
                && handRightToHipOrientation.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_FORWARD)
                && handRightToHeadOrientation.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_DOWNWARD))
            {
                // Movement did not start yet, initializing
                if (m_refLeftDirection == EnumKinectDirectionGesture.KINECT_DIRECTION_NONE && m_refRightDirection == EnumKinectDirectionGesture.KINECT_DIRECTION_NONE)
                {
                    // Condition : Movement direction hand right => right && Movement direction hand left => left
                    if (handRightMovement.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_RIGHT) && handLeftMovement.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_LEFT))
                    {
                        m_refLeftDirection = EnumKinectDirectionGesture.KINECT_DIRECTION_LEFT;
                        m_refRightDirection = EnumKinectDirectionGesture.KINECT_DIRECTION_RIGHT;
                        m_GestureBegin = true;
                        // Notify the gesture Maximize is begin
                        RaiseGestureBegining(this, new BeginGestureEventArgs 
                        {
                            Gesture = EnumGesture.GESTURE_MAXIMIZE,
                            Posture = EnumPosture.POSTURE_NONE
                        });
                    }
                    else
                    {
                        // Take other direction
                        Reset();
                    }
                }
                // Movement direction hand left or right changed
                else if (!handLeftMovement.Contains(m_refLeftDirection) || !handRightMovement.Contains(m_refRightDirection))
                {
                    Reset();
                }
                else
                {
                    // Gesture Maximize is complete
                    if (m_nIndex >= PropertiesPluginKinect.Instance.MaximizeLowerBoundForSuccess)
                    {
                        // Notify Gesture Maximize is detected
                        FireSucceeded(this, new SuccessGestureEventArgs
                        {
                            Gesture = EnumGesture.GESTURE_MAXIMIZE,
                            Posture = EnumPosture.POSTURE_NONE
                        });

                        IntuiLab.Kinect.Utils.DebugLog.DebugTraceLog("Condition Maximize complete", false);

                        m_nIndex = 0;                          

                        m_refLeftDirection = EnumKinectDirectionGesture.KINECT_DIRECTION_NONE;
                        m_refRightDirection = EnumKinectDirectionGesture.KINECT_DIRECTION_NONE;
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
                RaiseGestureEnded(this, new EndGestureEventArgs
                {
                    Gesture = EnumGesture.GESTURE_MAXIMIZE,
                    Posture = EnumPosture.POSTURE_NONE
                });
            }

            m_nIndex = 0;
            
            m_refLeftDirection = EnumKinectDirectionGesture.KINECT_DIRECTION_NONE;
            m_refRightDirection = EnumKinectDirectionGesture.KINECT_DIRECTION_NONE;

            FireFailed(this, new FailedGestureEventArgs
            {
                refCondition = this
            });

            
        }
    }
}
