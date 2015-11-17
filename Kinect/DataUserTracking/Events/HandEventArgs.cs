            // ****************************************************************************
            // <copyright file="HandEventArgs.cs" company="IntuiLab">
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
using Microsoft.Kinect.Toolkit.Interaction;
using System.Drawing;

namespace IntuiLab.Kinect.DataUserTracking.Events
{
    public class HandActiveEventArgs : EventArgs
    {
        public int userID { get; set; }

        public InteractionHandType HandType { get; set; }

        public bool IsActive { get; set; }

        public PointF PositionOnScreen { get; set; }
    }

    public class HandMoveEventArgs : EventArgs
    {
        public int userID { get; set; }

        public InteractionHandType HandType { get; set; }

        public PointF PositionOnScreen { get; set; }

        public PointF RawPosition { get; set; }

        public bool IsGrip { get; set; }
    }

    public class HandGripStateChangeEventArgs : EventArgs
    {
        public int userID { get; set; }

        public InteractionHandType HandType { get; set; }

        public bool IsGrip { get; set; }

        public PointF RawPosition { get; set; }
    }

    public class NewUserPointingEventArgs : EventArgs
    {
        public int UserID { get; set; }
    }

    public class DeleteUserPointingEventArgs : EventArgs
    {
        public int UserID { get; set; }
    }
}
