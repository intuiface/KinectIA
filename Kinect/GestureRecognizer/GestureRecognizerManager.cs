            // ****************************************************************************
            // <copyright file="GestureRecognizerManager.cs" company="IntuiLab">
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
using IntuiLab.Kinect.DataUserTracking;
using IntuiLab.Kinect.GestureRecognizer.Gestures;
using IntuiLab.Kinect.GestureRecognizer.Postures;
using System.Timers;
using IntuiLab.Kinect.Enums;
using Microsoft.Kinect;
using IntuiLab.Kinect.Events;
using IntuiLab.Kinect.DataUserTracking.Events;

namespace IntuiLab.Kinect.GestureRecognizer
{
    internal class GestureRecognizerManager : IDisposable
    {
        #region Fields

        #region Gesture Checker
        /// <summary>
        /// Swipe Left Gesture checker
        /// </summary>
        private SwipeGestureChecker m_refSwipeHandRightChecker;

        /// <summary>
        /// Swipe Right Gesture checker
        /// </summary>
        private SwipeGestureChecker m_refSwipeHandLeftChecker;

        /// <summary>
        /// Wave Gesture checker on hand right
        /// </summary>
        private WaveGestureChecker m_refHandRightWaveChecker;

        /// <summary>
        /// Wave Gesture checker on hand left
        /// </summary>
        private WaveGestureChecker m_refHandLeftWaveChecker;

        /// <summary>
        /// Push Gesture checker on hand right
        /// </summary>
        private PushGestureChecker m_refPushRightHandGestureChecker;

        /// <summary>
        /// Push Gesture checker on hand left
        /// </summary>
        private PushGestureChecker m_refPushLeftHandGestureChecker;

        /// <summary>
        /// Maximize Gesture checker
        /// </summary>
        private MaximizeGestureChecker m_refMaximizeGestureChecker;

        /// <summary>
        /// Minimize Gesture checker
        /// </summary>
        private MinimizeGestureChecker m_refMinimizeGestureChecker;

        /// <summary>
        /// Posture A Gesture checker
        /// </summary>
        private PostureAChecker m_refPostureAChecker;

        /// <summary>
        /// Posture Home Gesture checker
        /// </summary>
        private PostureHomeChecker m_refPostureHomeChecker;

        /// <summary>
        /// Posture Stay Gesture checker
        /// </summary>
        private PostureStayChecker m_refPostureStayChecker;

        /// <summary>
        /// Posture T Gesture checker
        /// </summary>
        private PostureTChecker m_refPostureTChecker;

        /// <summary>
        /// Posture U Gesture checker
        /// </summary>
        private PostureUChecker m_refPostureUChecker;

        /// <summary>
        /// Posture V Gesture checker
        /// </summary>
        private PostureVChecker m_refPostureVChecker;

        /// <summary>
        /// Posture Wait Gesture checker
        /// </summary>
        private PostureWaitChecker m_refPostureWaitChecker;
        #endregion

        #region Timers

        /// <summary>
        /// Manage the delay time between two gesture recognition. 
        /// While this timer is not stop, the gesture recognition events are store in buffer.
        /// When the timer is stop, the buffer is sort and just one event gesture recognition is emit.
        /// </summary>
        private Timer m_refTimerGestureManager;

        /// <summary>
        /// This timer is used to wait for all gestures in progress the terminates (if any) 
        /// when sorting buffer must be launched
        /// </summary>
        private Timer m_refTimerDelayGesture;

        /// <summary>
        /// This timer is used to wait a some time after the detection of a posture.
        /// </summary>
        private Timer m_refTimerPostureManager;

        #endregion

        /// <summary>
        /// Buffer for store the gesture recognition event (not posture event).
        /// </summary>
        private List<EnumGesture> m_refGestureRecognized;

        /// <summary>
        /// Last posture detected
        /// </summary>
        private EnumPosture m_refPostureRecognized;

        /// <summary>
        /// Flag gesture begin
        /// </summary>
        private Dictionary<EnumGesture, bool> m_refFlagGesturesBegin;

        /// <summary>
        /// Flag posture begin
        /// </summary>
        private Dictionary<EnumPosture, bool> m_refFlagPostureBegin;

