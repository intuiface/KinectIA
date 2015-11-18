using Microsoft.Kinect.Toolkit.Interaction;
using System;
using System.Drawing;

namespace IntuiLab.Kinect.DataUserTracking.Events
{
    public class HandActiveEventArgs : EventArgs
    {
        public int userID { get; set; }

        public InteractionHandType HandType { get; set; }

        public bool IsActive { get; set; }

        public PointF PositionOnScreen { get; set; }
    }

    public class HandMoveEventArgs : EventArgs
    {
        public int userID { get; set; }

        public InteractionHandType HandType { get; set; }

        public PointF PositionOnScreen { get; set; }

        public PointF RawPosition { get; set; }

        public bool IsGrip { get; set; }
    }

    public class HandGripStateChangeEventArgs : EventArgs
    {
        public int userID { get; set; }

        public InteractionHandType HandType { get; set; }

        public bool IsGrip { get; set; }

        public PointF RawPosition { get; set; }
    }

    public class NewUserPointingEventArgs : EventArgs
    {
        public int UserID { get; set; }
    }

    public class DeleteUserPointingEventArgs : EventArgs
    {
        public int UserID { get; set; }
    }
}
