            // ****************************************************************************
            // <copyright file="KinectGestureManager.cs" company="IntuiLab">
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

using IntuiLab.Kinect.Events;
using IntuiLab.Kinect.DataUserTracking.Events;
using IntuiLab.Kinect.Enums;

namespace IntuiLab.Kinect
{
    /// <summary>
    /// Manage the events gesture recognize 
    /// </summary>
    public partial class KinectModule
    {
        #region Events

        #region GestureSwipeLeftDetected

        /// <summary>
        /// Event triggered when an User perform a Swipe Left
        /// </summary>
        public event GestureSwipeLeftDetectedEventHandler GestureSwipeLeftDetected;

        /// <summary>
        /// Raise event GestureSwipeLeftDetected
        /// </summary>
        protected void RaiseGestureSwipeLeftDetected()
        {
            if (GestureSwipeLeftDetected != null)
            {
                GestureSwipeLeftDetected(this, new GestureSwipeLeftDetectedEventArgs());
            }
        }

        #endregion

        #region GestureSwipeRightDetected

        /// <summary>
        /// Event triggered when an User perform a Swipe Right
        /// </summary>
        public event GestureSwipeRightDetectedEventHandler GestureSwipeRightDetected;

        /// <summary>
        /// Raise event GestureSwipeRightDetected
        /// </summary>
        protected void RaiseGestureSwipeRightDetected()
        {
            if (GestureSwipeRightDetected != null)
            {
                GestureSwipeRightDetected(this, new GestureSwipeRightDetectedEventArgs());
            }
        }

        #endregion

        #region GestureWaveDetected

        /// <summary>
        /// Event triggered when an User perform a Wave
        /// </summary>
        public event GestureWaveDetectedEventHandler GestureWaveDetected;

        /// <summary>
        /// Raise event GestureWaveDetected
        /// </summary>
        protected void RaiseGestureWaveDetected()
        {
            if (GestureWaveDetected != null)
            {
                GestureWaveDetected(this, new GestureWaveDetectedEventArgs());
            }
        }

        #endregion

        #region GesturePushDetected

        /// <summary>
        /// Event triggered when an User perform a Push
        /// </summary>
        public event GesturePushDetectedEventHandler GesturePushDetected;

        /// <summary>
        /// Raise event GesturePushDetected
        /// </summary>
        protected void RaiseGesturePushDetected()
        {
            if (GesturePushDetected != null)
            {
                GesturePushDetected(this, new GesturePushDetectedEventArgs());
            }
        }

        #endregion

        #region GestureMaximizeDetected

        /// <summary>
        /// Event triggered when an User perform a Push
        /// </summary>
        public event GestureMaximizeDetectedEventHandler GestureMaximizeDetected;

        /// <summary>
        /// Raise event GestureMaximizeDetected
        /// </summary>
        protected void RaiseGestureMaximizeDetected()
        {
            if (GestureMaximizeDetected != null)
            {
                GestureMaximizeDetected(this, new GestureMaximizeDetectedEventArgs());
            }
        }

        #endregion

        #region GestureMinimizeDetected

        /// <summary>
        /// Event triggered when an User perform a Push
        /// </summary>
        public event GestureMinimizeDetectedEventHandler GestureMinimizeDetected;

        /// <summary>
        /// Raise event GesturePushDetected
        /// </summary>
        protected void RaiseGestureMinimizeDetected()
        {
            if (GestureMinimizeDetected != null)
            {
                GestureMinimizeDetected(this, new GestureMinimizeDetectedEventArgs());
            }
        }

        #endregion

        #region GestureA

        #region GestureADetected
        /// <summary>
        /// Event triggered when an User perform a posture A
        /// </summary>
        public event GestureADetectedEventHandler GestureADetected;

        /// <summary>
        /// Raise event GestureADetected
        /// </summary>
        protected void RaiseGestureADetected()
        {
            if (GestureADetected != null)
            {
                GestureADetected(this, new GestureADetectedEventArgs());
            }
        }
        #endregion