        /// <summary>
        /// Number of frame remaining before resetting flag gesture
        /// </summary>
        private int m_CounterFrameEndGesture;

        /// <summary>
        /// Indicate if we accepted the gesture event.
        /// We not authorized this events during a some of time after a posture detected.
        /// </summary>
        private bool m_refAuthorizedGesture;

        private UserData m_refUser;

        #endregion

        #region Events

        #region UserGestureDetected
        /// <summary>
        /// Event triggered when the user perform a gesture
        /// </summary>
        public event UserGestureDetectedEventHandler UserGestureDetected;

        /// <summary>
        /// Raise event UserGestureDetected
        /// </summary>
        /// <param name="gesture"></param>
        protected void RaiseUserGestureDetected(EnumKinectGestureRecognize gesture)
        {
            if (UserGestureDetected != null)
            {
                UserGestureDetected(this, new UserGestureDetectedEventArgs(gesture));
            }
        }
        #endregion

        #region UserGestureProgress

        /// <summary>
        /// Event trigerred when the user doing a gesture
        /// </summary>
        public event UserGestureProgressEventHandler UserGestureProgress;

        /// <summary>
        /// Raise event UserGestureProgress
        /// </summary>
        /// <param name="gesture"></param>
        /// <param name="percent"></param>
        protected void RaiseUserGestureProgress(EnumKinectGestureRecognize gesture, float percent)
        {
            if (UserGestureProgress != null)
            {
                UserGestureProgress(this, new UserGestureProgressEventArgs(gesture, percent));
            }
        }
        #endregion

        #region UserGestureLost

        /// <summary>
        /// Event triggered when the user stop a gesture
        /// </summary>
        public event UserGestureLostEventHandler UserGestureLost;

        /// <summary>
        /// Raise event UserGestureLost
        /// </summary>
        /// <param name="gesture"></param>
        protected void RaiseUserGestureLost(EnumKinectGestureRecognize gesture)
        {
            if (UserGestureLost != null)
            {
                UserGestureLost(this, new UserGestureLostEventArgs(gesture));
            }
        }

        #endregion

        #endregion

        #region Constructor

