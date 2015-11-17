            // ****************************************************************************
            // <copyright file="HandData.cs" company="IntuiLab">
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
using System.Windows.Media.Media3D;

using IntuiLab.Kinect.Enums;
using Microsoft.Kinect.Toolkit.Interaction;
using IntuiLab.Kinect.DataUserTracking.Events;
using System.Drawing;

namespace IntuiLab.Kinect.DataUserTracking
{
    internal class HandData
    {
        #region Properties

        #region HandType

        /// <summary>
        /// Indicates the hand type
        /// </summary>
        private InteractionHandType m_refHandType;
        public InteractionHandType HandType
        {
            get
            {
                return m_refHandType;
            }
            set
            {
                if (m_refHandType != value)
                {
                    m_refHandType = value;
                }
            }
        }
        #endregion

        #region HandRawPosition
       
        /// <summary>
        /// Indicates the hand's raw position
        /// Raw position correspond at the normalize coordinate ( [0,1] ) send to MGRE
        /// </summary>
        private PointF m_HandRawPosition;
        public PointF HandRawPosition
        {
            get
            {
                return m_HandRawPosition;
            }
            set
            {
                m_HandRawPosition = value;   
            }
        }
        #endregion

        #region HandScreenPosition
        /// <summary>
        /// Indicates the hand's screen position
        /// Screen position correspond at the hand's coordinate to the screen in pixel.
        /// </summary>
        private PointF m_HandScreenPosition;
        public PointF HandScreenPosition
        {
            get
            {
                return m_HandScreenPosition;  
            }
            set
            {
                if (m_HandScreenPosition != value)
                {
                    m_HandScreenPosition = value;
                    if (IsActive)
                    {
                        // Notify that the hand moved
                        RaiseHandMove(this, new HandMoveEventArgs
                        {
                            HandType = this.HandType,
                            PositionOnScreen = m_HandScreenPosition,
                            RawPosition = HandRawPosition,
                            IsGrip = this.IsGrip
                        });
                    }
                }
            }
        }
        #endregion

        #region IsPrimaryHand

        /// <summary>
        /// Indicate if the hand is primary or not.
        /// The first hand enter in the pointing area is the primary hand.
        /// </summary>
        private bool m_IsPrimaryHand;
        public bool IsPrimaryHand
        {
            get
            {
                return m_IsPrimaryHand;
            }
            set
            {
                if (m_IsPrimaryHand != value)
                {
                    m_IsPrimaryHand = value;
                }
            }
        }

        #endregion

        #region IsActive

        /// <summary>
        /// Indicate if the hand is active or not.
        /// For activate a hand, it must be in the pointing area
        /// </summary>
        private bool m_IsActive;
        public bool IsActive
        {
            get
            {
                return m_IsActive;
            }
            set
            {
                if (m_IsActive != value)
                {
                    m_IsActive = value;
                    // Notify that the hand is active
                    RaiseHandIsActive(this, new HandActiveEventArgs
                    {
                        HandType = this.HandType,
                        IsActive = this.IsActive,
                        PositionOnScreen = this.HandScreenPosition
                    });
                    if (m_IsActive)
                    {
                        // Notify the grip state for that the feedback display is good
                        RaiseHandGripStateChanged(this, new HandGripStateChangeEventArgs
                        {
                            HandType = this.HandType,
                            IsGrip = this.IsGrip
                        });
                    }
                }
            }

        }
        #endregion

        #region IsGrip

