            // ****************************************************************************
            // <copyright file="DataUserRecorder.cs" company="IntuiLab">
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
using System.IO;
using IntuiLab.Kinect.DataUserTracking;

namespace IntuiLab.Kinect.DataRecording
{
    internal class DataUserRecorder
    {
        /// <summary>
        /// Instance of StreamWriter
        /// </summary>
        private StreamWriter m_refStream;

        /// <summary>
        /// Number of frame
        /// </summary>
        private double m_dNumFrame;

        /// <summary>
        /// List of name parameter to recording
        /// </summary>
        private List<string> m_refNameData = new List<string>()
        {
            "NumFrame",
            "UserID",
            "Head","ShoulderCenter","HandRight","WristRight","ElbowRight","ShoulderRight","HandLeft","WristLeft","ElbowLeft","ShoulderLeft","Spine","HipCenter","HipLeft","KneeLeft","AnkleLeft","FootLeft","HipRight","KneeRight","AnkleRight","FootRight",
            "HeadOnScreen","ShoulderCenterOnScreen","HandRightOnScreen","WristRightOnScreen","ElbowRightOnScreen","ShoulderRightOnScreen","HandLeftOnScreen","WristLeftOnScreen","ElbowLeftOnScreen","ShoulderLeftOnScreen","SpineOnScreen","HipCenterOnScreen","HipLeftOnScreen","KneeLeftOnScreen","AnkleLeftOnScreen","FootLeftOnScreen","HipRightOnScreen","KneeRightOnScreen","AnkleRightOnScreen","FootRightOnScreen",
            "UserDepth",
            "IsNearest",
            "TimesTamp"
        };

        /// <summary>
        /// List of skeleton joints
        /// </summary>
        private List<Microsoft.Kinect.JointType> m_refJointType = new List<Microsoft.Kinect.JointType>()
        {
            Microsoft.Kinect.JointType.Head,
            Microsoft.Kinect.JointType.ShoulderCenter,
            Microsoft.Kinect.JointType.HandRight,
            Microsoft.Kinect.JointType.WristRight,
            Microsoft.Kinect.JointType.ElbowRight,
            Microsoft.Kinect.JointType.ShoulderRight,
            Microsoft.Kinect.JointType.HandLeft,
            Microsoft.Kinect.JointType.WristLeft,
            Microsoft.Kinect.JointType.ElbowLeft,
            Microsoft.Kinect.JointType.ShoulderLeft,
            Microsoft.Kinect.JointType.Spine,
            Microsoft.Kinect.JointType.HipCenter,
            Microsoft.Kinect.JointType.HipLeft,
            Microsoft.Kinect.JointType.KneeLeft,
            Microsoft.Kinect.JointType.AnkleLeft,
            Microsoft.Kinect.JointType.FootLeft,
            Microsoft.Kinect.JointType.HipRight,
            Microsoft.Kinect.JointType.KneeRight,
            Microsoft.Kinect.JointType.AnkleRight,
            Microsoft.Kinect.JointType.FootRight
        };

        /// <summary>
        /// Constructor
        /// </summary>
        public DataUserRecorder()
        {
            m_dNumFrame = 0;
        }

        /// <summary>
        /// Initialize file to recording
        /// </summary>
        /// <param name="sPathFile">Path of file</param>
        public void StartRecording(string sPathFile)
        {
            // Create StreamWriter
            m_refStream = new StreamWriter(sPathFile);

            string line = null;

            // Create name parameter line
            foreach (string header in m_refNameData)
            {
                line += header + ";";
            }

            // Write line in file
            m_refStream.WriteLine("{0}", line);
        }

        /// <summary>
        /// Terminates recording
        /// </summary>
        public void EndRecording()
        {
            m_refStream.Close();
            m_refStream = null;

            m_dNumFrame = 0;
        }

        /// <summary>
        /// Record a new data frame in file
        /// </summary>
        /// <param name="refUserData">User data of new frame</param>
        public void RecordData(UserData refUserData)
        {
            if (m_refStream != null)
            {
                // Create a new data line
                string line = null;

                // Frame number
                line += m_dNumFrame.ToString() + ";";

                // User ID
                line += refUserData.UserID + ";";

                // Joints position in real world
                foreach (Microsoft.Kinect.JointType joint in m_refJointType)
                {
                    line += refUserData.UserSkeleton.GetJointPosition(joint).X.ToString() + " ";
                    line += refUserData.UserSkeleton.GetJointPosition(joint).Y.ToString() + " ";
                    line += refUserData.UserSkeleton.GetJointPosition(joint).Z.ToString() + ";";
                }

                // Joints position on screen
                foreach (Microsoft.Kinect.JointType joint in m_refJointType)
                {
                    line += refUserData.UserSkeleton.GetJointPositionOnScreen(joint).X.ToString() + " ";
                    line += refUserData.UserSkeleton.GetJointPositionOnScreen(joint).Y.ToString() + ";";
                }

                // User depth
                line += refUserData.UserDepth.ToString() + ";";

                // User nearest or not
                line += refUserData.IsNearest.ToString() + ";";

                // Frame Timestamp
                line += refUserData.UserSkeleton.TimesTamp.ToString();

                // Write line in file
                m_refStream.WriteLine("{0}", line);

                m_dNumFrame++;
            }
        }
    }
}
