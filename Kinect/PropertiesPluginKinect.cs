using IntuiLab.Kinect.Events;
using System;
using System.ComponentModel;

namespace IntuiLab.Kinect
{
    public class PropertiesPluginKinect : IDisposable, INotifyPropertyChanged
    {
        #region Pattern Singleton
        
        private static volatile PropertiesPluginKinect m_refInstance = null;

        private static object syncRoot = new Object();

        public static PropertiesPluginKinect Instance
        {
            get
            {
                if (m_refInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (m_refInstance == null)
                        {
                            m_refInstance = new PropertiesPluginKinect();
                        }
                    }
                }
                return m_refInstance;
            }
        }
        
        public PropertiesPluginKinect() 
        {
            if (m_refInstance == null)
            {
                m_refInstance = this;
                InitializeProperties();
            }
            else
            {
               Console.WriteLine("PropertiesPluginKinect already instantiate");
            }
        }

        #endregion

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

        #endregion 

        #region Initialize

        public void InitializeProperties()
        {
            // Kinect Properties Initialisation
            KinectResolutionHeight = 480; // OK
            KinectResolutionWidth = 640; // OK
            KinectMaxDistanceToLock = 4; // OK
            KinectMinDistanceToLock = 0.5; // OK
            KinectElevationAngle = 10; // OK
            KinectSkeletonSmoothing = 0.5f; // 0.9
            KinectSkeletonCorrection = 0.1f; // 0.9
            KinectSkeletonPrediction = 0.5f; // 1.0
            KinectSkeletonJitter = 0.1f; // 1.0
            KinectSkeletonMaxDeviation = 0.1f; // 1.0

            KinectPointingModeEnabled = false;

            m_KinectModeDisplayOnstream = "Gestures";

            // Kinect Services Properties Initialisation
            EnableColorFrameService = true; // OK
            EnableDepthFrameService = false; // OK
            EnableSkeletonFrameService = false; // OK

            // UserData Properties Initialisation
            MaxSkeletonInStorage = 5; // OK
            GestureRecognizedTime = 1000; // OK
            TimerDelayGesture = 30; // OK
            PostureRecognizedTime = 3000; // OK
            NumberFrameForEndGesture = 5; // OK

            // Gesture Activation Properties Initialisation
            EnableGestureSwipeLeft = false;
            EnableGestureSwipeRight = false;
            EnableGestureWave = false; // OK
            EnableGesturePush = false; // OK
            EnableGestureMaximize = false; // OK
            EnableGestureMinimize = false;
            EnablePostureA = false; // OK
            EnablePostureHome = false; // OK
            EnablePostureStay = false; // OK
            EnablePostureT = false; // OK
            EnablePostureU = false; // OK
            EnablePostureV = false; // OK
            EnablePostureWait = false; // OK

            // Gesture Swipe Properties Initialisation
            SwipeLowerBoundForSuccess = 2; // OK
            SwipeLowerBoundForVelocity = 0.8; // OK
            SwipeCheckerTolerance = 0.04d; // OK

            // Gesture Wave Properties Initialisation
            WaveLowerBoundForSuccess = 1; // OK
            WaveLowerBoundForVelocity  = 0.8; // OK
            WaveCheckerTolerance = 0.02d; // OK

            // Gesture Push Properties Initialisation
            PushLowerBoundForSuccess = 4; // OK
            PushLowerBoundForVelocity = 0.5; // OK
            PushCheckerTolerance = 0.01d; // OK

            // Gesture Maximize Properties Initialisation
            MaximizeLowerBoundForSuccess = 2; // OK
            MaximizeLowerBoundForVelocity = 0.5; // OK
            MaximizeCheckerTolerance = 0.04d; // OK

            // Gesture Minimize Properties Initialisation
            MinimizeLowerBoundForSuccess = 2;
            MinimizeLowerBoundForVelocity = 0.5;
            MinimizeCherckerTolerance = 0.04d;

            // Global Posture Properties Initialisation
            PostureLowerBoundForVelocity = 0.2; // OK
            PostureNumberFrameInitialisation = 10; // OK
            PostureCheckerTolerance = 0.04d; // OK
            PostureAngleTresholdShoulderElBowHand = 30;

            // Posture A Properties Initialisation
            ALowerBoundForSuccess = 30; // OK
            AAngleThreshold = 15; // OK
            AAngleShoulderHands = 90; // OK
            AAngleShoulderElbowHand = 160; // OK

            // Posture Home Properties Initialisation
            HomeLowerBoundForSuccess = 30; // OK
            HomeAngleThreshold = 15; // OK
            HomeAngleShoulderCenterHandLarge = 45; // OK
            HomeAngleShoulderCenterHandSmall = 30; // OK
            HomeAngleShoulderElbowHand = 160; // OK

            // Posture Stay Properties Initialisation
            StayLowerBoundForSuccess = 30; // OK
            StayAngleThreshold = 20; // OK
            StayAngleShoulderElbowHand = 90; // OK

            // Posture T Properties Initialisation
            TLowerBoundForSuccess = 30; // OK
            TAngleThreshold = 20; // OK
            TAngleShoulderHands = 160; // OK
            TAngleShoulderElbowHand = 160; // OK

            // Posture U Properties Initialisation
            ULowerBoundForSuccess = 30; // OK
            UAngleThreshold = 30; // OK
            UAngleShoulderElbowHand = 90; // OK

            // Posture V Properties Initialisation
            VLowerBoundForSuccess = 30; // OK
            VAngleThreshold = 20; // OK
            VAngleShoulderHands = 110; // OK
            VAngleShoulderElbowHand = 160; // OK

            // Posture Wait Properties Initialisation
            WaitLowerBoundForSuccess = 30; // OK
            WaitAngleThreshold = 20; // OK
            WaitAngleShoulderElbowHand = 100; // OK

            SavePosturerecognize = Enums.EnumPosture.POSTURE_NONE;

            // Pointing Properties Initialisation
            PointingSpaceBetweenHands = 0.15f;
            PointingHandsAmplitude = 1f;

            // Default resolution experience IntuiFace 1920*1080
            ExperienceIntuiFaceWidth = 1920;
            ExperienceIntuifaceHeight = 1080;
        }
        #endregion

