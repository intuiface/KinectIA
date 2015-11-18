using IntuiLab.Kinect.DataUserTracking;
using System.Collections.Generic;

namespace IntuiLab.Kinect.GestureRecognizer.Postures
{
    internal class PostureTChecker : GestureChecker
    {
         protected const int ConditionTimeout = 1500;

         public PostureTChecker(UserData refUser)
            : base(new List<Condition> {

                new PostureTCondition(refUser)

            }, ConditionTimeout) { }
    }
}
