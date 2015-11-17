            // ****************************************************************************
            // <copyright file="PluginKinect.cs" company="IntuiLab">
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
using System.Timers;
using System.Threading;
using System.ComponentModel;

using IntuiLab.Kinect.Events;
using IntuiLab.Kinect.Exceptions;
using System.Net;
using System.IO;
using System.Xml;

namespace IntuiLab.Kinect
{
    /// <summary>
    /// Facade class to manage the Kinect global settings
    ///   - Enabled/Disabled streams (Color, Depth, Skeleton)
    ///   - Minimal distance user/sensor to lock kinect (in meter)
    ///   - Kinect sensor elevation (in degrees between -27° and 27°)
    /// </summary>
    public class PluginKinect : IDisposable
    {
        #region Fields 

        private Thread m_refThreadWebServiceRequest;

        #endregion

        #region Properties

        #region Kinect
        /// <summary>
        /// Module kinect
        /// </summary>
        private KinectModule m_refKinectModule;
        public KinectModule Kinect 
        {
            get
            {
                return m_refKinectModule;
            }
        }
        #endregion

        #region InstancePluginKinect
        /// <summary>
        /// Instance of Plugin Kinect (for Singleton)
        /// </summary>
        private static PluginKinect m_InstancePluginKinect = null;
        public static PluginKinect InstancePluginKinect
        {
            get
            {
                return m_InstancePluginKinect;
            }
        }
        #endregion
                
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// The Kinect module is automatically started
        /// </summary>
        public PluginKinect() 
        {
            if (PluginKinect.InstancePluginKinect == null)
            {
                // Initialize the plugin kinect and started this
                InitializePluginKinect(true);
            }
        }

        /// <summary>
        /// Constructor with parameters
        /// You choose if the kinect module is started or not
        /// </summary>
        /// <param name="mode"></param>
        public PluginKinect(bool start)
        {
            // Initialize the plugin kinect
            InitializePluginKinect(start);
        }

        /// <summary>
        /// Initialize the plugin kinect and start this if necessary
        /// </summary>
        /// <param name="start">Indicate if the kinect must be started</param>
        private void InitializePluginKinect(bool start)
        {
            if (m_InstancePluginKinect == null)
            {
                m_refThreadWebServiceRequest = new Thread(LaunchWebServiceRequest);
                m_refThreadWebServiceRequest.Start();

                // Save PluginKinect instance
                m_InstancePluginKinect = this;

                // Create the Kinect Module
                m_refKinectModule = new KinectModule();

                if (start)
                {
                    // Start the Kinect Module
                    StartModuleKinect();
                }
            }
            else
            {
                throw new KinectException("Kinect already instantiate");
            }
        }
        #endregion

        #region Public services

        #region Start/Stop Module kinect

        /// <summary>
        /// Run the Kinect Module
        /// </summary>
        public void StartModuleKinect()
        {
            if (!PluginKinect.InstancePluginKinect.Kinect.IsRunning)
            {
                PluginKinect.InstancePluginKinect.Kinect.Start();
            }
        }

        /// <summary>
        /// Stop the Kinect Module
        /// </summary>
        public void StopModuleKinect()
        {
            if (PluginKinect.InstancePluginKinect.Kinect.IsRunning)
            {
                PluginKinect.InstancePluginKinect.Kinect.Stop();
            }
        }
        #endregion

        #region Private Methods

        private void LaunchWebServiceRequest()
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

                Stream data = client.OpenRead("http://localhost:8000/intuifacepresentationplayer/getip3");
                StreamReader reader = new StreamReader(data);
                string resultRequest = reader.ReadToEnd();

                reader.Close();

                XmlDocument document = new XmlDocument();
                document.LoadXml(resultRequest);

                XmlNode root = document.DocumentElement;                

                if (root.Attributes.Count > 0)
                {
                    for (int i = 0; i < root.Attributes.Count; i++)
                    {
                        if (root.Attributes.Item(i).Name == "Width")
                        {
                            Console.WriteLine("Width = " + root.Attributes.Item(i).Value);
                            PropertiesPluginKinect.Instance.ExperienceIntuiFaceWidth = Convert.ToInt32(root.Attributes.Item(i).Value);
                        }

                        else if (root.Attributes.Item(i).Name == "Height")
                        {
                            Console.WriteLine("Height = " + root.Attributes.Item(i).Value);
                            PropertiesPluginKinect.Instance.ExperienceIntuifaceHeight = Convert.ToInt32(root.Attributes.Item(i).Value);
                        }
                    }
                }
            }
            catch(WebException ex)
            {
                Console.WriteLine("Error Web Service Request " + ex.Message );
            }
        }

        #endregion

        #region DataRecorder Manager

        public void StartRecord(string sFile)
        {
            m_refKinectModule.StartDataRecording(sFile);
        }

        public void StopRecord()
        {
            m_refKinectModule.StopDataRecording();
        }
        #endregion

        #endregion

        #region IDisposable's members

        /// <summary>
        /// Dispose this instance.
        /// </summary>
        public void Dispose()
        {
            if (m_InstancePluginKinect != null)
            {
                lock (this)
                {
                    if (PluginKinect.InstancePluginKinect.Kinect != null)
                    {
                        // Stop the Kinect module
                        if (PluginKinect.InstancePluginKinect.Kinect.IsRunning)
                        {
                            StopModuleKinect();
                        }

                        // Destroy the Kinect module
                        PluginKinect.InstancePluginKinect.Kinect.Dispose();
                        m_refKinectModule = null;
                    }

                    if (PropertiesPluginKinect.Instance != null)
                    {
                        PropertiesPluginKinect.Instance.Dispose();
                    }

                    m_InstancePluginKinect = null;
                }
            }
        }
        #endregion
    }
}
