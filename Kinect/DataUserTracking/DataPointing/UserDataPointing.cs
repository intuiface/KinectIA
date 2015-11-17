            // ****************************************************************************
            // <copyright file="UserDataPointing.cs" company="IntuiLab">
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
using IntuiLab.Kinect.DataUserTracking.Events;
using System.Windows.Media.Media3D;
using Microsoft.Kinect.Toolkit.Interaction;
using System.Drawing;

namespace IntuiLab.Kinect.DataUserTracking
{
    /// <summary>
    /// This classe permit to manage the data user in pointing mode.
    /// It resume operation of the UserData class, but the the gestures recognizer engine enabled just the postures.
    /// </summary>
    internal class UserDataPointing : UserData, IDisposable
    {
        #region Fields
        /// <summary>
        /// User hands data
        /// This dictionnary record the hand left and hand right data for an user
        /// </summary>
        private Dictionary<InteractionHandType, HandData> m_UserHandsData;

        #endregion

        #region Events

        #region UserHandActive
        /// <summary>
        /// Event triggered when a hand became active
        /// </summary>
        public event EventHandler<HandActiveEventArgs> UserHandActive;

        /// <summary>
        /// Raise event UserHandActive
        /// </summary>
        protected void RaiseUserHandActive(object sender, HandActiveEventArgs e)
        {
            if (UserHandActive != null)
            {
                UserHandActive(this, e);
            }
        }
        #endregion

        #region UserHandMove

        /// <summary>
        /// Event triggered when a hand moved
        /// </summary>
        public event EventHandler<HandMoveEventArgs> UserHandMove;

        /// <summary>
        /// Raise event UserHandMove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RaisUserHandMove(object sender, HandMoveEventArgs e)
        {
            if (UserHandMove != null)
            {
                UserHandMove(this, e);
            }
        }

        #endregion

        #region UserHandGripStateChanged

        /// <summary>
        /// Event triggered when a hand's grip state changed
        /// </summary>
        public event EventHandler<HandGripStateChangeEventArgs> UserHandGripStateChanged;

        /// <summary>
        /// Raise event UserHandGripStateChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RaisUserHandGripStateChnged(object sender, HandGripStateChangeEventArgs e)
        {
            if (UserHandGripStateChanged != null)
            {
                UserHandGripStateChanged(this, e);
            }
        }

        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="refSkeleton">User Skeleton</param>
        /// <param name="depth">User/Kinect sensor distance</param>
        /// <param name="timesTamp">SkeletonFrame TimesTamp</param>
        public UserDataPointing(int userId, Skeleton refSkeleton, double depth, long timesTamp)
            : base(userId,refSkeleton,depth,timesTamp)
        {
            // Initialize the hand data for the user's hands
            m_UserHandsData = new Dictionary<InteractionHandType, HandData>();

            m_UserHandsData[InteractionHandType.Left] = new HandData(InteractionHandType.Left);
            m_UserHandsData[InteractionHandType.Right] = new HandData(InteractionHandType.Right);

            m_UserHandsData[InteractionHandType.Left].HandIsActive += OnUserHandActive;
            m_UserHandsData[InteractionHandType.Right].HandIsActive += OnUserHandActive;

            m_UserHandsData[InteractionHandType.Left].HandMove += OnUserHandMove;
            m_UserHandsData[InteractionHandType.Right].HandMove += OnUserHandMove;

            m_UserHandsData[InteractionHandType.Left].HandGripStateChanged += OnUserHandGripStateChanged;
            m_UserHandsData[InteractionHandType.Right].HandGripStateChanged += OnUserHandGripStateChanged;
            
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Update the data hands of the user
        /// </summary>
        /// <param name="userInfos">The datas for the hand left and right</param>
        public void UpdateDataHands(UserInfo userInfos)
        {
            // get the hands datas
            var hands = userInfos.HandPointers;

            if (hands.Count != 0)
            {
                // for each hands, update the datas
                foreach (var hand in hands)
                {
                    m_UserHandsData[hand.HandType].UpdateHandData(new PointF((float)hand.X, (float)hand.Y), hand.HandEventType, hand.IsActive, hand.IsPrimaryForUser);
                }
            }   
        }

        #endregion

        #region Event Handler's

        /// <summary>
        /// Callback when a hand is active
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUserHandActive(object sender, HandActiveEventArgs e)
        {
            RaiseUserHandActive(this, new HandActiveEventArgs
            {
                HandType = e.HandType,
                IsActive = e.IsActive,
                PositionOnScreen = e.PositionOnScreen,
                userID = this.UserID
            });
        }

        /// <summary>
        /// Callback when a hand moved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUserHandMove(object sender, HandMoveEventArgs e)
        {
            RaisUserHandMove(this, new HandMoveEventArgs
            {
                HandType = e.HandType,
                userID = this.UserID,
                PositionOnScreen = e.PositionOnScreen,
                RawPosition = e.RawPosition,
                IsGrip = e.IsGrip
            });
        }

        /// <summary>
        /// Callback when the hand's grip state changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUserHandGripStateChanged(object sender, HandGripStateChangeEventArgs e)
        {
            RaisUserHandGripStateChnged(this, new HandGripStateChangeEventArgs
            {
                HandType = e.HandType,
                userID = this.UserID,
                IsGrip = e.IsGrip,
                RawPosition = e.RawPosition

            });
        }

        #endregion

        #region IDisposable's members

        /// <summary>
        /// Dispose this instance.
        /// </summary>
        public void DisposeUserDataPointing()
        {
            m_UserHandsData[InteractionHandType.Left].HandIsActive -= OnUserHandActive;
            m_UserHandsData[InteractionHandType.Right].HandIsActive -= OnUserHandActive;

            m_UserHandsData[InteractionHandType.Left].HandMove -= OnUserHandMove;
            m_UserHandsData[InteractionHandType.Right].HandMove -= OnUserHandMove;

            m_UserHandsData[InteractionHandType.Left].HandGripStateChanged -= OnUserHandGripStateChanged;
            m_UserHandsData[InteractionHandType.Right].HandGripStateChanged -= OnUserHandGripStateChanged;

            // Call the dispose to the UserData class.
            base.Dispose();
        }
        #endregion
    }
}
