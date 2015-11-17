            // ****************************************************************************
            // <copyright file="TuioManager.cs" company="IntuiLab">
            // INTUILAB CONFIDENTIAL
			//_____________________
			// [2002] - [2015] IntuiLab SA
			// All Rights Reserved.
			// NOTICE: All information contained herein is, and remains
			// the property of IntuiLab SA. The intellectual and technical
			// concepts contained herein are proprietary to IntuiLab SA
			// and may be covered by U.S. and other country Patents, patents
			// in process, and are protected by trade secret or copyright law.
			// Dissemination of this information or reproduction of this
			// material is strictly forbidden unless prior written permission
			// is obtained from IntuiLab SA.
            // </copyright>
            // ****************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntuiLab.Kinect.TUIO.CursorKinect;
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
