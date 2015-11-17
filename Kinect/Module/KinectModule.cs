            // ****************************************************************************
            // <copyright file="KinectModule.cs" company="IntuiLab">
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
using System.ComponentModel;
using System.Threading;
using System.IO;

using Microsoft.Kinect;

using IntuiLab.Kinect.Events;
using IntuiLab.Kinect.Enums;
using IntuiLab.Kinect.DataUserTracking;
using IntuiLab.Kinect.Utils;
using IntuiLab.Kinect.Exceptions;
using Microsoft.Kinect.Toolkit.Interaction;
using IntuiLab.Kinect.DummyKinectInteraction;

namespace IntuiLab.Kinect
{
    /// <summary>
    /// Principal class manages the operation of te Kinect sensor.
    /// Provides the public methods for manage kinect and the kinect main thread.
    /// </summary>
    public partial class KinectModule : INotifyPropertyChanged, IDisposable
    {
        #region Constants

        // Kinect stream parameters
        private const ColorImageFormat KINECT_DEFAULT_COLOR_STREAM_FORMAT = ColorImageFormat.RgbResolution640x480Fps30;
        private const DepthImageFormat KINECT_DEFAULT_DEPTH_STREAM_FORMAT = DepthImageFormat.Resolution640x480Fps30;

        // Kinect tracking mode parameter
        private const SkeletonTrackingMode KINECT_DEFAULT_SKELETON_TRACKING_MODE = SkeletonTrackingMode.Default;

        #endregion

        #region Fields

        /// <summary>
        /// Kinect main thread
        /// </summary>
        private Thread m_thMainThreadKinect;

        /// <summary>
        /// Instance of the kinect InteractionStream
        /// </summary>
        private InteractionStream m_refKinectInteraction;

        private static Queue<Action> m_lstTask = new Queue<Action>();
        private static ManualResetEventSlim m_resetEvent = new ManualResetEventSlim();
                
        #endregion

        #region KinectModule Membres

        #region NotifyProperties Changed

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

        #region Events

        #region DebugLog

        /// <summary>
        /// Event triggered when a new log is ready
        /// </summary>
        public event NewDebugLogEventHandler LogTrace;

        /// <summary>
        /// Raise event LogTrace
        /// </summary>
        /// <param name="log"></param>
        protected void RaiseLogTrace(string log)
        {
            if (LogTrace != null)
            {
                LogTrace(this, new NewDebugLogEventArgs(log));
            }
        }

        #endregion

        #endregion

        #endregion

        #region Properties

        #region InstanceKinectSensor

        /// <summary>
        /// Instance of Kinect sensor
        /// </summary>
        private KinectSensor m_refKinectsensor;
        internal KinectSensor InstanceKinectSensor
        {
            get
            {
                return m_refKinectsensor;
            }
        }

        #endregion

        #region IsRunning

        /// <summary>
        /// Indicates if the kinect sensor is running
        /// </summary>
        private volatile bool m_IsRunning;
        public bool IsRunning
        {
            get
            {
                return m_IsRunning;
            }
            set
            {
                if (m_IsRunning != value)
                {
                    m_IsRunning = value;

                    if (m_IsRunning)
                    {
                        State = EnumKinectModuleState.KINECT_MODULE_RUNNING;
                    }
                    else
                    {
                        State = EnumKinectModuleState.KINECT_MODULE_INITIALIZED;
                    }
                    NotifyPropertyChanged("IsRunning");
                }
            }
        }
        #endregion

        #region IsLocked
        
        /// <summary>
        /// Indicates if the kinect sensor system is locked (user too close or too far)
        /// </summary>
        private volatile bool m_IsLocked; 
        public bool IsLocked
        {
            get
            {
                return m_IsLocked;
            }
            set
            {
                if (m_IsLocked != value)
                {
                    m_IsLocked = value;
                    NotifyPropertyChanged("IsLocked");
                }
            }
        }
        #endregion

        #region State

        /// <summary>
        /// Indicates the kinect module state
        ///     - KINECT_MODULE_INITIALIZED,
        ///     - KINECT_MODULE_ERROR,
        ///     - KINECT_MODULE_RUNNING,
        ///     - KINECT_MODULE_STOPPED,
        ///     - KINECT_MODULE_WRONG_DEVICE
        /// </summary>
        private EnumKinectModuleState m_State = EnumKinectModuleState.KINECT_MODULE_STOPPED;
        public EnumKinectModuleState State
        {
            get
            {
                return m_State;
            }
            set
            {
                if (m_State != value)
                {
                    m_State = value;
                    NotifyPropertyChanged("State");
                }
            }
        }
        #endregion

        #region SkeletonTrackingMode