        #region GestureAProgress
        /// <summary>
        /// Event triggered when an User doing a posture A.
        /// This event get the percent realisation of posture A.
        /// </summary>
        public event GestureAProgressEventHandler GestureAProgress;

        /// <summary>
        /// Raise event GestureAProgress
        /// </summary>
        /// <param name="percent"></param>
        protected void RaiseGestureAProgress(float percent)
        {
            if (GestureAProgress != null)
            {
                GestureAProgress(this, new GestureAProgressEventArgs(percent));
            }
        }
        #endregion

        #region GestureALost
        /// <summary>
        /// Event triggered when an User stop a posture A
        /// </summary>
        public event GestureALostEventHandler GestureALost;

        /// <summary>
        /// Raise event GestureALost
        /// </summary>
        protected void RaiseGestureALost()
        {
            if (GestureALost != null)
            {
                GestureALost(this, new GestureALostEventArgs());
            }
        }
        #endregion

        #endregion

        #region GestureHome

        #region GestureHomeDetected
        /// <summary>
        /// Event triggered when an User perform a posture Home
        /// </summary>
        public event GestureHomeDetectedEventHandler GestureHomeDetected;

        /// <summary>
        /// Raise event GestureHomeDetected
        /// </summary>
        protected void RaiseGestureHomeDetected()
        {
            if (GestureHomeDetected != null)
            {
                GestureHomeDetected(this, new GestureHomeDetectedEventArgs());
            }
        }
        #endregion

        #region GestureHomeProgress
        /// <summary>
        /// Event triggered when an User doing a posture Home
        /// This event get the percent realisation of posture Home.
        /// </summary>
        public event GestureHomeProgressEventHandler GestureHomeProgress;

        /// <summary>
        /// Raise event GestureHomeProgress
        /// </summary>
        /// <param name="percent"></param>
        protected void RaiseGestureHomeProgress(float percent)
        {
            if (GestureHomeProgress != null)
            {
                GestureHomeProgress(this, new GestureHomeProgressEventArgs(percent));
            }
        }
        #endregion

        #region GestureHomeLost
        /// <summary>
        /// Event triggered when an User stop a posture Home
        /// </summary>
        public event GestureHomeLostEventHandler GestureHomeLost;

        /// <summary>
        /// Raise event GestureHomeLost
        /// </summary>
        protected void RaiseGestureHomeLost()
        {
            if (GestureHomeLost != null)
            {
                GestureHomeLost(this, new GestureHomeLostEventArgs());
            }
        }
        #endregion

        #endregion

        #region GestureStay

        #region GestureStayDetected

        /// <summary>
        /// Event triggered when an User perform a posture Stay
        /// </summary>
        public event GestureStayDetectedEventHandler GestureStayDetected;

        /// <summary>
        /// Raise event GestureStayDetected
        /// </summary>
        protected void RaiseGestureStayDetected()
        {
            if (GestureStayDetected != null)
            {
                GestureStayDetected(this, new GestureStayDetectedEventArgs());
            }
        }
        #endregion

        #region GestureStayProgress
        /// <summary>
        /// Event triggered when an User doing a posture Stay
        /// This event get the percent realisation of posture Stay.
        /// </summary>
        public event GestureStayProgressEventHandler GestureStayProgress;

        /// <summary>
        /// Raise event GestureStayProgress
        /// </summary>
        /// <param name="percent"></param>
        protected void RaiseGestureStayProgress(float percent)
        {
            if (GestureStayProgress != null)
            {
                GestureStayProgress(this, new GestureStayProgressEventArgs(percent));
            }
        }

        #endregion

        #region GestureStayLost
        /// <summary>
        /// Event triggered when an User stop a posture Stay
        /// </summary>
        public event GestureStayLostEventHandler GestureStayLost;

        /// <summary>
        /// Raise event GestureStayLost
        /// </summary>
        protected void RaiseGestureStayLost()
        {
            if (GestureStayLost != null)
            {
                GestureStayLost(this, new GestureStayLostEventArgs());
            }
        }

        #endregion

        #endregion

        #region GestureT

        #region GestureTDetected

        /// <summary>
        /// Event triggered when an User perform a posture T
        /// </summary>
        public event GestureTDetectedEventHandler GestureTDetected;

