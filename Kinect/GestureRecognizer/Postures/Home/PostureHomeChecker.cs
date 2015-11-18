using IntuiLab.Kinect.DataUserTracking;
using System.Collections.Generic;

namespace IntuiLab.Kinect.GestureRecognizer.Postures
{
    class PostureHomeChecker : GestureChecker
    {
        protected const int ConditionTimeout = 1500;

        public PostureHomeChecker(UserData refUser)
            : base(new List<Condition> {

                new PostureHomeCondition(refUser)

            }, ConditionTimeout) { }
    }
}
