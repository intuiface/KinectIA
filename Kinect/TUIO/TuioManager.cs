using IntuiLab.Kinect.TUIO.CursorKinect;
using System;
using System.Drawing;
using System.Threading;

namespace IntuiLab.Kinect.TUIO
{
    public class TuioManager : IDisposable
    {
        private static TuioKinect m_refTuioKinect;
        private bool m_RunThread;

        public TuioManager()
        {
            m_refTuioKinect = new TuioKinect();
            m_RunThread = true;
            ThreadPool.QueueUserWorkItem(ThreadPoolCallback);
        }

        private void ThreadPoolCallback(Object threadContext)
        {
            while (m_RunThread)
            {
                try
                {
                    m_refTuioKinect.InitFrame();
                    m_refTuioKinect.CommitFrame();
                }
                catch (Exception ex )
                {
                    // Do not crash if unable to send TUIO frame
                    Console.Error.WriteLine("Error sending TUIO frame : " + ex.Message + " _ " + ex.StackTrace);
                }
                Thread.Sleep(25);
            }
        }

        public void AddTuioHandKinect(int id, PointF position)
        {
            m_refTuioKinect.AddTuioCursor(id, position);
        }

        public void UpdateHandTuioKinect(int id, PointF position)
        {
            m_refTuioKinect.UpdateTuioCursor(id, position);
        }

        public void DeleteHandTuioKinect(int id)
        {
            m_refTuioKinect.DeleteTuioCursor(id);
        }

        #region IDisposable's members

        /// <summary>
        /// Dispose this instance.
        /// </summary>
        public void Dispose()
        {
            m_RunThread = false;
        }

        #endregion
    }
}
