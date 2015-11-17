            // ****************************************************************************
            // <copyright file="GestureDetectedEventArgs.cs" company="IntuiLab">
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
    #region GestureDetected
    public abstract class GestureDetectedEventArgs : EventArgs
    {
        public GestureDetectedEventArgs()
        {
        }
    }

    #region GestureSwipeLeftDetectedEventArgs
    public delegate void GestureSwipeLeftDetectedEventHandler(object sender, GestureSwipeLeftDetectedEventArgs e);

    public class GestureSwipeLeftDetectedEventArgs : GestureDetectedEventArgs
    {
        public GestureSwipeLeftDetectedEventArgs()
        {
        }
    }

    #endregion

    #region GestureSwipeRightDetectedEventArgs
    public delegate void GestureSwipeRightDetectedEventHandler(object sender, GestureSwipeRightDetectedEventArgs e);

    public class GestureSwipeRightDetectedEventArgs : GestureDetectedEventArgs
    {
        public GestureSwipeRightDetectedEventArgs()
        {
        }
    }
    #endregion

    #region GestureWaveDetectedEventArgs
    public delegate void GestureWaveDetectedEventHandler(object sender, GestureWaveDetectedEventArgs e);

    public class GestureWaveDetectedEventArgs : GestureDetectedEventArgs
    {
        public GestureWaveDetectedEventArgs()
        {
        }
    }
    #endregion

    #region GestureGripDetectedEventArgs
    public delegate void GestureGripDetectedEventHandler(object sender, GestureGripDetectedEventArgs e);

    public class GestureGripDetectedEventArgs : GestureDetectedEventArgs
    {
        public GestureGripDetectedEventArgs()
        {
        }
    }
    #endregion

    #region GesturePushDetectedEventArgs
    public delegate void GesturePushDetectedEventHandler(object sender, GesturePushDetectedEventArgs e);

    public class GesturePushDetectedEventArgs : GestureDetectedEventArgs
    {
        public GesturePushDetectedEventArgs()
        {
        }
    }
    #endregion

    #region GestureMaximizeDetectedEventArgs
    public delegate void GestureMaximizeDetectedEventHandler(object sender, GestureMaximizeDetectedEventArgs e);

    public class GestureMaximizeDetectedEventArgs : GestureDetectedEventArgs
    {
        public GestureMaximizeDetectedEventArgs()
        {
        }
    }
    #endregion

    #region GestureMinimizeDetectedEventArgs
    public delegate void GestureMinimizeDetectedEventHandler(object sender, GestureMinimizeDetectedEventArgs e);

    public class GestureMinimizeDetectedEventArgs : GestureDetectedEventArgs
    {
        public GestureMinimizeDetectedEventArgs()
        {
        }
    }
    #endregion

    #region GestureADetectedEventArgs
    public delegate void GestureADetectedEventHandler(object sender, GestureADetectedEventArgs e);

    public class GestureADetectedEventArgs : GestureDetectedEventArgs
    {
        public GestureADetectedEventArgs()
        {
        }
    }
    #endregion

    #region GestureHomeDetectedEventArgs
    public delegate void GestureHomeDetectedEventHandler(object sender, GestureHomeDetectedEventArgs e);

    public class GestureHomeDetectedEventArgs : GestureDetectedEventArgs
    {
        public GestureHomeDetectedEventArgs()
        {
        }
    }
    #endregion
    
    #region GestureStayDetectedEventArgs
    public delegate void GestureStayDetectedEventHandler(object sender, GestureStayDetectedEventArgs e);

    public class GestureStayDetectedEventArgs : GestureDetectedEventArgs
    {
        public GestureStayDetectedEventArgs()
        {
        }
    }
    #endregion

    #region GestureTDetectedEventArgs
    public delegate void GestureTDetectedEventHandler(object sender, GestureTDetectedEventArgs e);

    public class GestureTDetectedEventArgs : GestureDetectedEventArgs
    {
        public GestureTDetectedEventArgs()
        {
        }
    }
    #endregion

    #region GestureUDetectedEventArgs
    public delegate void GestureUDetectedEventHandler(object sender, GestureUDetectedEventArgs e);

    public class GestureUDetectedEventArgs : GestureDetectedEventArgs
    {
        public GestureUDetectedEventArgs()
        {
        }
    }
    #endregion

    #region GestureVDetectedEventArgs
    public delegate void GestureVDetectedEventHandler(object sender, GestureVDetectedEventArgs e);

    public class GestureVDetectedEventArgs : GestureDetectedEventArgs
    {
        public GestureVDetectedEventArgs()
        {
        }
    }
    #endregion

    #region GestureWaitDetectedEventArgs
    public delegate void GestureWaitDetectedEventHandler(object sender, GestureWaitDetectedEventArgs e);

    public class GestureWaitDetectedEventArgs : GestureDetectedEventArgs
    {
        public GestureWaitDetectedEventArgs()
        {
        }
    }
    #endregion

    #endregion

    #region GestureProgress

    public abstract class GestureProgressEventArgs : EventArgs
    {
        private float m_refPercent;
        public float Percent
        {
            get
            {
                return m_refPercent;
            }
        }

        public GestureProgressEventArgs(float percent)
        {
            m_refPercent = percent;
        }
    }

    #region GestureAProgressEventArgs
    public delegate void GestureAProgressEventHandler(object sender, GestureAProgressEventArgs e);

    public class GestureAProgressEventArgs : GestureProgressEventArgs
    {
        public GestureAProgressEventArgs(float percent)
            : base (percent)
        {
        }
    }
    #endregion

    #region GestureHomeProgressEventArgs
    public delegate void GestureHomeProgressEventHandler(object sender, GestureHomeProgressEventArgs e);

    public class GestureHomeProgressEventArgs : GestureProgressEventArgs
    {
        public GestureHomeProgressEventArgs(float percent)
            : base(percent)
        {
        }
    }
    #endregion

    #region GestureStayProgressEventArgs
    public delegate void GestureStayProgressEventHandler(object sender, GestureStayProgressEventArgs e);

    public class GestureStayProgressEventArgs : GestureProgressEventArgs
    {
        public GestureStayProgressEventArgs(float percent)
            : base(percent)
        {
        }
    }
    #endregion

    #region GestureTProgressEventArgs
    public delegate void GestureTProgressEventHandler(object sender, GestureTProgressEventArgs e);

    public class GestureTProgressEventArgs : GestureProgressEventArgs
    {
        public GestureTProgressEventArgs(float percent)
            : base(percent)
        {
        }
    }
    #endregion

    #region GestureUProgressEventArgs
    public delegate void GestureUProgressEventHandler(object sender, GestureUProgressEventArgs e);

    public class GestureUProgressEventArgs : GestureProgressEventArgs
    {
        public GestureUProgressEventArgs(float percent)
            : base(percent)
        {
        }
    }
    #endregion

    #region GestureVProgressEventArgs
    public delegate void GestureVProgressEventHandler(object sender, GestureVProgressEventArgs e);

    public class GestureVProgressEventArgs : GestureProgressEventArgs
    {
        public GestureVProgressEventArgs(float percent)
            : base(percent)
        {
        }
    }
    #endregion

    #region GestureWaitProgressEventArgs
    public delegate void GestureWaitProgressEventHandler(object sender, GestureWaitProgressEventArgs e);

    public class GestureWaitProgressEventArgs : GestureProgressEventArgs
    {
        public GestureWaitProgressEventArgs(float percent)
            : base(percent)
        {
        }
    }
    #endregion

    #endregion

    #region GestureLost

    public abstract class GestureLostEventArgs : EventArgs
    {
        public GestureLostEventArgs()
        {
        }
    }

    #region GestureALostEventArgs
    public delegate void GestureALostEventHandler(object sender, GestureALostEventArgs e);

    public class GestureALostEventArgs : GestureLostEventArgs
    {
        public GestureALostEventArgs()
        {
        }
    }
    #endregion

    #region GestureHomeLostEventArgs
    public delegate void GestureHomeLostEventHandler(object sender, GestureHomeLostEventArgs e);

    public class GestureHomeLostEventArgs : GestureLostEventArgs
    {
        public GestureHomeLostEventArgs()
        {
        }
    }
    #endregion

    #region GestureStayLostEventArgs
    public delegate void GestureStayLostEventHandler(object sender, GestureStayLostEventArgs e);

    public class GestureStayLostEventArgs : GestureLostEventArgs
    {
        public GestureStayLostEventArgs()
        {
        }
    }
    #endregion

    #region GestureTLostEventArgs
    public delegate void GestureTLostEventHandler(object sender, GestureTLostEventArgs e);

    public class GestureTLostEventArgs : GestureLostEventArgs
    {
        public GestureTLostEventArgs()
        {
        }
    }
    #endregion

    #region GestureULostEventArgs
    public delegate void GestureULostEventHandler(object sender, GestureULostEventArgs e);

    public class GestureULostEventArgs : GestureLostEventArgs
    {
        public GestureULostEventArgs()
        {
        }
    }
    #endregion

    #region GestureVLostEventArgs
    public delegate void GestureVLostEventHandler(object sender, GestureVLostEventArgs e);

    public class GestureVLostEventArgs : GestureLostEventArgs
    {
        public GestureVLostEventArgs()
        {
        }
    }
    #endregion

    #region GestureWaitLostEventArgs
    public delegate void GestureWaitLostEventHandler(object sender, GestureWaitLostEventArgs e);

    public class GestureWaitLostEventArgs : GestureLostEventArgs
    {
        public GestureWaitLostEventArgs()
        {
        }
    }
    #endregion

    #endregion
}
