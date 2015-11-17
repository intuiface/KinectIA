            // ****************************************************************************
            // <copyright file="UserGestureDetectedEventArgs.cs" company="IntuiLab">
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

using IntuiLab.Kinect.Enums;

namespace IntuiLab.Kinect.DataUserTracking.Events
{
    internal class UserGestureEventArgs : EventArgs
    {
        private EnumKinectGestureRecognize m_Gesture;
        public EnumKinectGestureRecognize Gesture
        {
            get
            {
                return m_Gesture;
            }
        }

        public UserGestureEventArgs(EnumKinectGestureRecognize refGesture)
        {
            m_Gesture = refGesture;
        }
    }

    #region UserGestureDetected
    internal delegate void UserGestureDetectedEventHandler(object sender, UserGestureDetectedEventArgs e);

    internal class UserGestureDetectedEventArgs : UserGestureEventArgs
    {
        public UserGestureDetectedEventArgs(EnumKinectGestureRecognize refGesture)
            : base(refGesture)
        {
        }
    }
    #endregion

    #region UserGestureProgress
    internal delegate void UserGestureProgressEventHandler(object sender, UserGestureProgressEventArgs e);

    internal class UserGestureProgressEventArgs : UserGestureEventArgs
    {
        private float m_refProgress;
        public float Progress
        {
            get
            {
                return m_refProgress;
            }
        }

        public UserGestureProgressEventArgs(EnumKinectGestureRecognize refGesture, float progress)
            : base(refGesture)
        {
            m_refProgress = progress;
        }
    }
    #endregion

    #region UserGestureLost
    internal delegate void UserGestureLostEventHandler(object sender, UserGestureLostEventArgs e);

    internal class UserGestureLostEventArgs : UserGestureEventArgs
    {
        public UserGestureLostEventArgs(EnumKinectGestureRecognize refGesture)
            : base(refGesture)
        {
        }
    }


    #endregion

}