        /// <summary>
        /// Raise event GestureTDetected
        /// </summary>
        protected void RaiseGestureTDetected()
        {
            if (GestureTDetected != null)
            {
                GestureTDetected(this, new GestureTDetectedEventArgs());
            }
        }

        #endregion

        #region GestureTProgress
        /// <summary>
        /// Event triggered when an User doing a posture T
        /// This event get the percent realisation of posture T.
        /// </summary>
        public event GestureTProgressEventHandler GestureTProgress;

        /// <summary>
        /// Raise event GestureTProgress
        /// </summary>
        /// <param name="percent"></param>
        protected void RaiseGestureTProgress(float percent)
        {
            if (GestureTProgress != null)
            {
                GestureTProgress(this, new GestureTProgressEventArgs(percent));
            }
        }
        #endregion

        #region GestureTLost
        /// <summary>
        /// Event triggered when an User stop a posture Stay
        /// </summary>
        public event GestureTLostEventHandler GestureTLost;

        /// <summary>
        /// Raise event GestureTLost
        /// </summary>
        protected void RaiseGestureTLost()
        {
            if (GestureTLost != null)
            {
                GestureTLost(this, new GestureTLostEventArgs());
            }
        }
        #endregion

        #endregion

        #region GestureU

        #region GestureUDetected

        /// <summary>
        /// Event triggered when an User perform posture U
        /// </summary>
        public event GestureUDetectedEventHandler GestureUDetected;

        /// <summary>
        /// Raise event GestureUDetected
        /// </summary>
        protected void RaiseGestureUDetected()
        {
            if (GestureUDetected != null)
            {
                GestureUDetected(this, new GestureUDetectedEventArgs());
            }
        }

        #endregion

        #region GestureUProgress
        /// <summary>
        /// Event triggered when an User doing a posture U
        /// This event get the percent realisation of posture U.
        /// </summary>
        public event GestureUProgressEventHandler GestureUProgress;

        /// <summary>
        /// Raise event GestureUProgress
        /// </summary>
        /// <param name="percent"></param>
        protected void RaiseGestureUProgress(float percent)
        {
            if (GestureUProgress != null)
            {
                GestureUProgress(this, new GestureUProgressEventArgs(percent));
            }
        }

        #endregion

        #region GestureULost
        /// <summary>
        /// Event triggered when an User stop a posture U
        /// </summary>
        public event GestureULostEventHandler GestureULost;

        /// <summary>
        /// Raise event GestureULost
        /// </summary>
        protected void RaiseGestureULost()
        {
            if (GestureULost != null)
            {
                GestureULost(this, new GestureULostEventArgs());
            }
        }
        #endregion

        #endregion

        #region GestureV

        #region GestureVDetected

        /// <summary>
        /// Event triggered when an User perform a posture V
        /// </summary>
        public event GestureVDetectedEventHandler GestureVDetected;

        /// <summary>
        /// Raise event GestureVDetected
        /// </summary>
        protected void RaiseGestureVDetected()
        {
            if (GestureVDetected != null)
            {
                GestureVDetected(this, new GestureVDetectedEventArgs());
            }
        }
        #endregion

        #region GestureVProgress
        /// <summary>
        /// Event triggered when an User doing a posture V
        /// This event get the percent realisation of posture V.
        /// </summary>
        public event GestureVProgressEventHandler GestureVProgress;

        /// <summary>
        /// Raise event GestureVProgress
        /// </summary>
        /// <param name="percent"></param>
        protected void RaiseGestureVProgress(float percent)
        {
            if (GestureVProgress != null)
            {
                GestureVProgress(this, new GestureVProgressEventArgs(percent));
            }
        }
        #endregion

        #region GestureVLost
        /// <summary>
        /// Event triggered when an User stop a posture U
        /// </summary>
        public event GestureVLostEventHandler GestureVLost;

        /// <summary>
        /// Raise event GestureVLost
        /// </summary>
        protected void RaiseGestureVLost()
        {
            if (GestureVLost != null)
            {
                GestureVLost(this, new GestureVLostEventArgs());
            }
        }
        #endregion