        /// <summary>
        /// Indicates the tracking mode of kinect sensor
        /// </summary>
        private SkeletonTrackingMode m_refTrackingMode;
        public SkeletonTrackingMode TrackingMode
        {
            get
            {
                return m_refTrackingMode;
            }
            set
            {
                if (m_refTrackingMode != value)
                {
                    m_refTrackingMode = value;
                    NotifyPropertyChanged("TrackingMode");

                    m_refKinectsensor.SkeletonStream.TrackingMode = m_refTrackingMode;
                }
            }
        }
        #endregion

        #region SpeechRecognizeResult

        /// <summary>
        /// Indicates the result of SpeechRecognizerManager
        /// </summary>
        private string m_SpeechRecognizeResult;
        public string SpeechRecognizeResult
        {
            get
            {
                return m_SpeechRecognizeResult;
            }
            set
            {
                if (m_SpeechRecognizeResult != value)
                {
                    m_SpeechRecognizeResult = value;
                    NotifyPropertyChanged("SpeechRecognizeResult");
                }
            }
        }
        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public KinectModule()
        {
            // Initialise the DebugLog
            DebugLog.SetKinectModule(this);

            m_bRecData = false;
            m_refDataUserRecorder = null;
            
            PropertiesPluginKinect.Instance.UserCounter = 0;
            m_IsLocked = false;

            State = EnumKinectModuleState.KINECT_MODULE_INITIALIZED;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Start the Kinect sensor
        /// </summary>
        public void Start()
        {
            m_refDataUserRecorder = null;
            m_refDataUserReplay = null;
            m_bRecData = false;

            // If already running, stop the kinect sensor
            if (IsRunning)
            {
                Stop();
            }

            if (State == EnumKinectModuleState.KINECT_MODULE_INITIALIZED)
            {
                // Notify kinect is running
                IsRunning = true;

                // Create the kinect main thread
                m_thMainThreadKinect = new Thread(kinectMainThread);

                // Start the main thread
                m_thMainThreadKinect.Start();
            }
            else
            {
                DisplayDebugLog("Kinect 'Start()' => la kinect ne peut être initialisé", true);
            }
        }

        /// <summary>
        /// Stop the Kinect sensor
        /// </summary>
        public void Stop()
        {
            // Notify kinect is stoped
            // This action stop the kinect main thread and clean up all variable of systeme
            IsRunning = false;
        }

        /// <summary>
        /// Lock the Kinect sensor
        /// </summary>
        public void Locked()
        {
            // Notify kinect is locked
            IsLocked = true;
        }

        /// <summary>
        /// Unlock the kinect sensor
        /// </summary>
        public void Unlocked()
        {
            // Notify kinect is unlocked
            IsLocked = false;
        }
        #endregion

        #region Internal services

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern int MessageBox(IntPtr hWnd, String text, String caption, int options);

        /// <summary>
        /// Initialize kinect sensor.
        /// </summary>
        /// <returns>boolean indicates if the sensor is well initialized</returns>
        private bool InitializeKinectSensor()
        {
            try
            {
                // If a Kinect sensor is found
                if (KinectSensor.KinectSensors.Count > 0)
                {
                    // We recover the first kinect sensor
                    m_refKinectsensor = KinectSensor.KinectSensors[0];
                }
                else // Display error message
                {
                    m_refKinectsensor = null;

                    MessageBox(IntPtr.Zero, "Looks like your Kinect sensor is unplugged. Be sure to plug it in before using any of IntuiFace’s Kinect interfaces.",
                                                                        "Kinect Sensor Missing", 0x001000);
                }
            }
            catch (Exception)
            {
                IsRunning = false;
                m_refKinectsensor = null;

                return false;
            }

            return (m_refKinectsensor != null);
        }

        /// <summary>
        /// Clean the kinect module
        /// </summary>
        private void CleanUpKinect()
        {
            try
            {
                if (m_refKinectsensor != null)
                {
                    // Disconnect user callback
                    if (m_refListUsers.Count > 0)
                    {
                        List<int> listUserID = new List<int>(m_refListUsers.Keys);

                        foreach (int userId in listUserID)
                        {
                            DeleteUserData(userId);
                        }
                    }

                    // Finished the recording if is recording
                    if (m_bRecData && m_refDataUserRecorder != null)
                    {
                        m_refDataUserRecorder.EndRecording();
                    }

                    m_refKinectsensor.AllFramesReady -= OnAllFramesReady;

                    // Disconnect synchronize callback
                    PropertiesPluginKinect.Instance.SynchronizeElevationAngle -= OnSynchronizeElevationAngle;
                    PropertiesPluginKinect.Instance.SynchronizeColorStreamActivation -= OnSynchronizeColorStream;
                    PropertiesPluginKinect.Instance.SynchronizeDepthStreamActivation -= OnSynchronizeDepthStream;
                    PropertiesPluginKinect.Instance.SynchronizeKinectMode -= OnSynchronizeKinectMode;
                }
            }
            catch (Exception ex)
            {
                DebugLog.DebugTraceLog("Error during Kinect cleanup" + ex.Message, true);
            }
        }

        #endregion

        #region Event's Handlers

        /// <summary>
        /// Kinect main thread
        /// </summary>
        private void kinectMainThread()
        {
            // Initialize sensor
            if (InitializeKinectSensor())
            {
                DisplayDebugLog("kinect main thread => initialisation success", true);
                State = EnumKinectModuleState.KINECT_MODULE_INITIALIZED;
            }
            else
            {
                DisplayDebugLog("kinect main thread => initialisation fail", true);
                State = EnumKinectModuleState.KINECT_MODULE_ERROR;
            }

            // Sensor is initialized
            if (State == EnumKinectModuleState.KINECT_MODULE_INITIALIZED)
            {
                try
                {
                    m_refListUsers = new Dictionary<int, UserData>();

                    // Connect to the synchronize event
                    PropertiesPluginKinect.Instance.SynchronizeElevationAngle += OnSynchronizeElevationAngle;
                    PropertiesPluginKinect.Instance.SynchronizeColorStreamActivation += OnSynchronizeColorStream;
                    PropertiesPluginKinect.Instance.SynchronizeDepthStreamActivation += OnSynchronizeDepthStream;
                    PropertiesPluginKinect.Instance.SynchronizeKinectMode += OnSynchronizeKinectMode;

                    // Activation of services desired by the user at start of kinect
                    if(PropertiesPluginKinect.Instance.EnableColorFrameService)
                    {
                        m_refKinectsensor.ColorStream.Enable(KINECT_DEFAULT_COLOR_STREAM_FORMAT);
                    }

                    if (PropertiesPluginKinect.Instance.EnableDepthFrameService ||
                        PropertiesPluginKinect.Instance.EnableGestureGrip ||
                        PropertiesPluginKinect.Instance.KinectPointingModeEnabled)
                    {
                        // Need depth stream
                        DebugLog.DebugTraceLog("Enable depth stream because required by Depth frame, Grip or Pointing", true);
                        m_refKinectsensor.DepthStream.Enable(KINECT_DEFAULT_DEPTH_STREAM_FORMAT);
                    }

                    TransformSmoothParameters smoothingParam = new TransformSmoothParameters();
                    {
                        smoothingParam.Smoothing = PropertiesPluginKinect.Instance.KinectSkeletonSmoothing;
                        smoothingParam.Correction = PropertiesPluginKinect.Instance.KinectSkeletonCorrection;
                        smoothingParam.Prediction = PropertiesPluginKinect.Instance.KinectSkeletonPrediction;
                        smoothingParam.JitterRadius = PropertiesPluginKinect.Instance.KinectSkeletonJitter;
                        smoothingParam.MaxDeviationRadius = PropertiesPluginKinect.Instance.KinectSkeletonMaxDeviation;
                    };
                    m_refKinectsensor.SkeletonStream.Enable(smoothingParam);
                    m_refKinectsensor.SkeletonStream.TrackingMode = KINECT_DEFAULT_SKELETON_TRACKING_MODE;

                    // Notify when all kinect frames are ready
                    m_refKinectsensor.AllFramesReady += OnAllFramesReady;
                    
                    // Create the InteractionStream
                    SynchronizeInteractionStream();
                    
                    // Start Kinect sensor
                    m_refKinectsensor.Start();

                    Action currentTask = null;

                    //Main loop
                    while (IsRunning)
                    {
                        lock (m_lstTask)
                        {
                            if (m_lstTask.Count != 0)
                            {
                                m_resetEvent.Set();
                            }
                        }
                        m_resetEvent.Wait();

                        lock (m_lstTask)
                        {
                            if (m_lstTask.Count != 0)
                            {
                                currentTask = m_lstTask.Dequeue();
                            }
                        }

                        if (currentTask != null)
                        {
                            currentTask.Invoke();
                            m_resetEvent.Reset();
                        }
                    }
                }
                catch (Exception e)
                {
                    DisplayDebugLog("Kinect main Thread => " + e.Message, true);
                    //throw new KinectException(e.Message);
                }
            }

            DisplayDebugLog("Kinect main thread => during the stop", true);

            // Clean kinect module's variables
            CleanUpKinect();

            DisplayDebugLog("Kinect main thread => stop made", true);
        }

        /// <summary>
        /// Callback when the elevation of kinect sensor is modified in PropertiesKinect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSynchronizeElevationAngle(object sender, SynchronizeElevationAngleEventArgs e)
        {
            if (m_refKinectsensor != null)
            {
                try
                {
                    m_refKinectsensor.ElevationAngle = e.Elevation;
                }
                catch (InvalidOperationException ex)
                {
                    DisplayDebugLog("Exception Elevation angle", true);
                    throw new KinectException(ex.Message);
                }
            }
        }

        /// <summary>
        /// Callback when the color stream state of kinect sensor is modified in PropertiesKinect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSynchronizeColorStream(object sender, SynchronizeColorStreamActivationEventArgs e)
        {
            if(m_refKinectsensor != null)
            {
                if(e.Enable)
                {
                    m_refKinectsensor.ColorStream.Enable(KINECT_DEFAULT_COLOR_STREAM_FORMAT);
                }
                else
                {
                    m_refKinectsensor.ColorStream.Disable();
                }
            }
        }

        /// <summary>
        /// Callback when the depth stream state of kinect sensor is modified in PropertiesKinect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSynchronizeDepthStream(object sender, SynchronizeDepthStreamActivationEventArgs e)
        {
            if (m_refKinectsensor != null)
            {
                if (PropertiesPluginKinect.Instance.KinectPointingModeEnabled ||
                    PropertiesPluginKinect.Instance.EnableGestureGrip)
                {
                    // Need the depth stream
                    DebugLog.DebugTraceLog("Enable depth stream because Pointing mode or Grip is enabled", true);
                    m_refKinectsensor.DepthStream.Enable(KINECT_DEFAULT_DEPTH_STREAM_FORMAT);
                }
                else
                {
                    // Regular management
                    if (e.Enable)
                    {
                        DebugLog.DebugTraceLog("Enable depth stream because required by the Feedback", true);
                        m_refKinectsensor.DepthStream.Enable(KINECT_DEFAULT_DEPTH_STREAM_FORMAT);
                    }
                    else
                    {
                        DebugLog.DebugTraceLog("DISABLE depth stream because NOT required by the Feedback", true);
                        m_refKinectsensor.DepthStream.Disable();
                    }
                }
            }
        }

        /// <summary>
        /// Synchronize the Interaction stream management
        /// </summary>
        public void SynchronizeInteractionStream()
        {
            if (PropertiesPluginKinect.Instance.KinectPointingModeEnabled || PropertiesPluginKinect.Instance.EnableGestureGrip)
            {
                if (m_refKinectInteraction == null)
                {
                    try
                    {
                        m_refKinectInteraction = new InteractionStream(m_refKinectsensor, new DummyInteractionClient());
                        m_refKinectInteraction.InteractionFrameReady += OnKinectInteractionFrameReady;

                        // Always need depth stream
                        m_refKinectsensor.DepthStream.Enable(KINECT_DEFAULT_DEPTH_STREAM_FORMAT);
                    }
                    catch (InvalidOperationException ex)
                    {
                        throw new KinectException(ex.Message);
                    }
                }

            }
            else
            {
                if (m_refKinectInteraction != null)
                {
                    // No noeed to have kinect interaction stream
                    lock (m_refKinectInteraction)
                    {
                        m_refKinectInteraction.InteractionFrameReady -= OnKinectInteractionFrameReady;
                        m_refKinectInteraction.Dispose();
                        m_refKinectInteraction = null;
                    }
                }

            }
        }

        public delegate void SynchronizeKinectModeState();

        private void OnSynchronizeKinectMode(object sender, SynchronizeKinectModeEventArgs e)
        {
            if (m_refKinectsensor != null)
            {                
                if (m_refListUsers.Count > 0)
                {
                    List<int> listUserID = new List<int>(m_refListUsers.Keys);

                    foreach (int userId in listUserID)
                    {
                        DeleteUserData(userId);
                    }
                }

                lock (m_lstTask)
                {
                    m_lstTask.Enqueue(new Action((SynchronizeKinectModeState)SynchronizeInteractionStream));
                }
                m_resetEvent.Set();
            }
        }
        #endregion

        #region DebugLog

        /// <summary>
        /// Display or send a debug log
        /// </summary>
        /// <param name="log">Debug log</param>
        /// <param name="console">Indicate if the debug log must be display in Console or send out of the dll</param>
        internal void DisplayDebugLog(string log, bool console)
        {
            if (console)
            {
                Console.WriteLine("\n\n" + log + "\n\n");
            }
            else
            {
                RaiseLogTrace(log);
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
                if (m_thMainThreadKinect != null)
                {
                    // Check if the thread is always alive
                    if (m_thMainThreadKinect.IsAlive)
                    {
                        IsRunning = false;
                        m_resetEvent.Set();
                        m_thMainThreadKinect.Join();
                    }
                    m_thMainThreadKinect = null;
                }
            }

            DisplayDebugLog("Dispose - Kinect module disposed", true);
        }

        #endregion
    }
}
