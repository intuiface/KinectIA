using IntuiLab.Kinect.DataUserTracking;
using System.Collections.Generic;

namespace IntuiLab.Kinect.GestureRecognizer.Gestures
{
    internal class MaximizeGestureChecker : GestureChecker
    {
        protected const int ConditionTimeout = 1500;

        public MaximizeGestureChecker(UserData refUser)
            : base(new List<Condition> {

                new MaximizeCondition(refUser)

            }, ConditionTimeout) { }
    }
}