        #endregion

        #region GestureWait

        #region GestureWaitDetected

        /// <summary>
        /// Event triggered when an User perform a posture Wait
        /// </summary>
        public event GestureWaitDetectedEventHandler GestureWaitDetected;

        /// <summary>
        /// Raise event GestureWaitDetected
        /// </summary>
        protected void RaiseGestureWaitDetected()
        {
            if (GestureWaitDetected != null)
            {
                GestureWaitDetected(this, new GestureWaitDetectedEventArgs());
            }
        }

        #endregion

        #region GestureWaitProgress
        /// <summary>
        /// Event triggered when an User doing a posture Wait
        /// This event get the percent realisation of posture Wait. 
        /// </summary>
        public event GestureWaitProgressEventHandler GestureWaitProgress;

        /// <summary>
        /// Raise event GestureWaitProgress
        /// </summary>
        /// <param name="percent"></param>
        protected void RaiseGestureWaitProgress(float percent)
        {
            if (GestureWaitProgress != null)
            {
                GestureWaitProgress(this, new GestureWaitProgressEventArgs(percent));
            }
        }
        #endregion

        #region GestureWaitLost
        /// <summary>
        /// Event triggered when an User stop a posture Wait
        /// </summary>
        public event GestureWaitLostEventHandler GestureWaitLost;

        /// <summary>
        /// Raise event GestureWaitLost
        /// </summary>
        protected void RaiseGestureWaitLost()
        {
            if (GestureWaitLost != null)
            {
                GestureWaitLost(this, new GestureWaitLostEventArgs());
            }
        }
        #endregion

        #endregion

        #endregion

        #region Event's Handler

        /// <summary>
        /// Callback when a gesture is detected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUserGestureDetected(object sender, UserGestureDetectedEventArgs e)
        {
            switch (e.Gesture)
            {
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_SWIPE_LEFT:
                    RaiseGestureSwipeLeftDetected();
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_SWIPE_RIGHT:
                    RaiseGestureSwipeRightDetected();
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_WAVE:
                    RaiseGestureWaveDetected();
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_PUSH:
                    RaiseGesturePushDetected();
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_MAXIMIZE:
                    RaiseGestureMaximizeDetected();
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_MINIMIZE:
                    RaiseGestureMinimizeDetected();
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_T:
                    RaiseGestureTDetected();
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_V:
                    RaiseGestureVDetected();
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_A:
                    RaiseGestureADetected();
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_U:
                    RaiseGestureUDetected();
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_WAIT:
                    RaiseGestureWaitDetected();
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_HOME:
                    RaiseGestureHomeDetected();
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_STAY:
                    RaiseGestureStayDetected();
                    break;
            }
        }

        /// <summary>
        /// Callback when a posture is in progress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUsergestureProgress(object sender, UserGestureProgressEventArgs e)
        {
            switch (e.Gesture)
            {
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_T:
                    RaiseGestureTProgress(e.Progress);
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_V:
                    RaiseGestureVProgress(e.Progress);
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_A:
                    RaiseGestureAProgress(e.Progress);
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_U:
                    RaiseGestureUProgress(e.Progress);
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_WAIT:
                    RaiseGestureWaitProgress(e.Progress);
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_HOME:
                    RaiseGestureHomeProgress(e.Progress);
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_STAY:
                    RaiseGestureStayProgress(e.Progress);
                    break;
            }
        }

        /// <summary>
        /// Callback when a posture is lost
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUserGestureLost(object sender, UserGestureLostEventArgs e)
        {
            switch (e.Gesture)
            {
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_T:
                    RaiseGestureTLost();
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_V:
                    RaiseGestureVLost();
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_A:
                    RaiseGestureALost();
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_U:
                    RaiseGestureULost();
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_WAIT:
                    RaiseGestureWaitLost();
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_HOME:
                    RaiseGestureHomeLost();
                    break;
                case EnumKinectGestureRecognize.KINECT_RECOGNIZE_STAY:
                    RaiseGestureStayLost();
                    break;
            }
        }

        #endregion
    }
}
