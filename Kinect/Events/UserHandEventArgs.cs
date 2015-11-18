using System;

namespace IntuiLab.Kinect.Events
{
    public delegate void NewHandActiveEventHandler(object sender, NewHandActiveEventArgs e);

    public class NewHandActiveEventArgs : EventArgs
    {
        public int HandID { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public NewHandActiveEventArgs(int handID, int posX, int posY)
        {
            HandID = handID;
            X = posX;
            Y = posY;
        }
    }

    public delegate void UserHandMoveEventHandler(object sender, UserHandActiveEventArgs e);

    public class UserHandActiveEventArgs : EventArgs
    {
        public int HandID { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public bool IsGrip { get; set; }

        public UserHandActiveEventArgs(int handID, int posX, int posY, bool isGrip)
        {
            HandID = handID;
            X = posX;
            Y = posY;
            IsGrip = isGrip;
        }
    }

    public delegate void UserHandLostEventHandler(object sender, UserHandLostEventArgs e);

    public class UserHandLostEventArgs : EventArgs
    {
        public int HandID { get; set; }

        public UserHandLostEventArgs(int handID)
        {
            HandID = handID;
        }
    }

    public delegate void UserHandGripEventHandler(object sender, UserHandGripEventArgs e);

    public class UserHandGripEventArgs : EventArgs
    {
        public int HandID { get; set; }

        public UserHandGripEventArgs(int handID)
        {
            HandID = handID;
        }
    }

    public delegate void UserHandGripReleasedEventHandler(object sender, UserHandGripReleasedEventArgs e);

    public class UserHandGripReleasedEventArgs : EventArgs
    {
        public int HandID { get; set; }

        public UserHandGripReleasedEventArgs(int handID)
        {
            HandID = handID;
        }
    }
}