        /// <summary>
        /// Indicates if the hand is grip or not.
        /// The grip state correspond to the close/open hand
        /// IsGrip = true  => hand opened
        /// IsGrip = false => hand closed
        /// </summary>
        private bool m_IsGrip;
        public bool IsGrip
        {
            get
            {
                return m_IsGrip;
            }
            set
            {
                if (m_IsGrip != value)
                {
                    m_IsGrip = value;
                    if (IsActive)
                    {
                        // Notify the hand's grip state is changed
                        RaiseHandGripStateChanged(this, new HandGripStateChangeEventArgs
                        {
                            HandType = this.HandType,
                            IsGrip = this.IsGrip,
                            RawPosition = HandRawPosition
                        });
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Events

        #region HandIsActive

        /// <summary>
        /// Event triggered when the hand is active
        /// </summary>
        public event EventHandler<HandActiveEventArgs> HandIsActive;

        /// <summary>
        /// Raise event HandIsActive
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RaiseHandIsActive(object sender, HandActiveEventArgs e)
        {
            if (HandIsActive != null)
            {
                HandIsActive(sender, e);
            }
        }

        #endregion

        #region HandMove

        /// <summary>
        /// Event triggered when the hand moved
        /// </summary>
        public event EventHandler<HandMoveEventArgs> HandMove;

        /// <summary>
        /// Raise event HandMove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RaiseHandMove(object sender, HandMoveEventArgs e)
        {
            if (HandMove != null)
            {
                HandMove(sender, e);
            }
        }

        #endregion

        #region HandGripStateChanged

        /// <summary>
        /// Event triggered whend the hand's grip state changed
        /// </summary>
        public event EventHandler<HandGripStateChangeEventArgs> HandGripStateChanged;

        /// <summary>
        /// Raise event HandGripStateChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RaiseHandGripStateChanged(object sender, HandGripStateChangeEventArgs e)
        {
            if (HandGripStateChanged != null)
            {
                HandGripStateChanged(sender, e);
            }
        }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Hand typer</param>
        /// <param name="tracked">Hand is active or not</param>
        /// <param name="position">Hand position</param>
        public HandData(InteractionHandType type)
        {
            HandType = type;
            m_IsActive = false;
            m_IsPrimaryHand = false;
            m_IsGrip = false;

            m_HandRawPosition = new PointF();
            m_HandScreenPosition = new PointF();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Update the hand datas
        /// </summary>
        /// <param name="rawPosition">Hand's raw position</param>
        /// <param name="handEventType">Event on the hand</param>
        /// <param name="isActive">Hand is active or not</param>
        /// <param name="isPrimaryHand">Hand is primary or not</param>
        public void UpdateHandData(PointF rawPosition, InteractionHandEventType handEventType, bool isActive, bool isPrimaryHand)
        {
            // Get the hand's raw position in the kinect hand landmark
            HandRawPosition = rawPosition;

            // Transform the hand's raw position in the kinec hand landmark to the MGRE landmark
            // This transformation take account the parameters 'SpaceBetweenHands' and 'PointingHandsAmplitude'
            if (HandType == InteractionHandType.Left)
            {
                // I consider the hand left landmark corresponding to the left half of the screen (/2)
                m_HandRawPosition.X = ((m_HandRawPosition.X + PropertiesPluginKinect.Instance.PointingSpaceBetweenHands + PropertiesPluginKinect.Instance.PointingHandsAmplitude) 
                                        / 
                                        ( (1-PropertiesPluginKinect.Instance.PointingSpaceBetweenHands) + 
                                          (PropertiesPluginKinect.Instance.PointingHandsAmplitude + PropertiesPluginKinect.Instance.PointingSpaceBetweenHands))
                                      ) / 2;
            }
            else
            {
                // I concider the hand right landmark corresponding to the right half of the screen (/2 + 0.5f)
                m_HandRawPosition.X = ((m_HandRawPosition.X - PropertiesPluginKinect.Instance.PointingSpaceBetweenHands) / ((1 - PropertiesPluginKinect.Instance.PointingSpaceBetweenHands) +
                                          (PropertiesPluginKinect.Instance.PointingHandsAmplitude + PropertiesPluginKinect.Instance.PointingSpaceBetweenHands))) / 2 + 0.5f;
            }

            PointF handScreen = new PointF();

            // Transform the hand's raw position to the hand's screen position
            handScreen.X = m_HandRawPosition.X * PropertiesPluginKinect.Instance.ExperienceIntuiFaceWidth;
            handScreen.Y = m_HandRawPosition.Y * PropertiesPluginKinect.Instance.ExperienceIntuifaceHeight;

            HandScreenPosition = handScreen;

            // Get the hand's event type
            if (handEventType == InteractionHandEventType.Grip)
            {
                IsGrip = true;
            }
            else if (handEventType == InteractionHandEventType.GripRelease)
            {
                IsGrip = false;
            }

            // Update the primary hand and active hand
            IsPrimaryHand = isPrimaryHand;
            IsActive = isActive;
        }

        #endregion
    }
}
