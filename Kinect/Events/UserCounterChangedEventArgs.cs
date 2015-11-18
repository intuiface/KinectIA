using System;

namespace IntuiLab.Kinect.Events
{
    public delegate void NoUserDetectedEventHandler(object sender, NoUserDetectedEventArgs e);

    public class NoUserDetectedEventArgs : EventArgs
    {
        public NoUserDetectedEventArgs()
        {
        }
    }

    public delegate void OnePersonDetectedEventHandler(object sender, OnePersonDetectedEventArgs e);

    public class OnePersonDetectedEventArgs : EventArgs
    {
        public OnePersonDetectedEventArgs()
        {
        }
    }
}
