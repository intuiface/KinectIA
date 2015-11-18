using IntuiLab.Kinect.Events;
using System;

namespace IntuiLab.Kinect
{
    public class PosturesOrdersFacade : IDisposable
    {
        #region Properties

        #region PostureHomeEnabled
        public bool PostureHomeEnabled
        {
            get
            {
                return PropertiesPluginKinect.Instance.EnablePostureHome;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.EnablePostureHome != value)
                {
                    PropertiesPluginKinect.Instance.EnablePostureHome = value;
                }
            }
        }
        #endregion

        #region TimePostureHome
        public int TimePostureHome
        {
            get
            {
                return PropertiesPluginKinect.Instance.HomeLowerBoundForSuccess / 30;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.HomeLowerBoundForSuccess/30 != value)
                {
                    PropertiesPluginKinect.Instance.HomeLowerBoundForSuccess = value * 30;
                }
            }
        }
        #endregion

        #region PostureStayEnabled
        public bool PostureStayEnabled
        {
            get
            {
                return PropertiesPluginKinect.Instance.EnablePostureStay;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.EnablePostureStay != value)
                {
                    PropertiesPluginKinect.Instance.EnablePostureStay = value;
                }
            }
        }
        #endregion

        #region TimePostureStay
        public int TimePostureStay
        {
            get
            {
                return PropertiesPluginKinect.Instance.StayLowerBoundForSuccess / 30;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.StayLowerBoundForSuccess/30 != value)
                {
                    PropertiesPluginKinect.Instance.StayLowerBoundForSuccess = value * 30;
                }
            }
        }
        #endregion

        #region PostureWaitEnabled
        public bool PostureWaitEnabled
        {
            get
            {
                return PropertiesPluginKinect.Instance.EnablePostureWait;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.EnablePostureWait != value)
                {
                    PropertiesPluginKinect.Instance.EnablePostureWait = value;
                }
            }
        }
        #endregion

        #region TimePostureWait
        public int TimePostureWait
        {
            get
            {
                return PropertiesPluginKinect.Instance.WaitLowerBoundForSuccess / 30;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.WaitLowerBoundForSuccess/30 != value)
                {
                    PropertiesPluginKinect.Instance.WaitLowerBoundForSuccess = value * 30;
                }
            }
        }
        #endregion

        #endregion

        #region Events

        #region PostureHomeDetected

        public event GestureHomeDetectedEventHandler PostureHomeDetected;

        protected void RaisePostureHomeDetected()
        {
            if (PostureHomeDetected != null)
            {
                PostureHomeDetected(this, new GestureHomeDetectedEventArgs());
            }
        }

        private void OnHome(object sender, GestureHomeDetectedEventArgs e)
        {
            RaisePostureHomeDetected();
        }

        #endregion

        #region PostureHomeProgress

        public event GestureHomeProgressEventHandler PostureHomeProgress;

        protected void RaisePostureHomeProgress(float percent)
        {
            if (PostureHomeProgress != null)
            {
                PostureHomeProgress(this, new GestureHomeProgressEventArgs(percent));
            }
        }

        private void OnHomeProgress(object sender, GestureHomeProgressEventArgs e)
        {
            RaisePostureHomeProgress(e.Percent);
        }

        #endregion

        #region PostureHomeLost

        public event GestureHomeLostEventHandler PostureHomeLost;

        protected void RaisePostureHomeLost()
        {
            if (PostureHomeLost != null)
            {
                PostureHomeLost(this, new GestureHomeLostEventArgs());
            }
        }

        private void OnHomeLost(object sender, GestureHomeLostEventArgs e)
        {
            RaisePostureHomeLost();
        }

        #endregion

        #region PostureStayDetected

        public event GestureStayDetectedEventHandler PostureStayDetected;

        protected void RaisePostureStayDetected()
        {
            if (PostureStayDetected != null)
            {
                PostureStayDetected(this, new GestureStayDetectedEventArgs());
            }
        }

        private void OnStay(object sender, GestureStayDetectedEventArgs e)
        {
            RaisePostureStayDetected();
        }

        #endregion

        #region PostureStayProgress

        public event GestureStayProgressEventHandler PostureStayProgress;

        protected void RaisePostureStayProgress(float percent)
        {
            if (PostureStayProgress != null)
            {
                PostureStayProgress(this, new GestureStayProgressEventArgs(percent));
            }
        }

        private void OnStayProgress(object sender, GestureStayProgressEventArgs e)
        {
            RaisePostureStayProgress(e.Percent);
        }

        #endregion

        #region PostureStayLost

        public event GestureStayLostEventHandler PostureStayLost;

        protected void RaisePostureStayLost()
        {
            if (PostureStayLost != null)
            {
                PostureStayLost(this, new GestureStayLostEventArgs());
            }
        }

        private void OnStayLost(object sender, GestureStayLostEventArgs e)
        {
            RaisePostureStayLost();
        }

        #endregion

        #region PostureWaitDetected

        public event GestureWaitDetectedEventHandler PostureWaitDetected;

        protected void RaisePostureWaitDetected()
        {
            if (PostureWaitDetected != null)
            {
                PostureWaitDetected(this, new GestureWaitDetectedEventArgs());
            }
        }

        private void OnWait(object sender, GestureWaitDetectedEventArgs e)
        {
            RaisePostureWaitDetected();
        }

        #endregion

        #region PostureWaitProgress

        public event GestureWaitProgressEventHandler PostureWaitProgress;

        protected void RaisePostureWaitProgress(float percent)
        {
            if (PostureWaitProgress != null)
            {
                PostureWaitProgress(this, new GestureWaitProgressEventArgs(percent));
            }
        }

        private void OnWaitProgress(object sender, GestureWaitProgressEventArgs e)
        {
            RaisePostureWaitProgress(e.Percent);
        }

        #endregion

        #region PostureWaitLost

        public event GestureWaitLostEventHandler PostureWaitLost;

        protected void RaisePostureWaitLost()
        {
            if (PostureWaitLost != null)
            {
                PostureWaitLost(this, new GestureWaitLostEventArgs());
            }
        }

        private void OnWaitLost(object sender, GestureWaitLostEventArgs e)
        {
            RaisePostureWaitLost();
        }

        #endregion

        #endregion

        #region Constructor
        public PosturesOrdersFacade()
        {
            if (PluginKinect.InstancePluginKinect == null)
            {
                PluginKinect plugin = new PluginKinect();
            }

            PostureHomeEnabled = true;
            PostureStayEnabled = true;
            PostureWaitEnabled = true;

            TimePostureHome = 1;
            TimePostureStay = 1;
            TimePostureWait = 1;

            ConnectHandler();
            Main.RegisterFacade(this);
        }
        #endregion

        #region Public Methods
        public void ConnectHandler()
        {
            if (PluginKinect.InstancePluginKinect != null)
            {
                PluginKinect.InstancePluginKinect.Kinect.GestureHomeDetected += OnHome;
                PluginKinect.InstancePluginKinect.Kinect.GestureHomeProgress += OnHomeProgress;
                PluginKinect.InstancePluginKinect.Kinect.GestureHomeLost += OnHomeLost;

                PluginKinect.InstancePluginKinect.Kinect.GestureStayDetected += OnStay;
                PluginKinect.InstancePluginKinect.Kinect.GestureStayProgress += OnStayProgress;
                PluginKinect.InstancePluginKinect.Kinect.GestureStayLost += OnStayLost;

                PluginKinect.InstancePluginKinect.Kinect.GestureWaitDetected += OnWait;
                PluginKinect.InstancePluginKinect.Kinect.GestureWaitProgress += OnWaitProgress;
                PluginKinect.InstancePluginKinect.Kinect.GestureWaitLost += OnWaitLost;
            }
        }

        public void ResetHandler()
        {
            if (PluginKinect.InstancePluginKinect != null)
            {
                PluginKinect.InstancePluginKinect.Kinect.GestureHomeDetected -= OnHome;
                PluginKinect.InstancePluginKinect.Kinect.GestureHomeProgress -= OnHomeProgress;
                PluginKinect.InstancePluginKinect.Kinect.GestureHomeLost -= OnHomeLost;

                PluginKinect.InstancePluginKinect.Kinect.GestureStayDetected -= OnStay;
                PluginKinect.InstancePluginKinect.Kinect.GestureStayProgress -= OnStayProgress;
                PluginKinect.InstancePluginKinect.Kinect.GestureStayLost -= OnStayLost;

                PluginKinect.InstancePluginKinect.Kinect.GestureWaitDetected -= OnWait;
                PluginKinect.InstancePluginKinect.Kinect.GestureWaitProgress -= OnWaitProgress;
                PluginKinect.InstancePluginKinect.Kinect.GestureWaitLost -= OnWaitLost;
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
