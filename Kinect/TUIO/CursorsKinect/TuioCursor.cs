using System.Drawing;

namespace IntuiLab.Kinect.TUIO.CursorKinect
{
    /// <summary>
    /// TUIO cursor.
    /// 
    /// (c) 2010 by Dominik Schmidt (schmidtd@comp.lancs.ac.uk)
    /// </summary>
    public class TuioCursor
    {
        #region Properties

        public int Id { get; private set; }

        public PointF Location { get; set; }

        public PointF Speed { get; set; }

        public float MotionAcceleration { get; set; }

        #endregion

        #region Constructor

        public TuioCursor(int id, PointF location)
        {
            Id = id;
            Location = location;
        }

        #endregion
    }
}
