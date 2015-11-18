using IntuiLab.Kinect.DataUserTracking;
using System.Collections.Generic;

namespace IntuiLab.Kinect.GestureRecognizer.Gestures
{
    internal class MinimizeGestureChecker : GestureChecker
    {
        protected const int ConditionTimeout = 1500;

        public MinimizeGestureChecker(UserData refUser)
            : base(new List<Condition> {

                new MinimizeCondition(refUser)

            }, ConditionTimeout) { }
    }
}
