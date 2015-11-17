            // ****************************************************************************
            // <copyright file="CurrentMillis.cs" company="IntuiLab">
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
