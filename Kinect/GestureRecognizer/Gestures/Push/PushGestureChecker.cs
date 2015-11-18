using IntuiLab.Kinect.DataUserTracking;
using Microsoft.Kinect;
using System.Collections.Generic;

namespace IntuiLab.Kinect.GestureRecognizer.Gestures
{
    internal class PushGestureChecker : GestureChecker
    {
        protected const int ConditionTimeout = 1500;

        public PushGestureChecker(UserData refUser, JointType refHand)
            : base(new List<Condition> {

                new PushCondition(refUser, refHand)

            }, ConditionTimeout) { }
    }
}