        public GestureRecognizerManager(UserData user)
        {
            //m_InstanceGestureRecognizerManager = this;

            m_refUser = user;

            #region Variables initialisation

            // Instantiate timer for manage gesture recognize
            m_refTimerGestureManager = new Timer();
            m_refTimerGestureManager.Interval = PropertiesPluginKinect.Instance.GestureRecognizedTime;
            m_refTimerGestureManager.Elapsed += OnTimeOutGesture;

            // Initialize buffer gesture recognition
            m_refGestureRecognized = new List<EnumGesture>();

            // Initialize gesture begin flags
            var ValueGestureAsList = System.Enum.GetValues(typeof(EnumGesture));
            m_refFlagGesturesBegin = new Dictionary<EnumGesture, bool>();
            foreach (EnumGesture gesture in ValueGestureAsList)
            {
                m_refFlagGesturesBegin.Add(gesture, false);
            }

            m_refAuthorizedGesture = true;

            // Instantiate timer for manage when a posture is recognized
            m_refTimerPostureManager = new Timer();
            m_refTimerPostureManager.Interval = PropertiesPluginKinect.Instance.PostureRecognizedTime;
            m_refTimerPostureManager.Elapsed += OnTimeOutPosture;

            //m_refPostureRecognized = EnumPosture.POSTURE_NONE;
            m_refPostureRecognized = PropertiesPluginKinect.Instance.SavePosturerecognize;

            if (PropertiesPluginKinect.Instance.SavePosturerecognize != EnumPosture.POSTURE_NONE)
            {
                m_refTimerPostureManager.Start();
            }

            // Initialize posture begin flags          
            var ValuePostureAsList = System.Enum.GetValues(typeof(EnumPosture));
            m_refFlagPostureBegin = new Dictionary<EnumPosture, bool>();
            foreach (EnumPosture posture in ValuePostureAsList)
            {
                m_refFlagPostureBegin.Add(posture, false);
            }

            // Listen the Gestures enable changement
            PropertiesPluginKinect.Instance.SynchronizeGesturesState += OnSynchronizeGesturesState;

            #endregion

            #region Gesture Checker initialisation

            if(!PropertiesPluginKinect.Instance.KinectPointingModeEnabled)
            {
                if (PropertiesPluginKinect.Instance.EnableGestureSwipeRight || PropertiesPluginKinect.Instance.EnableGestureSwipeLeft)
                {
                    m_refSwipeHandRightChecker = new SwipeGestureChecker(user, JointType.HandRight);
                    EnableGestureChecker(m_refSwipeHandRightChecker);

                    m_refSwipeHandLeftChecker = new SwipeGestureChecker(user, JointType.HandLeft);
                    EnableGestureChecker(m_refSwipeHandLeftChecker);
                }

                if (PropertiesPluginKinect.Instance.EnableGestureWave)
                {
                    m_refHandRightWaveChecker = new WaveGestureChecker(user, JointType.HandRight);
                    EnableGestureChecker(m_refHandRightWaveChecker);

                    m_refHandLeftWaveChecker = new WaveGestureChecker(user, JointType.HandLeft);
                    EnableGestureChecker(m_refHandLeftWaveChecker);
                }

                if (PropertiesPluginKinect.Instance.EnableGesturePush)
                {
                    m_refPushRightHandGestureChecker = new PushGestureChecker(user, JointType.HandRight);
                    EnableGestureChecker(m_refPushRightHandGestureChecker);

                    m_refPushLeftHandGestureChecker = new PushGestureChecker(user, JointType.HandLeft);
                    EnableGestureChecker(m_refPushLeftHandGestureChecker);
                }

                if (PropertiesPluginKinect.Instance.EnableGestureMaximize)
                {
                    m_refMaximizeGestureChecker = new MaximizeGestureChecker(user);
                    EnableGestureChecker(m_refMaximizeGestureChecker);
                }

                if (PropertiesPluginKinect.Instance.EnableGestureMinimize)
                {
                    m_refMinimizeGestureChecker = new MinimizeGestureChecker(user);
                    EnableGestureChecker(m_refMinimizeGestureChecker);
                }
            }

            if (PropertiesPluginKinect.Instance.EnablePostureA)
            {
                m_refPostureAChecker = new PostureAChecker(user);
                EnablePostureChecker(m_refPostureAChecker);
            }

            if (PropertiesPluginKinect.Instance.EnablePostureHome)
            {
                m_refPostureHomeChecker = new PostureHomeChecker(user);
                EnablePostureChecker(m_refPostureHomeChecker);
            }

            if (PropertiesPluginKinect.Instance.EnablePostureStay)
            {
                m_refPostureStayChecker = new PostureStayChecker(user);
                EnablePostureChecker(m_refPostureStayChecker);
            }

            if (PropertiesPluginKinect.Instance.EnablePostureT)
            {
                m_refPostureTChecker = new PostureTChecker(user);
                EnablePostureChecker(m_refPostureTChecker);
            }

            if (PropertiesPluginKinect.Instance.EnablePostureU)
            {
                m_refPostureUChecker = new PostureUChecker(user);
                EnablePostureChecker(m_refPostureUChecker);
            }

            if (PropertiesPluginKinect.Instance.EnablePostureV)
            {
                m_refPostureVChecker = new PostureVChecker(user);
                EnablePostureChecker(m_refPostureVChecker);
            }

            if (PropertiesPluginKinect.Instance.EnablePostureWait)
            {
                m_refPostureWaitChecker = new PostureWaitChecker(user);
                EnablePostureChecker(m_refPostureWaitChecker);
            }
            #endregion
        }

        #endregion

        #region Internal services
        /// <summary>
        /// Reset timer gesture
        /// </summary>
        private void ResetTimerGesture()
        {
            m_refTimerGestureManager.Stop();
            m_refTimerGestureManager.Start();
        }

        /// <summary>
        /// Reset timer posture
        /// </summary>
        private void ResetTimerPosture()
        {
            m_refTimerPostureManager.Stop();
            m_refTimerPostureManager.Start();
        }

