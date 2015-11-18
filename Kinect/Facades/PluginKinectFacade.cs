using System;
using System.ComponentModel;

namespace IntuiLab.Kinect
{
    public class PluginKinectFacade : INotifyPropertyChanged, IDisposable 
    {
        #region NotifyPropertyChanged
        // Event triggered by modification of a property
        public event PropertyChangedEventHandler PropertyChanged;

        internal void NotifyPropertyChanged(String strInfo)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(strInfo));
            }
        }

        internal void UpdatePropertyChanged(string propertyName)
        {
            NotifyPropertyChanged(propertyName);
        }
        #endregion 

        #region Events

        #region NoUserDetected

        /// <summary>
        /// Event triggered when there is nobody in front of the sensor
        /// </summary>
        public event EventHandler NoUserDetected;

        /// <summary>
        /// Raise event NoUserDetected
        /// </summary>
        /// <param name="nUserCounter"></param>
        internal void RaiseNoUserDetected()
        {
            if (NoUserDetected != null)
            {
                NoUserDetected(this, new EventArgs());
            }
        }
        #endregion

        #region OnePersonDetected
        /// <summary>
        /// Event triggered when a new is tracked by the sensor
        /// </summary>
        public event EventHandler OnePersonDetected;

        /// <summary>
        /// Raise event NoUserDetected
        /// </summary>
        /// <param name="nUserCounter"></param>
        internal void RaiseOnePersonDetected()
        {
            if (OnePersonDetected != null)
            {
                OnePersonDetected(this, new EventArgs());
            }
        }
        #endregion

        #endregion

        #region Properties

        #region UserCount
        public int UserCount
        {
            get
            {
                return PropertiesPluginKinect.Instance.UserCounter;
            }
        }
        #endregion

        #region UserDistance
        public string UserDistance
        {
            get
            {
                return PropertiesPluginKinect.Instance.KinectUserDistance;
            }
        }
        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// The Kinect module is automatically started
        /// </summary>
        public PluginKinectFacade() 
        {
            if (PluginKinect.InstancePluginKinect == null)
            {
                PluginKinect plugin = new PluginKinect();
            }
            PropertiesPluginKinect.Instance.PropertyChanged += new PropertyChangedEventHandler(PluginPropertyChanged);
            Main.RegisterFacade(this);
        }

        #endregion

        #region Public services

        #region Start/Stop Module kinect

        /// <summary>
        /// Run the Kinect Module
        /// </summary>
        public void StartModuleKinect()
        {
            if (!PluginKinect.InstancePluginKinect.Kinect.IsRunning)
            {
                PluginKinect.InstancePluginKinect.StartModuleKinect();
            }
        }

        /// <summary>
        /// Stop the Kinect Module
        /// </summary>
        public void StopModuleKinect()
        {
            if (PluginKinect.InstancePluginKinect.Kinect.IsRunning)
            {
                PluginKinect.InstancePluginKinect.StopModuleKinect();
            }
        }
        #endregion

        #region Active/Desactive Services

        #region All Services

        /// <summary>
        /// Enabled all Kinect module services
        /// </summary>
        public void EnableAllServices()
        {
            PropertiesPluginKinect.Instance.EnableSkeletonFrameService = true;
            PropertiesPluginKinect.Instance.EnableColorFrameService = true;
            PropertiesPluginKinect.Instance.EnableColorFrameService = true;
            PropertiesPluginKinect.Instance.EnableDepthFrameService = true;
        }

        /// <summary>
        /// Disable all Kinect module services
        /// </summary>
        public void DisableAllServices()
        {
            PropertiesPluginKinect.Instance.EnableSkeletonFrameService = false;
            PropertiesPluginKinect.Instance.EnableColorFrameService = false;
            PropertiesPluginKinect.Instance.EnableColorFrameService = false;
            PropertiesPluginKinect.Instance.EnableDepthFrameService = false;
        }


        #endregion

        #endregion

        #region DataRecorder Manager

        public void StartRecord(string sFile)
        {
            PluginKinect.InstancePluginKinect.StartRecord(sFile);
        }

        public void StopRecord()
        {
            PluginKinect.InstancePluginKinect.StopRecord();
        }
        #endregion

        #endregion

        #region IDisposable's members

        /// <summary>
        /// Dispose this instance.
        /// </summary>
        public void Dispose()
        {
            PropertiesPluginKinect.Instance.PropertyChanged -= new PropertyChangedEventHandler(PluginPropertyChanged);
        }
        #endregion

        #region Internal methods

        /// <summary>
        /// Called on changed plugin property
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PluginPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "UserCounter" :
                    {
                        NotifyPropertyChanged("UserCount");
                        break;
                    }
                case "UserDistance":
                    {
                        NotifyPropertyChanged("UserDistance");
                        break;
                    }
                case "OnePersonDetected":
                    {
                        RaiseOnePersonDetected();
                        break;
                    }
                case "NoUserDetected":
                    {
                        RaiseNoUserDetected();
                        break;
                    }
            }
        }
        #endregion
    }
}
