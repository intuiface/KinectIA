            // ****************************************************************************
            // <copyright file="PosturesLettersFacade.cs" company="IntuiLab">
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
    public class PosturesLettersFacade : IDisposable
    {
        #region Properties

        #region PostureAEnabled
        public bool PostureAEnabled
        {
            get
            {
                return PropertiesPluginKinect.Instance.EnablePostureA;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.EnablePostureA != value)
                {
                    PropertiesPluginKinect.Instance.EnablePostureA = value;
                }
            }
        }
        #endregion

        #region TimePostureA
        public int TimePostureA
        {
            get
            {
                return PropertiesPluginKinect.Instance.ALowerBoundForSuccess / 30;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.ALowerBoundForSuccess/30 != value)
                {
                    PropertiesPluginKinect.Instance.ALowerBoundForSuccess = value * 30;
                }
            }
        }
        #endregion
        
        #region PostureTEnabled
        public bool PostureTEnabled
        {
            get
            {
                return PropertiesPluginKinect.Instance.EnablePostureT;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.EnablePostureT != value)
                {
                    PropertiesPluginKinect.Instance.EnablePostureT = value;
                }
            }
        }
        #endregion

        #region TimePostureT
        public int TimePostureT
        {
            get
            {
                return PropertiesPluginKinect.Instance.TLowerBoundForSuccess / 30;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.TLowerBoundForSuccess/30 != value)
                {
                    PropertiesPluginKinect.Instance.TLowerBoundForSuccess = value * 30;
                }
            }
        }
        #endregion

        #region PostureUEnabled
        public bool PostureUEnabled
        {
            get
            {
                return PropertiesPluginKinect.Instance.EnablePostureU;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.EnablePostureU != value)
                {
                    PropertiesPluginKinect.Instance.EnablePostureU = value;
                }
            }
        }
        #endregion

        #region TimePostureU
        public int TimePostureU
        {
            get
            {
                return PropertiesPluginKinect.Instance.ULowerBoundForSuccess/30;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.ULowerBoundForSuccess/30 != value)
                {
                    PropertiesPluginKinect.Instance.ULowerBoundForSuccess = value * 30;
                }
            }
        }
        #endregion

        #region PostureVEnabled
        public bool PostureVEnabled
        {
            get
            {
                return PropertiesPluginKinect.Instance.EnablePostureV;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.EnablePostureV != value)
                {
                    PropertiesPluginKinect.Instance.EnablePostureV = value;
                }
            }
        }
        #endregion

        #region TimePostureV
        public int TimePostureV
        {
            get
            {
                return PropertiesPluginKinect.Instance.VLowerBoundForSuccess/30;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.VLowerBoundForSuccess/30 != value)
                {
                    PropertiesPluginKinect.Instance.VLowerBoundForSuccess = value * 30;
                }
            }
        }
        #endregion

        #endregion

        #region Events

        #region PostureADetected

        public event GestureADetectedEventHandler PostureADetected;

        protected void RaisePostureADetected()
        {
            if (PostureADetected != null)
            {
                PostureADetected(this, new GestureADetectedEventArgs());
            }
        }

        private void OnA(object sender, GestureADetectedEventArgs e)
        {
            RaisePostureADetected();
        }

        #endregion

        #region PostureAProgress

        public event GestureAProgressEventHandler PostureAProgress;

        protected void RaisePostureAProgress(float percent)
        {
            if (PostureAProgress != null)
            {
                PostureAProgress(this, new GestureAProgressEventArgs(percent));
            }
        }

        private void OnAProgress(object sender, GestureAProgressEventArgs e)
        {
            RaisePostureAProgress(e.Percent);
        }

        #endregion

        #region PostureALost

        public event GestureALostEventHandler PostureALost;

        protected void RaisePostureALost()
        {
            if (PostureALost != null)
            {
                PostureALost(this, new GestureALostEventArgs());
            }
        }

        private void OnALost(object sender, GestureALostEventArgs e)
        {
            RaisePostureALost();
        }

        #endregion

        #region PostureTDetected

        public event GestureTDetectedEventHandler PostureTDetected;

        protected void RaisePostureTDetected()
        {
            if (PostureTDetected != null)
            {
                PostureTDetected(this, new GestureTDetectedEventArgs());
            }
        }

        private void OnT(object sender, GestureTDetectedEventArgs e)
        {
            RaisePostureTDetected();
        }

        #endregion

        #region PostureTProgress

        public event GestureTProgressEventHandler PostureTProgress;

        protected void RaisePostureTProgress(float percent)
        {
            if (PostureTProgress != null)
            {
                PostureTProgress(this, new GestureTProgressEventArgs(percent));
            }
        }

        private void OnTProgress(object sender, GestureTProgressEventArgs e)
        {
            RaisePostureTProgress(e.Percent);
        }

        #endregion

        #region PostureTLost

        public event GestureTLostEventHandler PostureTLost;

        protected void RaisePostureTLost()
        {
            if (PostureTLost != null)
            {
                PostureTLost(this, new GestureTLostEventArgs());
            }
        }

        private void OnTLost(object sender, GestureTLostEventArgs e)
        {
            RaisePostureTLost();
        }

        #endregion

        #region PostureUDetected

        public event GestureUDetectedEventHandler PostureUDetected;

        protected void RaisePostureUDetected()
        {
            if (PostureUDetected != null)
            {
                PostureUDetected(this, new GestureUDetectedEventArgs());
            }
        }

        private void OnU(object sender, GestureUDetectedEventArgs e)
        {
            RaisePostureUDetected();
        }

        #endregion

        #region PostureUProgress

        public event GestureUProgressEventHandler PostureUProgress;

        protected void RaisePostureUProgress(float percent)
        {
            if (PostureUProgress != null)
            {
                PostureUProgress(this, new GestureUProgressEventArgs(percent));
            }
        }

        private void OnUProgress(object sender, GestureUProgressEventArgs e)
        {
            RaisePostureUProgress(e.Percent);
        }

        #endregion

        #region PostureULost

        public event GestureULostEventHandler PostureULost;

        protected void RaisePostureULost()
        {
            if (PostureULost != null)
            {
                PostureULost(this, new GestureULostEventArgs());
            }
        }

        private void OnULost(object sender, GestureULostEventArgs e)
        {
            RaisePostureULost();
        }

        #endregion

        #region PostureVDetected

        public event GestureVDetectedEventHandler PostureVDetected;

        protected void RaisePostureVDetected()
        {
            if (PostureVDetected != null)
            {
                PostureVDetected(this, new GestureVDetectedEventArgs());
            }
        }

        private void OnV(object sender, GestureVDetectedEventArgs e)
        {
            RaisePostureVDetected();
        }

        #endregion

        #region PostureVProgress

        public event GestureVProgressEventHandler PostureVProgress;

        protected void RaisePostureVProgress(float percent)
        {
            if (PostureVProgress != null)
            {
                PostureVProgress(this, new GestureVProgressEventArgs(percent));
            }
        }

        private void OnVProgress(object sender, GestureVProgressEventArgs e)
        {
            RaisePostureVProgress(e.Percent);
        }

        #endregion

        #region PostureVLost

        public event GestureVLostEventHandler PostureVLost;

        protected void RaisePostureVLost()
        {
            if (PostureVLost != null)
            {
                PostureVLost(this, new GestureVLostEventArgs());
            }
        }

        private void OnVLost(object sender, GestureVLostEventArgs e)
        {
            RaisePostureVLost();
        }

        #endregion

        #endregion

        #region Constructor
        public PosturesLettersFacade()
        {
            if (PluginKinect.InstancePluginKinect == null)
            {
                PluginKinect plugin = new PluginKinect();
                //PluginKinect.InstancePluginKinect.IsPrimaryInstance = false;
            }

            PostureAEnabled = true;
            PostureTEnabled = true;
            PostureUEnabled = true;
            PostureVEnabled = true;

            TimePostureA = 1;
            TimePostureT = 1;
            TimePostureU = 1;
            TimePostureV = 1;

            ConnectHandler();
            Main.RegisterFacade(this);
        }
        #endregion

        #region Public Methods
        public void ConnectHandler()
        {
            if (PluginKinect.InstancePluginKinect != null)
            {
                PluginKinect.InstancePluginKinect.Kinect.GestureADetected += OnA;
                PluginKinect.InstancePluginKinect.Kinect.GestureAProgress += OnAProgress;
                PluginKinect.InstancePluginKinect.Kinect.GestureALost += OnALost;

                PluginKinect.InstancePluginKinect.Kinect.GestureTDetected += OnT;
                PluginKinect.InstancePluginKinect.Kinect.GestureTProgress += OnTProgress;
                PluginKinect.InstancePluginKinect.Kinect.GestureTLost += OnTLost;

                PluginKinect.InstancePluginKinect.Kinect.GestureUDetected += OnU;
                PluginKinect.InstancePluginKinect.Kinect.GestureUProgress += OnUProgress;
                PluginKinect.InstancePluginKinect.Kinect.GestureULost += OnULost;

                PluginKinect.InstancePluginKinect.Kinect.GestureVDetected += OnV;
                PluginKinect.InstancePluginKinect.Kinect.GestureVProgress += OnVProgress;
                PluginKinect.InstancePluginKinect.Kinect.GestureVLost += OnVLost;
            }
        }

        public void ResetHandler()
        {
            if (PluginKinect.InstancePluginKinect != null)
            {
                PluginKinect.InstancePluginKinect.Kinect.GestureADetected -= OnA;
                PluginKinect.InstancePluginKinect.Kinect.GestureAProgress -= OnAProgress;
                PluginKinect.InstancePluginKinect.Kinect.GestureALost -= OnALost;

                PluginKinect.InstancePluginKinect.Kinect.GestureTDetected -= OnT;
                PluginKinect.InstancePluginKinect.Kinect.GestureTProgress -= OnTProgress;
                PluginKinect.InstancePluginKinect.Kinect.GestureTLost -= OnTLost;

                PluginKinect.InstancePluginKinect.Kinect.GestureUDetected -= OnU;
                PluginKinect.InstancePluginKinect.Kinect.GestureUProgress -= OnUProgress;
                PluginKinect.InstancePluginKinect.Kinect.GestureULost -= OnULost;

                PluginKinect.InstancePluginKinect.Kinect.GestureVDetected -= OnV;
                PluginKinect.InstancePluginKinect.Kinect.GestureVProgress -= OnVProgress;
                PluginKinect.InstancePluginKinect.Kinect.GestureVLost -= OnVLost;
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