        /// <summary>
        /// Sort the event gesture recognize event
        /// </summary>
        private void AcceptEventsGesture()
        {
            // Just an event in buffer
            if (m_refGestureRecognized.Count == 1)
            {
                RaiseUserGestureDetected(ConvertGestureEnum(m_refGestureRecognized[0]));
            }
            // Several events in buffer
            else if (m_refGestureRecognized.Count > 1)
            {
                // Calculate gesture recognize events ocurence
                Dictionary<EnumKinectGestureRecognize, int> occurrence = new Dictionary<EnumKinectGestureRecognize, int>();
                foreach (EnumKinectGestureRecognize gesture in m_refGestureRecognized)
                {
                    if (!occurrence.ContainsKey(gesture))
                    {
                        occurrence.Add(gesture, 1);
                    }
                    else
                    {
                        occurrence[gesture]++;
                    }
                }

                // If we have the same events in the buffer
                if (occurrence.Count == 1)
                {
                    RaiseUserGestureDetected(ConvertGestureEnum(m_refGestureRecognized[0]));
                }
                // If differents events in the buffer
                else
                {
                    // Gesture Minimize had the highest priority
                    if (m_refGestureRecognized.Contains(EnumGesture.GESTURE_MINIMIZE))
                    {
                        RaiseUserGestureDetected(EnumKinectGestureRecognize.KINECT_RECOGNIZE_MINIMIZE);
                    }
                    // Gesture Maximize had the second priority
                    else if (m_refGestureRecognized.Contains(EnumGesture.GESTURE_MAXIMIZE))
                    {
                        RaiseUserGestureDetected(EnumKinectGestureRecognize.KINECT_RECOGNIZE_MAXIMIZE);
                    }
                    // Gesture Wave had the third priority
                    else if (m_refGestureRecognized.Contains(EnumGesture.GESTURE_WAVE))
                    {
                        RaiseUserGestureDetected(EnumKinectGestureRecognize.KINECT_RECOGNIZE_WAVE);
                    }
                    // Gesture Swipe had the fourth priority
                    else if (m_refGestureRecognized.Contains(EnumGesture.GESTURE_SWIPE_LEFT) &&
                            m_refGestureRecognized.Contains(EnumGesture.GESTURE_SWIPE_RIGHT))
                    {
                        RaiseUserGestureDetected(ConvertGestureEnum(m_refGestureRecognized[0]));
                    }
                    else if (m_refGestureRecognized.Contains(EnumGesture.GESTURE_SWIPE_LEFT) ||
                            m_refGestureRecognized.Contains(EnumGesture.GESTURE_SWIPE_RIGHT))
                    {
                        if (m_refGestureRecognized.Contains(EnumGesture.GESTURE_SWIPE_LEFT))
                        {
                            RaiseUserGestureDetected(EnumKinectGestureRecognize.KINECT_RECOGNIZE_SWIPE_LEFT);
                        }
                        else if (m_refGestureRecognized.Contains(EnumGesture.GESTURE_SWIPE_RIGHT))
                        {
                            RaiseUserGestureDetected(EnumKinectGestureRecognize.KINECT_RECOGNIZE_SWIPE_RIGHT);
                        }
                    }
                    // Gesture Push had the lowest priority
                    else if (m_refGestureRecognized.Contains(EnumGesture.GESTURE_PUSH))
                    {
                        RaiseUserGestureDetected(EnumKinectGestureRecognize.KINECT_RECOGNIZE_PUSH);
                    }
                }
            }

            // Clean flag begin gesture
            m_refGestureRecognized.Clear();
            foreach (KeyValuePair<EnumGesture, bool> flag in m_refFlagGesturesBegin)
            {
                m_refFlagGesturesBegin[flag.Key] = false;
            }
        }

        /// <summary>
        /// Convert an EnumGesture to EnumKinectGestureRecognize
        /// </summary>
        /// <param name="gesture">gesture treated</param>
        /// <returns>gesture recognized</returns>
        private EnumKinectGestureRecognize ConvertGestureEnum(EnumGesture gesture)
        {
            switch (gesture)
            {
                case EnumGesture.GESTURE_SWIPE_LEFT:
                    return EnumKinectGestureRecognize.KINECT_RECOGNIZE_SWIPE_LEFT;

                case EnumGesture.GESTURE_SWIPE_RIGHT:
                    return EnumKinectGestureRecognize.KINECT_RECOGNIZE_SWIPE_RIGHT;

                case EnumGesture.GESTURE_WAVE:
                    return EnumKinectGestureRecognize.KINECT_RECOGNIZE_WAVE;

                case EnumGesture.GESTURE_PUSH:
                    return EnumKinectGestureRecognize.KINECT_RECOGNIZE_PUSH;

                case EnumGesture.GESTURE_MAXIMIZE:
                    return EnumKinectGestureRecognize.KINECT_RECOGNIZE_MAXIMIZE;

                case EnumGesture.GESTURE_MINIMIZE:
                    return EnumKinectGestureRecognize.KINECT_RECOGNIZE_MINIMIZE;
            }

            return EnumKinectGestureRecognize.KINECT_RECOGNIZE_NONE;
        }

