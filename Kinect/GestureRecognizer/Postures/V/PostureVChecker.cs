using IntuiLab.Kinect.DataUserTracking;
using System.Collections.Generic;

namespace IntuiLab.Kinect.GestureRecognizer.Postures
{
    internal class PostureVChecker : GestureChecker
    {
        protected const int ConditionTimeout = 1500;

        public PostureVChecker(UserData refUser)
            : base(new List<Condition> {

                new PostureVCondition(refUser)

            }, ConditionTimeout) { }
    }
}