        #region Constants

        private const double KINECT_MIN_DISTANCE_TO_LOCK = 0.5;

        #endregion

        #region Event Synchronize

        #region SynchronizeElevationAngle

        public event EventHandler<SynchronizeElevationAngleEventArgs> SynchronizeElevationAngle;

        private void RaiseSynchronizeElevationAngle(object sender, SynchronizeElevationAngleEventArgs e)
        {
            if (SynchronizeElevationAngle != null)
            {
                SynchronizeElevationAngle(sender, e);
            }
        }
        #endregion

        #region SynchronizeColorStreamActivation

        public event EventHandler<SynchronizeColorStreamActivationEventArgs> SynchronizeColorStreamActivation;

        private void RaiseSynchronizeColorStreamActivation(object sender, SynchronizeColorStreamActivationEventArgs e)
        {
            if (SynchronizeColorStreamActivation != null)
            {
                SynchronizeColorStreamActivation(sender, e);
            }
        }
        #endregion

        #region SynchronizeDepthStreamActivation

        public event EventHandler<SynchronizeDepthStreamActivationEventArgs> SynchronizeDepthStreamActivation;

        private void RaiseSynchronizeDepthStreamActivation(object sender, SynchronizeDepthStreamActivationEventArgs e)
        {
            if (SynchronizeDepthStreamActivation != null)
            {
                SynchronizeDepthStreamActivation(sender, e);
            }
        }
        #endregion

        #region SynchronizeKinectMode

        public event EventHandler<SynchronizeKinectModeEventArgs> SynchronizeKinectMode;

        private void RaiseSynchronizeKinectMode(object sender, SynchronizeKinectModeEventArgs e)
        {
            if (SynchronizeDepthStreamActivation != null)
            {
                SynchronizeKinectMode(sender, e);
            }
        }
        #endregion

