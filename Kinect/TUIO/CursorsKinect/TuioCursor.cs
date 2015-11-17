            // ****************************************************************************
            // <copyright file="TuioCursor.cs" company="IntuiLab">
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
using System.Drawing;
using System.Linq;
using System.Text;

namespace IntuiLab.Kinect.TUIO.CursorKinect
{
    /// <summary>
    /// TUIO cursor.
    /// 
    /// (c) 2010 by Dominik Schmidt (schmidtd@comp.lancs.ac.uk)
    /// </summary>
    public class TuioCursor
    {
        #region properties

        public int Id { get; private set; }

        public PointF Location { get; set; }

        public PointF Speed { get; set; }

        public float MotionAcceleration { get; set; }

        #endregion

        #region constructors

        public TuioCursor(int id, PointF location)
        {
            Id = id;
            Location = location;
        }

        #endregion

    }
}
