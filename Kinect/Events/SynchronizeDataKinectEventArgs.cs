using System;

namespace IntuiLab.Kinect.Events
{
    public class SynchronizeElevationAngleEventArgs : EventArgs
    {
        public int Elevation { get; set; }
    }

    public class SynchronizeColorStreamActivationEventArgs : EventArgs
    {
        public bool Enable { get; set; }
    }

    public class SynchronizeDepthStreamActivationEventArgs : EventArgs
    {
        public bool Enable { get; set; }
    }

    public class SynchronizeGesturesStateEventArgs : EventArgs
    {
        public string GestureName { get; set; }
    }

    public class SynchronizeKinectModeEventArgs : EventArgs
    {
        public bool PointingMode { get; set; }
    }
}