        #endregion

        #region Kinect Properties

        #region ColorFrame
        private byte[] m_ColorFrame;
        public byte[] ColorFrame
        {
            get
            {
                return m_ColorFrame;
            }
            set
            {
                if (m_ColorFrame != value)
                {
                    m_ColorFrame = value;
                    NotifyPropertyChanged("ColorFrame");
                }
            }
        }
        #endregion

        #region DepthFrame
        private byte[] m_DepthFrame;
        public byte[] DepthFrame
        {
            get
            {
                return m_DepthFrame;
            }
            set
            {
                if (m_DepthFrame != value)
                {
                    m_DepthFrame = value;
                    NotifyPropertyChanged("DepthFrame");
                }
            }
        }
        #endregion

        #region SkeletonFrameAlone
        private byte[] m_SkeletonFrameAlone;
        public byte[] SkeletonFrameAlone
        {
            get
            {
                return m_SkeletonFrameAlone;
            }
            set
            {
                if (m_SkeletonFrameAlone != value)
                {
                    m_SkeletonFrameAlone = value;
                    NotifyPropertyChanged("SkeletonFrame");
                }
            }
        }
        #endregion

        #region KinectMinDistanceToLock
        private double m_KinectMinDistanceToLock;
        public double KinectMinDistanceToLock 
        {
            get
            {
                return m_KinectMinDistanceToLock;
            }
            set
            {
                if (m_KinectMinDistanceToLock != value)
                {
                    if (value >= KINECT_MIN_DISTANCE_TO_LOCK && value <= PropertiesPluginKinect.Instance.KinectMaxDistanceToLock)
                    {
                        m_KinectMinDistanceToLock = value;
                        NotifyPropertyChanged("KinectMinDistanceToLock");
                    }
                }
            }
        }
        #endregion

        #region KinectElevationAngle
        private int m_KinectElevationAngle;
        public int KinectElevationAngle
        {
            get
            {
                return m_KinectElevationAngle;
            }
            set
            {
                if (m_KinectElevationAngle != value)
                {
                    m_KinectElevationAngle = value;
                    if (PluginKinect.InstancePluginKinect.Kinect != null)
                    {
                        if (value >= -27 && value <= 27)
                        {
                            m_KinectElevationAngle = value;
                            RaiseSynchronizeElevationAngle(this, new SynchronizeElevationAngleEventArgs
                            {
                                Elevation = m_KinectElevationAngle
                            });
                        }
                    }
                    NotifyPropertyChanged("KinectElevationAngle");
                }
            }
        }
        #endregion

        #region UserCounter
        private int m_UserCounter;
        public int UserCounter
        {
            get
            {
                return m_UserCounter;
            }
            set
            {
                if (m_UserCounter != value)
                {
                    if (m_UserCounter == 0 && value == 1)
                    {
                        NotifyPropertyChanged("OnePersonDetected");
                    }
                    else if (m_UserCounter == 1 && value == 0)
                    {
                        NotifyPropertyChanged("NoUserDetected");
                    }

                    m_UserCounter = value;
                    NotifyPropertyChanged("UserCounter");
                }
            }
        }
        #endregion

        #region KinectUserDistance

        /// <summary>
        /// Distance between User and the Kinect sensor
        /// </summary>
        private double m_dUserDistance;

