using IntuiLab.Kinect.DataUserTracking;
using Microsoft.Kinect;
using System.Collections.Generic;

namespace IntuiLab.Kinect.GestureRecognizer.Gestures
{
    internal class WaveGestureChecker : GestureChecker
    {
        protected const int ConditionTimeout = 2500;

        public WaveGestureChecker(UserData refUser, JointType hand)
            : base(new List<Condition>
            {
                new WaveLeftCondition(refUser, hand),
                new WaveRightCondition(refUser, hand)                
            }, ConditionTimeout) { }
    }
}