        /// <summary>
        /// Convert EnumPosture to EnumKinectgestureRecognize
        /// </summary>
        /// <param name="posture">Posture treated</param>
        /// <returns>gesture recognized</returns>
        private EnumKinectGestureRecognize ConvertPostureEnum(EnumPosture posture)
        {
            switch (posture)
            {
                case EnumPosture.POSTURE_T:
                    return EnumKinectGestureRecognize.KINECT_RECOGNIZE_T;

                case EnumPosture.POSTURE_V:
                    return EnumKinectGestureRecognize.KINECT_RECOGNIZE_V;

                case EnumPosture.POSTURE_A:
                    return EnumKinectGestureRecognize.KINECT_RECOGNIZE_A;

                case EnumPosture.POSTURE_U:
                    return EnumKinectGestureRecognize.KINECT_RECOGNIZE_U;

                case EnumPosture.POSTURE_WAIT:
                    return EnumKinectGestureRecognize.KINECT_RECOGNIZE_WAIT;

                case EnumPosture.POSTURE_HOME:
                    return EnumKinectGestureRecognize.KINECT_RECOGNIZE_HOME;

                case EnumPosture.POSTURE_STAY:
                    return EnumKinectGestureRecognize.KINECT_RECOGNIZE_STAY;
            }

            return EnumKinectGestureRecognize.KINECT_RECOGNIZE_NONE;
        }

        private void EnableGestureChecker(GestureChecker checker)
        {
            checker.Successful += OnGestureDetected;
            checker.GestureBegin += OnGestureBegining;
            checker.GestureEnd += OnGestureEnded;
            checker.GestureProgress += OnGestureProgress;
        }

        private void EnablePostureChecker(GestureChecker checker)
        {
            checker.Successful += OnPosture;
            checker.GestureBegin += OnGestureBegining;
            checker.GestureEnd += OnGestureEnded;
            checker.GestureProgress += OnGestureProgress;
        }

        private void DisableGestureChecker(GestureChecker checker)
        {
            if (checker != null)
            {
                checker.Successful -= OnGestureDetected;
                checker.GestureBegin -= OnGestureBegining;
                checker.GestureEnd -= OnGestureEnded;
                checker.GestureProgress -= OnGestureProgress;

                checker.Dispose();

                checker = null;
            }
        }

        private void DisablePostureChecker(GestureChecker checker)
        {
            if (checker != null)
            {
                checker.Successful -= OnPosture;
                checker.GestureBegin -= OnGestureBegining;
                checker.GestureEnd -= OnGestureEnded;
                checker.GestureProgress -= OnGestureProgress;

                checker.Dispose();

                checker = null;
            }
        }
        #endregion

        #region Event's Handler

        /// <summary>
        /// Callback when a gestures checker detect a gesture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGestureDetected(object sender, GesturesEventArgs e)
        {
            if (m_refAuthorizedGesture)
            {
                // Add event in buffer
                m_refGestureRecognized.Add(e.Gesture);
                ResetTimerGesture();
            }
        }

        /// <summary>
        /// Callback when a gesture checker detect a posture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPosture(object sender, GesturesEventArgs e)
        {
            // Posture detect in first time
            //if (m_refPostureRecognized != e.Posture)
            if(PropertiesPluginKinect.Instance.SavePosturerecognize != e.Posture)
            {
                RaiseUserGestureDetected(ConvertPostureEnum(e.Posture));
            }
            m_refPostureRecognized = e.Posture;
            PropertiesPluginKinect.Instance.SavePosturerecognize = e.Posture;
            m_refAuthorizedGesture = false;
            ResetTimerPosture();
        }

