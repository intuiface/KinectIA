            // ****************************************************************************
            // <copyright file="UserCounterChangedEventArgs.cs" company="IntuiLab">
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
    public delegate void NoUserDetectedEventHandler(object sender, NoUserDetectedEventArgs e);

    public class NoUserDetectedEventArgs : EventArgs
    {
        public NoUserDetectedEventArgs()
        {
        }
    }

    public delegate void OnePersonDetectedEventHandler(object sender, OnePersonDetectedEventArgs e);

    public class OnePersonDetectedEventArgs : EventArgs
    {
        public OnePersonDetectedEventArgs()
        {
        }
    }


}