        public string KinectUserDistance
        {
            get
            {
                if (m_dUserDistance < PropertiesPluginKinect.Instance.KinectMinDistanceToLock)
                {
                    return "MIN";
                }
                else if (m_dUserDistance > PropertiesPluginKinect.Instance.KinectMaxDistanceToLock)
                {
                    return "MAX";
                }
                else
                {
                    return m_dUserDistance.ToString(".##");
                }
            }
            set
            {
                double valueConvert;

                // Test if value (string) can be convert to double
                if (Double.TryParse(value, out valueConvert))
                {
                    if (m_dUserDistance != valueConvert)
                    {
                        m_dUserDistance = valueConvert;
                        NotifyPropertyChanged("UserDistance");

                        // if Value betwween minimum and maximum distance to lock, th system is unlocked
                        if (m_dUserDistance >= PropertiesPluginKinect.Instance.KinectMinDistanceToLock && m_dUserDistance <= PropertiesPluginKinect.Instance.KinectMaxDistanceToLock)
                        {
                            if (PluginKinect.InstancePluginKinect != null)
                            {
                                PluginKinect.InstancePluginKinect.Kinect.Unlocked();
                            }
                        }
                        // else locked system
                        else
                        {
                            if (PluginKinect.InstancePluginKinect != null)
                            {
                                PluginKinect.InstancePluginKinect.Kinect.Locked();
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region KinectPointingModeEnabled

        private bool m_KinectPointingModeEnabled;
        public bool KinectPointingModeEnabled 
        {
            get
            {
                return m_KinectPointingModeEnabled;
            }
            set
            {
                if (m_KinectPointingModeEnabled != value)
                {
                    m_KinectPointingModeEnabled = value;

                    if (m_KinectPointingModeEnabled)
                    {
                        m_KinectModeDisplayOnstream = "Pointing";
                    }
                    else
                    {
                        m_KinectModeDisplayOnstream = "Gestures";
                    }
                    NotifyPropertyChanged("KinectPointingModeEnabled");
                    

                    RaiseSynchronizeKinectMode(this, new SynchronizeKinectModeEventArgs
                    {
                        PointingMode = m_KinectPointingModeEnabled
                    });
                }
            }
        }
        #endregion

        public double KinectMaxDistanceToLock { get; set; }
        public int KinectResolutionWidth { get; set; }
        public int KinectResolutionHeight { get; set; }
        public float KinectSkeletonSmoothing { get; set; }
        public float KinectSkeletonCorrection { get; set; }
        public float KinectSkeletonPrediction { get; set; }
        public float KinectSkeletonJitter { get; set; }
        public float KinectSkeletonMaxDeviation { get; set; }

        private string m_KinectModeDisplayOnstream;
        public string KinectModeDisplayOnstream
        {
            get
            {
                return m_KinectModeDisplayOnstream;
            }
        }

        #endregion

        #region Kinect Services Properties

        #region EnableColorFrameService
        private bool m_EnableColorFrameService;
        public bool EnableColorFrameService 
        {
            get
            {
                return m_EnableColorFrameService;
            }
            set
            {
                if (m_EnableColorFrameService != value)
                {
                    m_EnableColorFrameService = value;
                    if (PluginKinect.InstancePluginKinect.Kinect != null)
                    {
                        RaiseSynchronizeColorStreamActivation(this, new SynchronizeColorStreamActivationEventArgs
                        {
                            Enable = m_EnableColorFrameService
                        });
                    }
                    NotifyPropertyChanged("EnableColorFrameService");
                    
                }
            }
        }
        #endregion

        #region EnableDepthFrameService
        private bool m_EnableDepthFrameService;
        public bool EnableDepthFrameService
        {
            get
            {
                return m_EnableDepthFrameService;
            }
            set
            {
                if (m_EnableDepthFrameService != value)
                {
                    m_EnableDepthFrameService = value;
                    
                    if (PluginKinect.InstancePluginKinect.Kinect != null)
                    {
                        RaiseSynchronizeDepthStreamActivation(this, new SynchronizeDepthStreamActivationEventArgs
                        {
                            Enable = m_EnableDepthFrameService
                        });
                    }
                    NotifyPropertyChanged("EnableDepthFrameService");
                    
                }
            }
        }
        #endregion

        #region EnableSkeletonFrameService
        private bool m_EnableSkeletonFrameService;
        public bool EnableSkeletonFrameService 
        {
            get
            {
                return m_EnableSkeletonFrameService;
            }
            set
            {
                if (m_EnableSkeletonFrameService != value)
                {
                    m_EnableSkeletonFrameService = value;
                    NotifyPropertyChanged("EnableSkeletonFrameService");
                }
            }
        }
        #endregion

        #region EnableSkeletonOnColorDepth
        private bool m_EnableSkeletonOnColorDepth;
        public bool EnableSkeletonOnColorDepth
        {
            get
            {
                return m_EnableSkeletonOnColorDepth;
            }
            set
            {
                if (m_EnableSkeletonOnColorDepth != value)
                {
                    m_EnableSkeletonOnColorDepth = value;
                }
            }
        }

        #endregion

        #endregion

        #region UserData Properties
        public int MaxSkeletonInStorage { get; set; }
        public int GestureRecognizedTime { get; set; }
        public int TimerDelayGesture { get; set; }
        public int PostureRecognizedTime { get; set; }
        public int NumberFrameForEndGesture { get; set; }
        #endregion

        #region Gesture Properties

        #region Events Gestures Activation Sybchronize

        public event EventHandler<SynchronizeGesturesStateEventArgs> SynchronizeGesturesState;

        private void RaiseSynchronizeGesturesActivation(object sender, SynchronizeGesturesStateEventArgs e)
        {
            if (SynchronizeGesturesState != null)
            {
                SynchronizeGesturesState(sender, e);
            }
        }

        #endregion

        #region Gesture Activation Properties
        private bool m_EnableGestureSwipeLeft;
        public bool EnableGestureSwipeLeft 
        { 
            get
            {
                return m_EnableGestureSwipeLeft;
            }
            set
            {
                if (!KinectPointingModeEnabled)
                {
                    if (m_EnableGestureSwipeLeft != value)
                    {
                        m_EnableGestureSwipeLeft = value;
                        RaiseSynchronizeGesturesActivation(this, new SynchronizeGesturesStateEventArgs
                        {
                            GestureName = "SwipeLeft"
                        });
                    }
                }
            }
        }

        private bool m_EnableGestureSwipeRight;
        public bool EnableGestureSwipeRight 
        {
            get
            {
                return m_EnableGestureSwipeRight;
            }
            set
            {
                if (!KinectPointingModeEnabled)
                {
                    if (m_EnableGestureSwipeRight != value)
                    {
                        m_EnableGestureSwipeRight = value;
                        RaiseSynchronizeGesturesActivation(this, new SynchronizeGesturesStateEventArgs
                        {
                            GestureName = "SwipeRight"
                        });
                    }
                }
            }        
        }

        private bool m_EnableGestureWave;
        public bool EnableGestureWave 
        {
            get
            {
                return m_EnableGestureWave;
            }
            set
            {
                if (!KinectPointingModeEnabled)
                {
                    if (m_EnableGestureWave != value)
                    {
                        m_EnableGestureWave = value;
                        RaiseSynchronizeGesturesActivation(this, new SynchronizeGesturesStateEventArgs
                        {
                            GestureName = "Wave"
                        });
                    }
                }
            }        
        }

        private bool m_EnableGestureGrip;
        public bool EnableGestureGrip
        {
            get
            {
                return m_EnableGestureGrip;
            }
            set
            {
                if (!KinectPointingModeEnabled)
                {
                    if (m_EnableGestureGrip != value)
                    {
                        m_EnableGestureGrip = value;
                        RaiseSynchronizeGesturesActivation(this, new SynchronizeGesturesStateEventArgs
                        {
                            GestureName = "Grip"
                        });

                        // Also activate depth stream & interaction stream
                        RaiseSynchronizeDepthStreamActivation(this, new SynchronizeDepthStreamActivationEventArgs
                        {
                            Enable = m_EnableGestureGrip
                        });
                        RaiseSynchronizeKinectMode(this, new SynchronizeKinectModeEventArgs
                        {
                            PointingMode = m_KinectPointingModeEnabled
                        });
                    }
                }
            }
        }

        private bool m_EnableGesturePush;
        public bool EnableGesturePush 
        {
            get
            {
                return m_EnableGesturePush;
            }
            set
            {
                if (!KinectPointingModeEnabled)
                {
                    if (m_EnableGesturePush != value)
                    {
                        m_EnableGesturePush = value;
                        RaiseSynchronizeGesturesActivation(this, new SynchronizeGesturesStateEventArgs
                        {
                            GestureName = "Push"
                        });
                    }
                }
            }        
        }

        private bool m_EnableGestureMaximize;
        public bool EnableGestureMaximize 
        {
            get
            {
                return m_EnableGestureMaximize;
            }
            set
            {
                if (!KinectPointingModeEnabled)
                {
                    if (m_EnableGestureMaximize != value)
                    {
                        m_EnableGestureMaximize = value;
                        RaiseSynchronizeGesturesActivation(this, new SynchronizeGesturesStateEventArgs
                        {
                            GestureName = "Maximize"
                        });
                    }
                }
            }     
        }

        private bool m_EnableGestureMinimize;
        public bool EnableGestureMinimize 
        {
            get
            {
                return m_EnableGestureMinimize;
            }
            set
            {
                if (!KinectPointingModeEnabled)
                {
                    if (m_EnableGestureMinimize != value)
                    {
                        m_EnableGestureMinimize = value;
                        RaiseSynchronizeGesturesActivation(this, new SynchronizeGesturesStateEventArgs
                        {
                            GestureName = "Minimize"
                        });
                    }
                }
            }   
        }

        private bool m_EnablePostureA;
        public bool EnablePostureA 
        {
            get
            {
                return m_EnablePostureA;
            }
            set
            {
                if (m_EnablePostureA != value)
                {
                    m_EnablePostureA = value;
                    RaiseSynchronizeGesturesActivation(this, new SynchronizeGesturesStateEventArgs
                    {
                        GestureName = "A"
                    });
                }
            }   
        }

        private bool m_EnablePostureHome;
        public bool EnablePostureHome 
        {
            get
            {
                return m_EnablePostureHome;
            }
            set
            {
                if (m_EnablePostureHome != value)
                {
                    m_EnablePostureHome = value;
                    RaiseSynchronizeGesturesActivation(this, new SynchronizeGesturesStateEventArgs
                    {
                        GestureName = "Home"
                    });
                }
            }   
        }

        private bool m_EnablePostureStay;
        public bool EnablePostureStay 
        {
            get
            {
                return m_EnablePostureStay;
            }
            set
            {
                if (m_EnablePostureStay != value)
                {
                    m_EnablePostureStay = value;
                    RaiseSynchronizeGesturesActivation(this, new SynchronizeGesturesStateEventArgs
                    {
                        GestureName = "Stay"
                    });
                }
            }  
        }

        private bool m_EnablePostureT;
        public bool EnablePostureT 
        {
            get
            {
                return m_EnablePostureT;
            }
            set
            {
                if (m_EnablePostureT != value)
                {
                    m_EnablePostureT = value;
                    RaiseSynchronizeGesturesActivation(this, new SynchronizeGesturesStateEventArgs
                    {
                        GestureName = "T"
                    });
                }
            }  
        }

        private bool m_EnablePostureU;
        public bool EnablePostureU 
        {
            get
            {
                return m_EnablePostureU;
            }
            set
            {
                if (m_EnablePostureU != value)
                {
                    m_EnablePostureU = value;
                    RaiseSynchronizeGesturesActivation(this, new SynchronizeGesturesStateEventArgs
                    {
                        GestureName = "U"
                    });
                }
            }
        }

        private bool m_EnablePostureV;
        public bool EnablePostureV 
        {
            get
            {
                return m_EnablePostureV;
            }
            set
            {
                if (m_EnablePostureV != value)
                {
                    m_EnablePostureV = value;
                    RaiseSynchronizeGesturesActivation(this, new SynchronizeGesturesStateEventArgs
                    {
                        GestureName = "V"
                    });
                }
            }
        }

        private bool m_EnablePostureWait;
        public bool EnablePostureWait 
        {
            get
            {
                return m_EnablePostureWait;
            }
            set
            {
                if (m_EnablePostureWait != value)
                {
                    m_EnablePostureWait = value;
                    RaiseSynchronizeGesturesActivation(this, new SynchronizeGesturesStateEventArgs
                    {
                        GestureName = "Wait"
                    });
                }
            }
        }
        #endregion

        #region Gesture Swipe Properties
        public int SwipeLowerBoundForSuccess { get; set; }
        public double SwipeLowerBoundForVelocity { get; set; }
        public double SwipeCheckerTolerance { get; set; }
        #endregion

        #region Gesture Wave Properties
        public int WaveLowerBoundForSuccess { get; set; }
        public double WaveLowerBoundForVelocity { get; set; }
        public double WaveCheckerTolerance { get; set; }
        #endregion

        #region Gesture Push Properties
        public int PushLowerBoundForSuccess { get; set; }
        public double PushLowerBoundForVelocity { get; set; }
        public double PushCheckerTolerance { get; set; }
        #endregion

        #region Gesture Maximize
        public int MaximizeLowerBoundForSuccess { get; set; }
        public double MaximizeLowerBoundForVelocity { get; set; }
        public double MaximizeCheckerTolerance { get; set; }
        #endregion

        #region Minimize
        public int MinimizeLowerBoundForSuccess { get; set; }
        public double MinimizeLowerBoundForVelocity { get; set; }
        public double MinimizeCherckerTolerance { get; set; }
        #endregion

        #region Global Posture Properties
        public double PostureLowerBoundForVelocity { get; set; }
        public int PostureNumberFrameInitialisation { get; set; }
        public double PostureCheckerTolerance { get; set; }
        public double PostureAngleTresholdShoulderElBowHand { get; set; }
        #endregion

        #region Posture A Properties
        public int ALowerBoundForSuccess { get; set; }
        public int AAngleThreshold { get; set; }
        public int AAngleShoulderHands { get; set; }
        public int AAngleShoulderElbowHand { get; set; }
        #endregion

        #region Posture Home Properties
        public int HomeLowerBoundForSuccess { get; set; }
        public int HomeAngleThreshold { get; set; }
        public int HomeAngleShoulderCenterHandLarge { get; set; }
        public int HomeAngleShoulderCenterHandSmall { get; set; }
        public int HomeAngleShoulderElbowHand { get; set; }
        #endregion

        #region Posture Stay Properties
        public int StayLowerBoundForSuccess { get; set; }
        public int StayAngleThreshold { get; set; }
        public int StayAngleShoulderElbowHand { get; set; }
        #endregion

        #region Posture T Properties
        public int TLowerBoundForSuccess { get; set; }
        public int TAngleThreshold { get; set; }
        public int TAngleShoulderHands { get; set; }
        public int TAngleShoulderElbowHand { get; set; }
        #endregion

        #region Posture U Properties
        public int ULowerBoundForSuccess { get; set; }
        public int UAngleThreshold { get; set; }
        public int UAngleShoulderElbowHand { get; set; }
        #endregion

        #region Posture V Properties
        public int VLowerBoundForSuccess { get; set; }
        public int VAngleThreshold { get; set; }
        public int VAngleShoulderHands { get; set; }
        public int VAngleShoulderElbowHand { get; set; }
        #endregion

        #region Posture Wait Properties
        public int WaitLowerBoundForSuccess { get; set; }
        public int WaitAngleThreshold { get; set; }
        public int WaitAngleShoulderElbowHand { get; set; }
        #endregion

        public IntuiLab.Kinect.Enums.EnumPosture SavePosturerecognize { get; set; } 
        #endregion

        #region Pointing Properties

        public float PointingSpaceBetweenHands { get; set; }
        public float PointingHandsAmplitude { get; set; }

        #endregion

        #region Presentation IntuiFace Properties

        public int ExperienceIntuiFaceWidth { get; set; }
        public int ExperienceIntuifaceHeight { get; set; }

        #endregion

        #region IDisposable's members

        /// <summary>
        /// Dispose this instance.
        /// </summary>
        public void Dispose()
        {
            if (m_refInstance != null)
            {
                lock (m_refInstance)
                {
                    m_refInstance = null;
                }
            }
        }
        #endregion
    }
}
