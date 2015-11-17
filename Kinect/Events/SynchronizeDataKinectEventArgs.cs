            // ****************************************************************************
            // <copyright file="SynchronizeDataKinectEventArgs.cs" company="IntuiLab">
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

namespace IntuiLab.Kinect.Events
{
    public class SynchronizeElevationAngleEventArgs : EventArgs
    {
        public int Elevation { get; set; }
    }

    public class SynchronizeColorStreamActivationEventArgs : EventArgs
    {
        public bool Enable { get; set; }
    }

    public class SynchronizeDepthStreamActivationEventArgs : EventArgs
    {
        public bool Enable { get; set; }
    }

    public class SynchronizeGesturesStateEventArgs : EventArgs
    {
        public string GestureName { get; set; }
    }

    public class SynchronizeKinectModeEventArgs : EventArgs
    {
        public bool PointingMode { get; set; }
    }
}
