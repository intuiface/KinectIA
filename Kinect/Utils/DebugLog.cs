            // ****************************************************************************
            // <copyright file="DebugLog.cs" company="IntuiLab">
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
