using Microsoft.Kinect.Toolkit.Interaction;

namespace IntuiLab.Kinect.DummyKinectInteraction
{
    /// <summary>
    /// This class is essential to use the Interaction of kinect SDK.
    /// It permit to get the information of the hand's without needing the WPF's control.
    /// </summary>
    class DummyInteractionClient : IInteractionClient
    {
        public InteractionInfo GetInteractionInfoAtLocation(
            int skeletonTrackingId, 
            InteractionHandType handType, 
            double x, 
            double y)
        {
            var result = new InteractionInfo();
            result.IsGripTarget = true;
            result.IsPressTarget = true;
            result.PressAttractionPointX = 0.5;
            result.PressAttractionPointY = 0.5;
            result.PressTargetControlId = 1;

            return result;
        }
    }
}
