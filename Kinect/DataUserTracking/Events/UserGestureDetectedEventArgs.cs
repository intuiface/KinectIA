using IntuiLab.Kinect.Enums;
using System;

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
