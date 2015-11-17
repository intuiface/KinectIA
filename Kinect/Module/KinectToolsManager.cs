            // ****************************************************************************
            // <copyright file="KinectToolsManager.cs" company="IntuiLab">
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
using System.Windows.Media.Media3D;

using IntuiLab.Kinect.DataRecording;
using IntuiLab.Kinect.Exceptions;

namespace IntuiLab.Kinect
{
    /// <summary>
    /// This class exist only for realize tests.
    /// Provides the methods permit to record or replay.
    /// </summary>
    public partial class KinectModule
    {
        #region Fields

        /// <summary>
        /// Indicate if the data user must be recording
        /// </summary>
        private bool m_bRecData;

        /// <summary>
        /// Data user recorder manager
        /// </summary>
        private DataUserRecorder m_refDataUserRecorder;

        /// <summary>
        /// Data user replayer manager
        /// </summary>
        private DataUserReplay m_refDataUserReplay;

        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize the DataUserRecorder
        /// </summary>
        public void InitializeDataRecording()
        {
            if (m_refDataUserRecorder != null)
            {
                StopDataRecording();
                m_refDataUserRecorder = null;
            }
            // Create the data user recorder
            m_refDataUserRecorder = new DataUserRecorder();
        }

        /// <summary>
        /// Start the recording
        /// </summary>
        /// <param name="sPathFileSave">File where save the data</param>
        public void StartDataRecording(string sPathFileSave)
        {
            if (m_refDataUserRecorder != null)
            {
                m_bRecData = true;
                m_refDataUserRecorder.StartRecording(sPathFileSave);
            }
        }

        /// <summary>
        /// Stop the recording
        /// </summary>
        public void StopDataRecording()
        {
            if (m_refDataUserRecorder != null)
            {
                m_bRecData = false;
                m_refDataUserRecorder.EndRecording();
            }
        }

        /// <summary>
        /// Initialize the DataUserReplay
        /// </summary>
        public void InitializeDataReplay()
        {
            // Create the data user replay
            m_refDataUserReplay = new DataUserReplay();
        }

        /// <summary>
        /// Start the Replay
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="jointPosition">Joints position of first frame</param>
        /// <param name="depth">User depth of first frame</param>
        /// <param name="timesTamp">TimesTamps of first frame</param>
        public void LaunchReplay(int userID, Dictionary<string, Point3D> jointPosition, double depth, long timesTamp)
        {
            if (m_refDataUserReplay != null)
            {
                m_refDataUserReplay.CreateUser(userID, jointPosition, depth, timesTamp);
            }
            else
            {
                DisplayDebugLog("Error plugin Kinect : Replay mode not activate", true);
                throw new KinectException("Replay mode not activate");
            }
        }

        /// <summary>
        /// Permit to receive a new frame for the replay
        /// </summary>
        /// <param name="jointPosition">Joints position of new frame</param>
        /// <param name="depth">User depth of new frame</param>
        /// <param name="timesTamp">TimesTamp of new frame</param>
        public void ReceiveNewFrameReplay(Dictionary<string, Point3D> jointPosition, double depth, long timesTamp)
        {
            if (m_refDataUserReplay != null)
            {
                m_refDataUserReplay.ReceiveNewFrame(jointPosition, depth, timesTamp);
            }
            else
            {
                DisplayDebugLog("Error plugin Kinect : Replay mode not activate", true);
                throw new KinectException("Replay mode not activate");
            }
        }

        /// <summary>
        /// Stop the Replay
        /// </summary>
        public void EndReplayMode()
        {
            if (m_refDataUserReplay != null)
            {
                m_refDataUserReplay.EndReplay();
            }
            else
            {
                DisplayDebugLog("Error plugin Kinect : Replay mode not activate", true);
                throw new KinectException("Replay mode not activate");
            }
        }
        #endregion
    }
}
