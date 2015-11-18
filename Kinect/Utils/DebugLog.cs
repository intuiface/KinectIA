namespace IntuiLab.Kinect.Utils
{
    public static class DebugLog
    {
        private static KinectModule m_refKinectModule;

        public static void SetKinectModule(KinectModule refKinectModule)
        {
            m_refKinectModule = refKinectModule;
        }

        public static void DebugTraceLog(string trace, bool console)
        {
            string log = "Kinect Module => " + trace;

            m_refKinectModule.DisplayDebugLog(log, console);
        }
    }
}