        /// <summary>
        /// Callback when gestures checker detect a gesture begining
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGestureBegining(object sender, BeginGestureEventArgs e)
        {
            // If event emit by a gesture
            if (e.Gesture != EnumGesture.GESTURE_NONE)
            {
                if (m_refFlagGesturesBegin.ContainsKey(e.Gesture))
                {
                    m_refFlagGesturesBegin[e.Gesture] = true;
                }
            }
            // If event emit by a posture
            //else if (e.Posture != EnumPosture.POSTURE_NONE)
            else if (PropertiesPluginKinect.Instance.SavePosturerecognize != EnumPosture.POSTURE_NONE)
            {
                if (PropertiesPluginKinect.Instance.SavePosturerecognize != e.Posture)
                {
                    RaiseUserGestureProgress(ConvertPostureEnum(e.Posture), 0f);
                }
            }
        }

        private void OnGestureProgress(object sender, ProgressGestureEventArgs e)
        {
            if (e.Posture != EnumPosture.POSTURE_NONE && PropertiesPluginKinect.Instance.SavePosturerecognize != e.Posture)
            {
                RaiseUserGestureProgress(ConvertPostureEnum(e.Posture), e.Percent);
            }
        }

        /// <summary>
        /// Callback when gestures checker detect a gesture ending
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGestureEnded(object sender, EndGestureEventArgs e)
        {
            // if event emit by a gesture
            if (e.Gesture != EnumGesture.GESTURE_NONE)
            {
                if (m_refFlagGesturesBegin.ContainsKey(e.Gesture))
                {
                    m_refFlagGesturesBegin[e.Gesture] = false;
                }
            }
            // if event emit by a posture
            else if (e.Posture != EnumPosture.POSTURE_NONE)
            {
                if (PropertiesPluginKinect.Instance.SavePosturerecognize != e.Posture)
                {
                    RaiseUserGestureLost(ConvertPostureEnum(e.Posture));
                }
            }
        }

        /// <summary>
        /// Callback when thimer posture manager is stoped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimeOutPosture(object sender, ElapsedEventArgs e)
        {
            m_refTimerPostureManager.Stop();
            m_refPostureRecognized = EnumPosture.POSTURE_NONE;
            PropertiesPluginKinect.Instance.SavePosturerecognize = EnumPosture.POSTURE_NONE;
            m_refAuthorizedGesture = true;
        }

        /// <summary>
        /// Callback when timer gesture manager is stopped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimeOutGesture(object sender, ElapsedEventArgs e)
        {
            m_refTimerGestureManager.Stop();
            // If gestures is begining, run timer delay for wait the end of gestures
            if (m_refFlagGesturesBegin.ContainsValue(true))
            {
                m_CounterFrameEndGesture = 0;
                m_refTimerDelayGesture = new Timer();
                m_refTimerDelayGesture.Interval = PropertiesPluginKinect.Instance.TimerDelayGesture;
                m_refTimerDelayGesture.Elapsed += OnTicDelayGesture;
                m_refTimerDelayGesture.Start();
            }
            // Else sort buffer
            else
            {
                AcceptEventsGesture();
            }
        }

        /// <summary>
        /// Callback when tic timer delay gesture 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTicDelayGesture(object sender, ElapsedEventArgs e)
        {
            if (m_CounterFrameEndGesture < PropertiesPluginKinect.Instance.NumberFrameForEndGesture)
            {
                if (!m_refFlagGesturesBegin.ContainsValue(true))
                {
                    m_refTimerDelayGesture.Stop();
                    m_refTimerDelayGesture.Dispose();
                    m_refTimerDelayGesture = null;

                    AcceptEventsGesture();
                }
                m_CounterFrameEndGesture++;

            }
            else
            {
                m_refTimerDelayGesture.Stop();
                m_refTimerDelayGesture.Dispose();
                m_refTimerDelayGesture = null;

                AcceptEventsGesture();
            }

        }

