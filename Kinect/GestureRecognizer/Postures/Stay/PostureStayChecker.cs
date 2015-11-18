using IntuiLab.Kinect.DataUserTracking;
using System.Collections.Generic;

namespace IntuiLab.Kinect.GestureRecognizer.Postures
{
    class PostureStayChecker : GestureChecker
    {
        protected const int ConditionTimeout = 1500;

        public PostureStayChecker(UserData refUser)
            : base(new List<Condition> {

                new PostureStayCondition(refUser)

            }, ConditionTimeout) { }
    }
}
