            // ****************************************************************************
            // <copyright file="PushCondition.cs" company="IntuiLab">
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
using System.Windows.Media.Media3D;

namespace IntuiLab.Kinect.GestureRecognizer.Gestures
{
    internal class PushCondition : Condition
    {
        #region Field

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

        /// <summary>
        /// Hand position when the gesture begin
        /// </summary>
        private Point3D m_refStartPoint;
        
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="refUser">User data</param>
        /// <param name="leftOrRightHand">Hand treated</param>
        public PushCondition(UserData refUser, JointType leftOrRightHand)
            : base(refUser)
        {
            m_nIndex = 0;
            m_refHand = leftOrRightHand;
            m_refChecker = new Checker(refUser, PropertiesPluginKinect.Instance.PushCheckerTolerance);
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

            // Movement directions to hand
            List<EnumKinectDirectionGesture> handMovement = m_refChecker.GetAbsoluteMovement(m_refHand).ToList();

            // Relative velocity of hand
            m_handVelocity.Add(m_refChecker.GetRelativeVelocity(JointType.HipCenter, m_refHand));
            double handVelocity = m_refChecker.GetRelativeVelocity(JointType.HipCenter, m_refHand);
            
            if (m_refHand == JointType.HandRight)
            {
                IntuiLab.Kinect.Utils.DebugLog.DebugTraceLog("Push speed = " + handVelocity, false);
            }

            // Speed condition
            if (handVelocity < PropertiesPluginKinect.Instance.PushLowerBoundForVelocity)
            {
                Reset();
            }
            // Condition : Hand is in front of the body
            else if (handToHipOrientation.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_FORWARD)
                && handToHipOrientation.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_UPWARD)) 
            {
                // Movement did not start yet, initializing
                if (m_refDirection == EnumKinectDirectionGesture.KINECT_DIRECTION_NONE)
                {
                    // Condition : Movement direction hand => forward
                    if (handMovement.Contains(EnumKinectDirectionGesture.KINECT_DIRECTION_FORWARD))
                    {
                        m_GestureBegin = true;
                        
                        m_refDirection = EnumKinectDirectionGesture.KINECT_DIRECTION_FORWARD;
                        
                        // Save the start hand position
                        m_refStartPoint = new Point3D(
                                e.m_refSkeletonData.GetJointPosition(m_refHand).X,
                                e.m_refSkeletonData.GetJointPosition(m_refHand).Y,
                                e.m_refSkeletonData.GetJointPosition(m_refHand).Z);
                        
                        // Notify the gesture Push is begin
                        RaiseGestureBegining(this, new BeginGestureEventArgs
                        {
                            Gesture = EnumGesture.GESTURE_PUSH,
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
                    // Gesture Push is complete
                    if (m_nIndex >= PropertiesPluginKinect.Instance.PushLowerBoundForSuccess)
                    {
                        // Get the end hand position
                        Point3D endPoint = new Point3D(
                                e.m_refSkeletonData.GetJointPosition(m_refHand).X,
                                e.m_refSkeletonData.GetJointPosition(m_refHand).Y,
                                e.m_refSkeletonData.GetJointPosition(m_refHand).Z);

                        // difference between the start and end point
                        double dx = endPoint.X - m_refStartPoint.X;
                        double dy = endPoint.Y - m_refStartPoint.Y;
                        double dz = endPoint.Z - m_refStartPoint.Z;

                        // Condition : Square 15cm aroud the start position in X-Axis and Y-Axis && the hand forward at least 10cm in Z-Axis
                        if (Math.Abs(dx) < 0.15 && Math.Abs(dy) < 0.15 && Math.Abs(dz) > 0.1)
                        {
                            // Calculate mean velocity
                            double meanVelocity = 0;
                            foreach (double velocity in m_handVelocity)
                            {
                                meanVelocity += velocity;
                            }

                            meanVelocity = meanVelocity / (double)m_handVelocity.Count;

                            IntuiLab.Kinect.Utils.DebugLog.DebugTraceLog("Mean Velocity = " + meanVelocity, false);

                            // Notify Gesture Push is detected
                            FireSucceeded(this, new SuccessGestureEventArgs
                            {
                                Gesture = EnumGesture.GESTURE_PUSH,
                                Posture = EnumPosture.POSTURE_NONE
                            });

                            IntuiLab.Kinect.Utils.DebugLog.DebugTraceLog("Condition Push complete", false);

                            m_nIndex = 0;
                            m_refDirection = EnumKinectDirectionGesture.KINECT_DIRECTION_NONE;
                        }
                        else
                        {
                            Reset();
                        }
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
                    Gesture = EnumGesture.GESTURE_PUSH,
                    Posture = EnumPosture.POSTURE_NONE
                });
            }

            m_nIndex = 0;
            m_refDirection = EnumKinectDirectionGesture.KINECT_DIRECTION_NONE;

            m_refStartPoint = new Point3D();
            m_handVelocity.Clear();

            FireFailed(this, new FailedGestureEventArgs
            {
                refCondition = this
            });
        }
    }
}
