            // ****************************************************************************
            // <copyright file="PluginKinectEnum.cs" company="IntuiLab">
            // INTUILAB CONFIDENTIAL
			//_____________________
			// [2002] - [2015] IntuiLab SA
			// All Rights Reserved.
			// NOTICE: All information contained herein is, and remains
			// the property of IntuiLab SA. The intellectual and technical
			// concepts contained herein are proprietary to IntuiLab SA
			// and may be covered by U.S. and other country Patents, patents
			// in process, and are protected by trade secret or copyright law.
			// Dissemination of this information or reproduction of this
			// material is strictly forbidden unless prior written permission
			// is obtained from IntuiLab SA.
            // </copyright>
            // ****************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    //#region KinectMode
    ///// <summary>
    ///// Indicate differents kinect mode
    ///// KINECT_MODE_GESTURE => Kinect enable gesture and posture
    ///// KINECT_MODE_POINTING => Kinect enable pointing for direct manipulation
    ///// </summary>
    //public enum KinectMode
    //{
    //    KINECT_MODE_GESTURE,
    //    KINECT_MODE_POINTING
    //}

    //#endregion
}
