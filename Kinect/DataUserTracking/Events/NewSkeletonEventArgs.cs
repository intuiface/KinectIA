using System;

namespace IntuiLab.Kinect.DataUserTracking
{
    internal class NewSkeletonEventArgs : EventArgs
    {
        public SkeletonData m_refSkeletonData { get; private set; }

        public NewSkeletonEventArgs(SkeletonData refSkeletonData)
        {
            m_refSkeletonData = refSkeletonData;
        }
    }
}
