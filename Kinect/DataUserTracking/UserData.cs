            // ****************************************************************************
            // <copyright file="UserData.cs" company="IntuiLab">
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

using System.Windows;
using Microsoft.Kinect;
using System.Windows.Media.Media3D;
using System;
using System.Collections.Generic;

using IntuiLab.Kinect.Enums;
using IntuiLab.Kinect.DataUserTracking.Events;
using IntuiLab.Kinect.GestureRecognizer.Gestures;
using IntuiLab.Kinect.GestureRecognizer.Postures;
using IntuiLab.Kinect.GestureRecognizer;
using System.Timers;
using IntuiLab.Kinect.Events;
using Microsoft.Kinect.Toolkit.Interaction;


namespace IntuiLab.Kinect.DataUserTracking
{
    /// <summary>
    /// This class permit to manage the data user (skeleton, gesture recognizer engine).
    /// This class is instantiated when the Gestures mode is enabled.
    /// </summary>
    internal class UserData : IDisposable
    {
        #region Fields

        /// <summary>
        /// Instance of gesture recognizer engine
        /// In Pointing mode, just the postures are enabled
        /// In Gestures mode, the gestures and postures are enabled.
        /// </summary>
        private GestureRecognizerManager m_refGestureRecognizerManager;

        #endregion

        #region Properties

        /// <summary>
        /// User ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Indicates if this user is the nearest
        /// </summary>
        public bool IsNearest { get; set; }

        /// <summary>
        /// Skeleton buffer for the gesture recognition
        /// </summary>
        private List<SkeletonData> m_refUserSkeleton;
        public SkeletonData UserSkeleton 
        {
            get
            {
                if (m_refUserSkeleton.Count == 0)
                {
                    return null;
                }
                else
                {
                    return m_refUserSkeleton[m_refUserSkeleton.Count - 1];
                }
            }
            set
            {
                // If the buffer is full, remove the older
                if (m_refUserSkeleton.Count >= PropertiesPluginKinect.Instance.MaxSkeletonInStorage)
                {
                    m_refUserSkeleton.RemoveAt(0);
                }

                m_refUserSkeleton.Add(value);
            }
        }

        /// <summary>
        /// User position in ColorFrame
        /// </summary>
        public Point UserPositionInColorFrame { get; set; }

        /// <summary>
        /// User position in DepthFrame
        /// </summary>
        public Point UserPositionInDepthFrame { get; set; }

        /// <summary>
        /// User/Kinect sensor distance
        /// </summary>
        public double UserDepth { get; set; }

        #endregion

        #region Events

        #region NewSkeletonData
        /// <summary>
        /// Event triggered when a new Skeleton is ready for gesture checker
        /// </summary>
        internal event EventHandler<NewSkeletonEventArgs> NewSkeletonData;

        /// <summary>
        /// Raise event NewSkeletonData
        /// </summary>
        /// <param name="skeleton"></param>
        protected void RaiseNewSkeletonData(SkeletonData skeleton)
        {
            if (NewSkeletonData != null)
            {
                NewSkeletonData(this, new NewSkeletonEventArgs(skeleton));
            }
        }
        #endregion

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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="refSkeleton">User Skeleton</param>
        /// <param name="depth">User/Kinect sensor distance</param>
        /// <param name="timesTamp">SkeletonFrame TimesTamp</param>
        public UserData(int userId, Skeleton refSkeleton, double depth, long timesTamp)
        {
            // Initialize variables
            m_refUserSkeleton = new List<SkeletonData>();
            UserPositionInColorFrame = new Point();
            UserPositionInDepthFrame = new Point();

            // Set User data
            UserID = userId;
            AddSkeleton(refSkeleton, timesTamp);
            UserDepth = depth;

            // Instantiate the gestures recognizer engine.
            // In the Gestures mode gestures and postures are enabled.
            // In the Pointing mode just postures are enabled.
            m_refGestureRecognizerManager = new GestureRecognizerManager(this);
            m_refGestureRecognizerManager.UserGestureDetected += OnUserGestureDetected;
            m_refGestureRecognizerManager.UserGestureLost += OnUserGestureLost;
            m_refGestureRecognizerManager.UserGestureProgress += OnUserGestureProgress;
            
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add Skeleton in buffer
        /// </summary>
        /// <param name="refSkeleton">Skeleton</param>
        /// <param name="timesTamp">Skeleton TimesTamp</param>
        public void AddSkeleton(Skeleton refSkeleton, long timesTamp)
        {
            // Create SkeletonData
            SkeletonData refSkeletonData = new SkeletonData(refSkeleton, timesTamp);
            UserSkeleton = refSkeletonData;

            RaiseNewSkeletonData(refSkeletonData);
        }

        /// <summary>
        /// Get a specific SkeletonData in buffer
        /// </summary>
        /// <param name="nbFrameLast">SkeletonData position in buffer</param>
        /// <returns></returns>
        public SkeletonData GetSkeletonFrameAt(int nbFrameLast)
        {
            if (nbFrameLast > m_refUserSkeleton.Count - 1 || nbFrameLast < 0)
            {
                return null;
            }

            return m_refUserSkeleton[m_refUserSkeleton.Count - nbFrameLast - 1];
        }

        /// <summary>
        /// Get the time between two SkeletonFrame (in ms.)
        /// </summary>
        /// <param name="first">First SkeletonData position in buffer</param>
        /// <param name="second">Second SkeletonData position in buffer</param>
        /// <returns></returns>
        public long MillisBetweenFrames(int first, int second) 
        {
            long diff = (GetSkeletonFrameAt(second).TimesTamp - GetSkeletonFrameAt(first).TimesTamp);
            
            return diff;
        }

        /// <summary>
        /// Get the hand's Kinect JointType
        /// </summary>
        /// <param name="hand">hand treated</param>
        /// <returns></returns>
        protected JointType GetJointToTypeHand(EnumKinectHandType hand)
        {
            if (hand == EnumKinectHandType.HAND_RIGHT)
            {
                return JointType.HandRight;
            }
            else
            {
                return JointType.HandLeft;
            }
        }

        #endregion

        #region Event's Handler

        /// <summary>
        /// Callback when the gestures recognizer engine detected a gesture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUserGestureDetected(object sender, UserGestureDetectedEventArgs e)
        {
            RaiseUserGestureDetected(e.Gesture);
        }

        /// <summary>
        /// Callback when the gestures recognizer engine lost a gesture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUserGestureLost(object sender, UserGestureLostEventArgs e)
        {
            RaiseUserGestureLost(e.Gesture);
        }

        /// <summary>
        /// Callback when the gestures recognizer engine detected a gesture is in progress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUserGestureProgress(object sender, UserGestureProgressEventArgs e)
        {
            RaiseUserGestureProgress(e.Gesture, e.Progress);
        }

        #endregion

        #region IDisposable's members

        /// <summary>
        /// Dispose this instance.
        /// </summary>
        public void Dispose()
        {
            m_refGestureRecognizerManager.Dispose();
            m_refGestureRecognizerManager.UserGestureDetected -= OnUserGestureDetected;
            m_refGestureRecognizerManager.UserGestureLost -= OnUserGestureLost;
            m_refGestureRecognizerManager.UserGestureProgress -= OnUserGestureProgress;
        }
        #endregion
    }
}
