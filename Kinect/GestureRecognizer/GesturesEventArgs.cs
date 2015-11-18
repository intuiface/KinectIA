using IntuiLab.Kinect.Enums;
using System;

namespace IntuiLab.Kinect.GestureRecognizer
{
    internal abstract class GesturesEventArgs : EventArgs
    {
        public EnumGesture Gesture { get; set; }
        public EnumPosture Posture { get; set; }
    }

    internal class FailedGestureEventArgs : GesturesEventArgs
    {
        public Condition refCondition { get; set; }
    }

    internal class SuccessGestureEventArgs : GesturesEventArgs
    {
    }

    internal class BeginGestureEventArgs : GesturesEventArgs
    {
    }

    internal class EndGestureEventArgs : GesturesEventArgs
    {
    }

    internal class ProgressGestureEventArgs : GesturesEventArgs
    {
        public float Percent { get; set; }
    }
}
