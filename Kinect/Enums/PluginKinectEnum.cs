namespace IntuiLab.Kinect.Enums
{
    #region KinectModuleState
    /// <summary>
    /// Indicate the kinect module state
    /// </summary>
    public enum EnumKinectModuleState
    {
        KINECT_MODULE_INITIALIZED,
        KINECT_MODULE_ERROR,
        KINECT_MODULE_RUNNING,
        KINECT_MODULE_STOPPED,
        KINECT_MODULE_WRONG_DEVICE
    } ;
    #endregion

    #region KinectJointTrackingState
    /// <summary>
    /// Indicate different joint tracking state
    /// </summary>
    public enum EnumKinectJointTrackingState
    {
        KINECT_JOINT_TRACKED,
        KINECT_JOINT_NOT_TRACKED,
        KINECT_JOINT_INFERRED
    } ;
    #endregion

    #region KinectHandType
    /// <summary>
    /// Indicate the hand type
    /// </summary>
    public enum EnumKinectHandType
    {
        HAND_LEFT,
        HAND_RIGHT
    } ;
    #endregion

    #region KinectGestureRecognize
    /// <summary>
    /// Indicate the all differents type of gesture which can recognize
    /// </summary>
    public enum EnumKinectGestureRecognize
    {
        KINECT_RECOGNIZE_SWIPE_LEFT,
        KINECT_RECOGNIZE_SWIPE_RIGHT,
        KINECT_RECOGNIZE_WAVE,
        KINECT_RECOGNIZE_PUSH,
        KINECT_RECOGNIZE_MAXIMIZE,
        KINECT_RECOGNIZE_MINIMIZE,
        KINECT_RECOGNIZE_T,
        KINECT_RECOGNIZE_V,
        KINECT_RECOGNIZE_A,
        KINECT_RECOGNIZE_U,
        KINECT_RECOGNIZE_WAIT,
        KINECT_RECOGNIZE_HOME,
        KINECT_RECOGNIZE_STAY,
        KINECT_RECOGNIZE_NONE
    };
    #endregion

    #region EnumGesture
    /// <summary>
    /// Indicate differents gesture which can recognize
    /// </summary>
    public enum EnumGesture
    {
        GESTURE_SWIPE_LEFT,
        GESTURE_SWIPE_RIGHT,
        GESTURE_WAVE,
        GESTURE_PUSH,
        GESTURE_MAXIMIZE,
        GESTURE_MINIMIZE,
        GESTURE_NONE
    }
    #endregion

    #region EnumPosture
    /// <summary>
    /// Indicate differents posture which can recognize
    /// </summary>
    public enum EnumPosture
    {
        POSTURE_T,
        POSTURE_V,
        POSTURE_A,
        POSTURE_U,
        POSTURE_WAIT,
        POSTURE_HOME,
        POSTURE_STAY,
        POSTURE_NONE
    }
    #endregion

    #region KinectDirectionGesture
    /// <summary>
    /// Indicate differents gesture direction
    /// </summary>
    public enum EnumKinectDirectionGesture
    {
        KINECT_DIRECTION_FORWARD,
        KINECT_DIRECTION_BACKWARD,
        KINECT_DIRECTION_UPWARD,
        KINECT_DIRECTION_DOWNWARD,
        KINECT_DIRECTION_LEFT,
        KINECT_DIRECTION_RIGHT,
        KINECT_DIRECTION_NONE
    };
    #endregion
}