        /// <summary>
        /// Callback when a gesture state is modified during operation of the engine 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSynchronizeGesturesState(object sender, SynchronizeGesturesStateEventArgs e)
        {
            switch (e.GestureName)
            {
                case "SwipeLeft":
                    if (!PropertiesPluginKinect.Instance.EnableGestureSwipeLeft)
                    {
                        if (!PropertiesPluginKinect.Instance.EnableGestureSwipeRight && m_refSwipeHandLeftChecker != null && m_refSwipeHandRightChecker != null)
                        {
                            DisableGestureChecker(m_refSwipeHandLeftChecker);
                            DisableGestureChecker(m_refSwipeHandRightChecker);
                        }
                    }
                    else
                    {
                        if (!PropertiesPluginKinect.Instance.EnableGestureSwipeRight)
                        {
                            m_refSwipeHandRightChecker = new SwipeGestureChecker(m_refUser, JointType.HandRight);
                            EnableGestureChecker(m_refSwipeHandRightChecker);

                            m_refSwipeHandLeftChecker = new SwipeGestureChecker(m_refUser, JointType.HandLeft);
                            EnableGestureChecker(m_refSwipeHandLeftChecker);
                        }
                    }
                    break;

                case "SwipeRight":
                    if (!PropertiesPluginKinect.Instance.EnableGestureSwipeRight)
                    {
                        if (!PropertiesPluginKinect.Instance.EnableGestureSwipeLeft && m_refSwipeHandLeftChecker != null && m_refSwipeHandRightChecker != null)
                        {
                            DisableGestureChecker(m_refSwipeHandLeftChecker);
                            DisableGestureChecker(m_refSwipeHandRightChecker);
                        }
                    }
                    else
                    {
                        if (!PropertiesPluginKinect.Instance.EnableGestureSwipeLeft)
                        {
                            m_refSwipeHandRightChecker = new SwipeGestureChecker(m_refUser, JointType.HandRight);
                            EnableGestureChecker(m_refSwipeHandRightChecker);

                            m_refSwipeHandLeftChecker = new SwipeGestureChecker(m_refUser, JointType.HandLeft);
                            EnableGestureChecker(m_refSwipeHandLeftChecker);
                        }
                    }
                    break;

                case "Wave":
                    if (!PropertiesPluginKinect.Instance.EnableGestureWave)
                    {
                        DisableGestureChecker(m_refHandLeftWaveChecker);
                        DisableGestureChecker(m_refHandRightWaveChecker);
                    }
                    else
                    {
                        m_refHandRightWaveChecker = new WaveGestureChecker(m_refUser, JointType.HandRight);
                        EnableGestureChecker(m_refHandLeftWaveChecker);


                        m_refHandLeftWaveChecker = new WaveGestureChecker(m_refUser, JointType.HandLeft);
                        EnableGestureChecker(m_refHandRightWaveChecker);
                    }
                    break;

                case "Grip":
                    if (!PropertiesPluginKinect.Instance.EnableGestureGrip)
                    {
                        // Nothing to do ?
                    }
                    break;
                case "Push":
                    if (!PropertiesPluginKinect.Instance.EnableGesturePush)
                    {
                        DisableGestureChecker(m_refPushLeftHandGestureChecker);
                        DisableGestureChecker(m_refPushRightHandGestureChecker);
                    }
                    else
                    {
                        m_refPushRightHandGestureChecker = new PushGestureChecker(m_refUser, JointType.HandRight);
                        EnableGestureChecker(m_refPushRightHandGestureChecker);

                        m_refPushLeftHandGestureChecker = new PushGestureChecker(m_refUser, JointType.HandLeft);
                        EnableGestureChecker(m_refPushLeftHandGestureChecker);
                    }
                    break;

                case "Maximize":
                    if (!PropertiesPluginKinect.Instance.EnableGestureMaximize)
                    {
                        DisableGestureChecker(m_refMaximizeGestureChecker);
                    }
                    else
                    {
                        m_refMaximizeGestureChecker = new MaximizeGestureChecker(m_refUser);
                        EnableGestureChecker(m_refMaximizeGestureChecker);
                    }
                    break;

                case "Minimize":
                    if (!PropertiesPluginKinect.Instance.EnableGestureMinimize)
                    {
                        DisableGestureChecker(m_refMinimizeGestureChecker);
                    }
                    else
                    {
                        m_refMinimizeGestureChecker = new MinimizeGestureChecker(m_refUser);
                        EnableGestureChecker(m_refMinimizeGestureChecker);
                    }
                    break;

                case "A":
                    if (!PropertiesPluginKinect.Instance.EnablePostureA)
                    {
                        DisablePostureChecker(m_refPostureAChecker);
                    }
                    else
                    {
                        m_refPostureAChecker = new PostureAChecker(m_refUser);
                        EnablePostureChecker(m_refPostureAChecker);
                    }
                    break;

                case "Home":
                    if (!PropertiesPluginKinect.Instance.EnablePostureHome)
                    {
                        DisableGestureChecker(m_refPostureHomeChecker);
                    }
                    else
                    {
                        m_refPostureHomeChecker = new PostureHomeChecker(m_refUser);
                        EnablePostureChecker(m_refPostureHomeChecker);
                    }
                    break;

                case "Stay":
                    if (!PropertiesPluginKinect.Instance.EnablePostureStay)
                    {
                        DisablePostureChecker(m_refPostureHomeChecker);
                    }
                    else
                    {
                        m_refPostureHomeChecker = new PostureHomeChecker(m_refUser);
                        EnablePostureChecker(m_refPostureHomeChecker);
                    }
                    break;

                case "T":
                    if (!PropertiesPluginKinect.Instance.EnablePostureT)
                    {
                        DisablePostureChecker(m_refPostureTChecker);
                    }
                    else
                    {
                        m_refPostureTChecker = new PostureTChecker(m_refUser);
                        EnablePostureChecker(m_refPostureTChecker);
                    }
                    break;

                case "U":
                    if (!PropertiesPluginKinect.Instance.EnablePostureU)
                    {
                        DisablePostureChecker(m_refPostureUChecker);
                    }
                    else
                    {
                        m_refPostureUChecker = new PostureUChecker(m_refUser);
                        EnablePostureChecker(m_refPostureUChecker);
                    }
                    break;

                case "V":
                    if (!PropertiesPluginKinect.Instance.EnablePostureV)
                    {
                        DisablePostureChecker(m_refPostureVChecker);
                    }
                    else
                    {
                        m_refPostureVChecker = new PostureVChecker(m_refUser);
                        EnablePostureChecker(m_refPostureVChecker);
                    }
                    break;

                case "Wait":
                    if (!PropertiesPluginKinect.Instance.EnablePostureWait)
                    {
                        DisablePostureChecker(m_refPostureWaitChecker);
                    }
                    else
                    {
                        m_refPostureWaitChecker = new PostureWaitChecker(m_refUser);
                        EnablePostureChecker(m_refPostureWaitChecker);
                    }
                    break;
            }
        }

