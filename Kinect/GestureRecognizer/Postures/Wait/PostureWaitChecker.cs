using IntuiLab.Kinect.DataUserTracking;
using System.Collections.Generic;

namespace IntuiLab.Kinect.GestureRecognizer.Postures
{
    class PostureWaitChecker : GestureChecker
    {
        protected const int ConditionTimeout = 1500;

        public PostureWaitChecker(UserData refUser)
            : base(new List<Condition> {

                new PostureWaitCondition(refUser)

            }, ConditionTimeout) { }
    }
}
