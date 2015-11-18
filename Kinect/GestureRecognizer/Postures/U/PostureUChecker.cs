using IntuiLab.Kinect.DataUserTracking;
using System.Collections.Generic;

namespace IntuiLab.Kinect.GestureRecognizer.Postures
{
    class PostureUChecker : GestureChecker
    {
        protected const int ConditionTimeout = 1500;

        public PostureUChecker(UserData refUser)
            : base(new List<Condition> {

                new PostureUCondition(refUser)

            }, ConditionTimeout) { }
    }
}
