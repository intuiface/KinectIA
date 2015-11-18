using IntuiLab.Kinect.DataUserTracking;
using System.Collections.Generic;

namespace IntuiLab.Kinect.GestureRecognizer.Postures
{
    internal class PostureAChecker : GestureChecker
    {
        protected const int ConditionTimeout = 1500;

        public PostureAChecker(UserData refUser)
            : base(new List<Condition> {

                new PostureACondition(refUser)

            }, ConditionTimeout) { }
    }
}
