using IntuiLab.Kinect.DataUserTracking;
using Microsoft.Kinect;
using System.Collections.Generic;

namespace IntuiLab.Kinect.GestureRecognizer.Gestures
{
    internal class SwipeGestureChecker : GestureChecker
    {
        protected const int ConditionTimeout = 1500;

        public SwipeGestureChecker(UserData refUser, JointType refHand)
            : base(new List<Condition> {

                new SwipeCondition(refUser, refHand)

            }, ConditionTimeout) { }
    }
}