        #endregion

        #region IDisposable's members

        /// <summary>
        /// Dispose this instance.
        /// </summary>
        public void Dispose()
        {

            if (m_refTimerGestureManager != null)
            {
                m_refTimerGestureManager.Elapsed -= OnTimeOutGesture;
                m_refTimerGestureManager.Stop();
                m_refTimerGestureManager.Dispose();
            }

            if (m_refTimerPostureManager != null)
            {
                m_refTimerPostureManager.Elapsed -= OnTimeOutPosture;
                m_refTimerPostureManager.Stop();
                m_refTimerPostureManager.Dispose();
            }

            if (m_refTimerDelayGesture != null)
            {
                m_refTimerDelayGesture.Elapsed -= OnTicDelayGesture;
                m_refTimerDelayGesture.Stop();
                m_refTimerDelayGesture.Dispose();
            }

            PropertiesPluginKinect.Instance.SynchronizeGesturesState -= OnSynchronizeGesturesState;

            DisableGestureChecker(m_refSwipeHandLeftChecker);
            DisableGestureChecker(m_refSwipeHandRightChecker);
            DisableGestureChecker(m_refHandLeftWaveChecker);
            DisableGestureChecker(m_refHandRightWaveChecker);
            DisableGestureChecker(m_refPushLeftHandGestureChecker);
            DisableGestureChecker(m_refPushRightHandGestureChecker);
            DisableGestureChecker(m_refMaximizeGestureChecker);
            DisableGestureChecker(m_refMinimizeGestureChecker);
            DisablePostureChecker(m_refPostureAChecker);
            DisablePostureChecker(m_refPostureTChecker);
            DisablePostureChecker(m_refPostureUChecker);
            DisablePostureChecker(m_refPostureVChecker);
            DisablePostureChecker(m_refPostureWaitChecker);
        }
        #endregion
    }
}
