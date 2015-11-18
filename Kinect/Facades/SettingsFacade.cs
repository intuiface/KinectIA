using System;
using System.ComponentModel;

namespace IntuiLab.Kinect
{
    public class SettingsFacade : INotifyPropertyChanged, IDisposable
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

        #region Enum

        public enum KinectStream
        {
            Color,
            Depth,
            None
        };

        #endregion

        #region Properties

        #region UseColorStream
        /// <summary>
        /// Indicate if the Kinect Color Stream is used
        /// </summary>
        public bool UseColorStream
        {
            get
            {
                return PropertiesPluginKinect.Instance.EnableColorFrameService;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.EnableColorFrameService != value)
                {
                    PropertiesPluginKinect.Instance.EnableColorFrameService = value;
                    if (PropertiesPluginKinect.Instance.EnableColorFrameService)
                    {
                        UseDepthStream = false;
                        UseSkeletonStream = false;
                    }
                }
            }
        }
        #endregion

        #region UseDepthStream
        /// <summary>
        /// Indicate if the Kinect Depth Stream is used
        /// </summary>
        public bool UseDepthStream
        {
            get
            {
                return PropertiesPluginKinect.Instance.EnableDepthFrameService;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.EnableDepthFrameService != value)
                {
                    PropertiesPluginKinect.Instance.EnableDepthFrameService = value;
                    if (PropertiesPluginKinect.Instance.EnableDepthFrameService)
                    {
                        UseColorStream = false;
                        UseSkeletonStream = false;
                    }
                }
            }
        }
        #endregion

        #region UseSkeletonStream
        /// <summary>
        /// Indicate if the Kinect Skeleton Stream is used
        /// </summary>
        public bool UseSkeletonStream
        {
            get
            {
                return PropertiesPluginKinect.Instance.EnableSkeletonFrameService;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.EnableSkeletonFrameService != value)
                {
                    PropertiesPluginKinect.Instance.EnableSkeletonFrameService = value;
                    if (PropertiesPluginKinect.Instance.EnableSkeletonFrameService)
                    {
                        UseColorStream = false;
                        UseDepthStream = false;
                    }
                }
            }
        }
        #endregion

        #region ColorStream
        public byte[] ColorStream
        {
            get
            {
                return PropertiesPluginKinect.Instance.ColorFrame;
            }
        }
        #endregion

        #region DepthStream
        public byte[] DepthStream
        {
            get
            {
                return PropertiesPluginKinect.Instance.DepthFrame;
            }
        }
        #endregion

        #region SkeletonStream
        public byte[] SkeletonStream
        {
            get
            {
                return PropertiesPluginKinect.Instance.SkeletonFrameAlone;
            }
        }
        #endregion

        #region Stream
        private KinectStream m_Stream;
        public KinectStream Stream
        {
            get
            {
                return m_Stream;
            }
            set
            {
                if (m_Stream != value)
                {
                    m_Stream = value;

                    if (m_Stream == KinectStream.Color)
                    {
                        UseColorStream = true;
                    }
                    else if (m_Stream == KinectStream.Depth)
                    {
                        UseDepthStream = true;
                    }
                    else if (m_Stream == KinectStream.None)
                    {
                        UseSkeletonStream = true;
                    }
                    NotifyPropertyChanged("Stream");
                }
            }
        }
        #endregion

        #region StreamWithSkeleton

        public bool StreamWithSkeleton
        {
            get
            {
                return PropertiesPluginKinect.Instance.EnableSkeletonOnColorDepth;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.EnableSkeletonOnColorDepth != value)
                {
                    PropertiesPluginKinect.Instance.EnableSkeletonOnColorDepth = value;
                    NotifyPropertyChanged("StreamWithSkeleton");
                }
            }
        }

        #endregion

        #region MinDistanceToLockKinect
        /// <summary>
        /// Minimal distance user/sensor to lock kinect (in meter)
        /// </summary>
        public double MinDistanceToLockKinect
        {
            get
            {
                return PropertiesPluginKinect.Instance.KinectMinDistanceToLock;
            }
            set
            {
                PropertiesPluginKinect.Instance.KinectMinDistanceToLock = value;
            }
        }
        #endregion

        #region ElevationAngleKinect
        /// <summary>
        /// Kinect sensor elevation (in degrees between -27° and 27°)
        /// </summary>
        public int ElevationAngleKinect
        {
            get
            {
                return PropertiesPluginKinect.Instance.KinectElevationAngle;
            }
            set
            {
                PropertiesPluginKinect.Instance.KinectElevationAngle = value;
            }
        }
        #endregion       

        #endregion

        #region Events

        public event EventHandler KinectPointingModeActivated;

        public void RaiseKinectPointingModeActivated()
        {
            if (KinectPointingModeActivated != null)
            {
                KinectPointingModeActivated(this, new EventArgs());
            }
        }

        public event EventHandler KinectGestureModeActivated;

        public void RaiseKinectGestureModeActivated()
        {
            if (KinectGestureModeActivated != null)
            {
                KinectGestureModeActivated(this, new EventArgs());
            }
        }

        #endregion

        #region Constructor
        public SettingsFacade()
        {
            if (PluginKinect.InstancePluginKinect == null)
            {
                PluginKinect plugin = new PluginKinect();
            }

            // Subscribe to plugin property change
            PropertiesPluginKinect.Instance.PropertyChanged += new PropertyChangedEventHandler(PluginPropertyChanged);

            Main.RegisterFacade(this);
        }
        #endregion

        #region Kinect Parameters

        /// <summary>
        /// Set the Elevation Sensor in degrees
        /// Value between -27° and 27°
        /// </summary>
        /// <param name="nElevation">the new elevation sensor</param>
        public void ModifyElevationSensor(string nElevation)
        {
            PropertiesPluginKinect.Instance.KinectElevationAngle = Convert.ToInt32(nElevation);
        }

        /// <summary>
        /// Increment of 1° the elevation sensor
        /// </summary>
        public void IncrementElevationSensor()
        {
            //m_refKinectModule.KinectSensorAngle += 1;
            PropertiesPluginKinect.Instance.KinectElevationAngle += 1;
        }

        /// <summary>
        /// Decrement of 1° the elevation sensor
        /// </summary>
        public void DecrementElevationSensor()
        {
            PropertiesPluginKinect.Instance.KinectElevationAngle -= 1;
        }

        /// <summary>
        /// Modify the minimum distance to lock the kinect module (in meter)
        /// </summary>
        /// <param name="dDistance"></param>
        public void ModifyMinDistanceToLock(string dDistance)
        {
            PropertiesPluginKinect.Instance.KinectMinDistanceToLock = Convert.ToDouble(dDistance);
        }

        /// <summary>
        ///  Increment of 0.1m. the minimum distance to lock the kinect module
        /// </summary>
        public void IncrementMinDistanceToLock()
        {
            PropertiesPluginKinect.Instance.KinectMinDistanceToLock += 0.1;
        }

        /// <summary>
        /// Decrement of 0.1m. the minimum distance to lock th kinect module
        /// </summary>
        public void DecrementMinDistanceToLock()
        {
            PropertiesPluginKinect.Instance.KinectMinDistanceToLock -= 0.1;
        }
        #endregion

        #region Public Methods

        public void ChangeKinectMode()
        {
            PropertiesPluginKinect.Instance.KinectPointingModeEnabled = !PropertiesPluginKinect.Instance.KinectPointingModeEnabled;
        }

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
        /// Called on changed of plugin property
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PluginPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Propagate property change events
            switch (e.PropertyName)
            {
                case "ColorFrame":
                    {
                        NotifyPropertyChanged("ColorStream");
                        break;
                    }
                case "DepthFrame":
                    {
                        NotifyPropertyChanged("DepthStream");
                        break;
                    }
                case "SketeletonFrame":
                    {
                        NotifyPropertyChanged("SkeletonStream");
                        break;
                    }
                case "EnableColorFrameService":
                    {
                        NotifyPropertyChanged("UseColorStream");
                        break;
                    }
                case "EnableDepthFrameService":
                    {
                        NotifyPropertyChanged("UseDepthStream");
                        break;
                    }
                case "EnableSkeletonFrameService":
                    {
                        NotifyPropertyChanged("UseSkeletonStream");
                        break;
                    }
                case "KinectMinDistanceToLock":
                    {
                        NotifyPropertyChanged("MinDistanceToLockKinect");
                        break;
                    }
                case "KinectElevationAngle":
                    {
                        NotifyPropertyChanged("ElevationAngleKinect");
                        break;
                    }
                case "KinectPointingMode":
                    {
                        if (PropertiesPluginKinect.Instance.KinectPointingModeEnabled)
                        {
                            RaiseKinectPointingModeActivated();
                        }
                        else
                        {
                            RaiseKinectGestureModeActivated();
                        }
                        break;
                    }
            }
        }
        #endregion
    }
}
