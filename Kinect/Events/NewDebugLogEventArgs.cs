using System;

namespace IntuiLab.Kinect.Events
{
    #region Delegate
    public delegate void NewDebugLogEventHandler(object sender, NewDebugLogEventArgs e);
    #endregion

    public class NewDebugLogEventArgs : EventArgs
    {
        private string m_Log;
        public string Log
        {
            get
            {
                return m_Log;
            }
        }

        public NewDebugLogEventArgs(string log)
        {
            m_Log = log;
        }
    }
}
