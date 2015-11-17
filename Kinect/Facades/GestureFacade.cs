            // ****************************************************************************
            // <copyright file="GestureFacade.cs" company="IntuiLab">
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
using IntuiLab.Kinect.Exceptions;

namespace IntuiLab.Kinect
{
    public class GestureFacade : IDisposable
    {
        #region Properties

        #region GestureMaximizeEnabled
        /// <summary>
        /// Indicates if the gesture Maximize is enabled or not
        /// </summary>
        public bool GestureMaximizeEnabled
        {
            get
            {
                return PropertiesPluginKinect.Instance.EnableGestureMaximize;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.EnableGestureMaximize != value)
                {
                    PropertiesPluginKinect.Instance.EnableGestureMaximize = value;
                }
            }
        }
        #endregion

        #region GestureMinimizeEnabed
        /// <summary>
        /// Inidicates if the gestures minimize is enabled or not
        /// </summary>
        public bool GestureMinimizeEnabed
        {
            get
            {
                return PropertiesPluginKinect.Instance.EnableGestureMinimize;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.EnableGestureMinimize != value)
                {
                    PropertiesPluginKinect.Instance.EnableGestureMinimize = value;
                }
            }
        }
        #endregion

        #region GesturePushEnabled
        /// <summary>
        /// Indicates if the gesture Push is enabled or not
        /// </summary>
        public bool GesturePushEnabled
        {
            get
            {
                return PropertiesPluginKinect.Instance.EnableGesturePush;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.EnableGesturePush != value)
                {
                    PropertiesPluginKinect.Instance.EnableGesturePush = value;
                }
            }
        }
        #endregion

        #region GestureSwipeLeftEnabled
        /// <summary>
        /// Indicates if the gesture swipe left is enabled or not
        /// </summary>
        public bool GestureSwipeLeftEnabled
        {
            get
            {
                return PropertiesPluginKinect.Instance.EnableGestureSwipeLeft;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.EnableGestureSwipeLeft != value)
                {
                    PropertiesPluginKinect.Instance.EnableGestureSwipeLeft = value;
                }
            }
        }
        #endregion

        #region GestureSwipeRightEnabled
        /// <summary>
        /// Indicates if the gesture swipe right is enabled or not
        /// </summary>
        public bool GestureSwipeRightEnabled
        {
            get
            {
                return PropertiesPluginKinect.Instance.EnableGestureSwipeRight;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.EnableGestureSwipeRight != value)
                {
                    PropertiesPluginKinect.Instance.EnableGestureSwipeRight = value;
                }
            }
        }
        #endregion

        #region GestureWaveEnabled
        /// <summary>
        /// Indicates if the gesture Wave is enabled or not
        /// </summary>
        public bool GestureWaveEnabled
        {
            get
            {
                return PropertiesPluginKinect.Instance.EnableGestureWave;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.EnableGestureWave != value)
                {
                    PropertiesPluginKinect.Instance.EnableGestureWave = value;
                }
            }
        }
        #endregion

        #region GestureGripEnabled
        /// <summary>
        /// Indicates if the gesture Grip is enabled or not
        /// </summary>
        public bool GestureGripEnabled
        {
            get
            {
                return PropertiesPluginKinect.Instance.EnableGestureGrip;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.EnableGestureGrip != value)
                {
                    PropertiesPluginKinect.Instance.EnableGestureGrip = value;
                }
            }
        }
        #endregion

        #endregion

        #region Events

        #region MaximizeDetected

        /// <summary>
        /// Event trigerred when the gesture Maximize is detected
        /// </summary>
        public event GestureMaximizeDetectedEventHandler MaximizeDetected;

        /// <summary>
        /// Raise event MaximizeDetected
        /// </summary>
        protected void RaiseMaximizeDetected()
        {
            if (MaximizeDetected != null)
            {
                MaximizeDetected(this, new GestureMaximizeDetectedEventArgs());
            }
        }

        /// <summary>
        /// Callback when a gesture Maximize is detected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaximize(object sender, GestureMaximizeDetectedEventArgs e)
        {
            RaiseMaximizeDetected();
        }

        #endregion

        #region MinimizeDetected

        /// <summary>
        /// Event triggered when the gesture minimize is detected
        /// </summary>
        public event GestureMinimizeDetectedEventHandler MinimizeDetected;

        /// <summary>
        /// Raise event MinimizeDetected
        /// </summary>
        protected void RaiseMinimizeDetected()
        {
            if (MinimizeDetected != null)
            {
                MinimizeDetected(this, new GestureMinimizeDetectedEventArgs());
            }
        }

        /// <summary>
        /// Callback when a gesture Minimize is detected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinimize(object sender, GestureMinimizeDetectedEventArgs e)
        {
            RaiseMinimizeDetected();
        }

        #endregion

        #region PushDetected
        public event GesturePushDetectedEventHandler PushDetected;

        protected void RaisePushDetected()
        {
            if (PushDetected != null)
            {
                PushDetected(this, new GesturePushDetectedEventArgs());
            }
        }

        private void OnPush(object sender, GesturePushDetectedEventArgs e)
        {
            RaisePushDetected();
        }

        #endregion

        #region SwipeLeftDetected

        public event GestureSwipeLeftDetectedEventHandler SwipeLeftDetected;

        protected void RaiseSwipeLeftDetected()
        {
            if (SwipeLeftDetected != null)
            {
                SwipeLeftDetected(this, new GestureSwipeLeftDetectedEventArgs());
            }
        }

        private void OnSwipeLeft(object sender, GestureSwipeLeftDetectedEventArgs e)
        {
            RaiseSwipeLeftDetected();
        }

        #endregion

        #region SwipeRightDetected

        public event GestureSwipeRightDetectedEventHandler SwipeRightDetected;

        protected void RaiseSwipeRightDetected()
        {
            if (SwipeRightDetected != null)
            {
                SwipeRightDetected(this, new GestureSwipeRightDetectedEventArgs());
            }
        }

        private void OnSwipeRight(object sender, GestureSwipeRightDetectedEventArgs e)
        {
            RaiseSwipeRightDetected();
        }

        #endregion

        #region WaveDetected
        public event GestureWaveDetectedEventHandler WaveDetected;

        protected void RaiseWaveDetected()
        {
            if (WaveDetected != null)
            {
                WaveDetected(this, new GestureWaveDetectedEventArgs());
            }
        }

        private void OnWave(object sender, GestureWaveDetectedEventArgs e)
        {
            RaiseWaveDetected();
        }

        #endregion

        #region GripDetected
        public event GestureGripDetectedEventHandler GripDetected;
        
        protected void RaiseGripDetected()
        {
            if (GripDetected != null)
            {
                GripDetected(this, new GestureGripDetectedEventArgs());
            }
        }

        private void OnGrip(object sender, DataUserTracking.Events.HandGripStateChangeEventArgs e)
        {
            if (GestureGripEnabled && e.IsGrip)
            {
                RaiseGripDetected();    
            }
        }
        
        #endregion
        

        #endregion

        #region Constructor
        public GestureFacade()
        {
            if (PluginKinect.InstancePluginKinect == null)
            {
                PluginKinect plugin = new PluginKinect();
            }
            
            
            GestureMaximizeEnabled = false;
            GestureMinimizeEnabed = false;
            GesturePushEnabled = true;
            GestureSwipeLeftEnabled = true;
            GestureSwipeRightEnabled = true;
            GestureWaveEnabled = true;
            GestureGripEnabled = true;

            ConnectHandler();
            Main.RegisterFacade(this);
        }
        #endregion

        #region Public Methods
        public void ConnectHandler()
        {
            if (PluginKinect.InstancePluginKinect != null)
            {
                PluginKinect.InstancePluginKinect.Kinect.GestureMaximizeDetected += OnMaximize;
                PluginKinect.InstancePluginKinect.Kinect.GestureMinimizeDetected += OnMinimize;
                PluginKinect.InstancePluginKinect.Kinect.GesturePushDetected += OnPush;
                PluginKinect.InstancePluginKinect.Kinect.GestureSwipeLeftDetected += OnSwipeLeft;
                PluginKinect.InstancePluginKinect.Kinect.GestureSwipeRightDetected += OnSwipeRight;
                PluginKinect.InstancePluginKinect.Kinect.GestureWaveDetected += OnWave;

                PluginKinect.InstancePluginKinect.Kinect.UserHandGripStateChanged += OnGrip;

            }
        }

        public void ResetHandler()
        {
            if (PluginKinect.InstancePluginKinect != null)
            {
                PluginKinect.InstancePluginKinect.Kinect.GestureMaximizeDetected -= OnMaximize;
                PluginKinect.InstancePluginKinect.Kinect.GestureMinimizeDetected -= OnMinimize;
                PluginKinect.InstancePluginKinect.Kinect.GesturePushDetected -= OnPush;
                PluginKinect.InstancePluginKinect.Kinect.GestureSwipeLeftDetected -= OnSwipeLeft;
                PluginKinect.InstancePluginKinect.Kinect.GestureSwipeRightDetected -= OnSwipeRight;
                PluginKinect.InstancePluginKinect.Kinect.GestureWaveDetected -= OnWave;

                PluginKinect.InstancePluginKinect.Kinect.UserHandGripStateChanged -= OnGrip;
            }
        }

        #endregion

        #region IDisposable's members

        /// <summary>
        /// Dispose this instance.
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                ResetHandler();
            }
        }
        #endregion
    }
}
