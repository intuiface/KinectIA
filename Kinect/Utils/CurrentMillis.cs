using System;

namespace IntuiLab.Kinect.Utils
{
    /// <summary>
    /// Get the current TimesTamp with a sufficient accuracy
    /// </summary>
    internal static class CurrentMillis
    {
        private static readonly DateTime m_baseTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long Millis
        {
            get
            {
                return (long)((DateTime.UtcNow - m_baseTime).TotalMilliseconds);
            }
        }
    }
}
