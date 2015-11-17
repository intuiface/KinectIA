            // ****************************************************************************
            // <copyright file="GesturesEventArgs.cs" company="IntuiLab">
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

using IntuiLab.Kinect.Enums;

namespace IntuiLab.Kinect.GestureRecognizer
{
    internal abstract class GesturesEventArgs : EventArgs
    {
        public EnumGesture Gesture { get; set; }
        public EnumPosture Posture { get; set; }
    }

    internal class FailedGestureEventArgs : GesturesEventArgs
    {
        public Condition refCondition { get; set; }
    }

    internal class SuccessGestureEventArgs : GesturesEventArgs
    {
    }

    internal class BeginGestureEventArgs : GesturesEventArgs
    {
    }

    internal class EndGestureEventArgs : GesturesEventArgs
    {
    }

    internal class ProgressGestureEventArgs : GesturesEventArgs
    {
        public float Percent { get; set; }
    }
}
