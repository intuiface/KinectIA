            // ****************************************************************************
            // <copyright file="Main.cs" company="IntuiLab">
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

namespace IntuiLab.Kinect
{
    /// <summary>
    /// Global entry point for Kinect module
    /// </summary>
    public class Main
    {
        #region Attributes
        /// <summary>
        /// Counter of Start calls
        /// </summary>
        private static int m_nbInterfaceStart = 0;

        /// <summary>
        /// List of instanciated facades
        /// </summary>
        private static List<IDisposable> m_lstFacades = new List<IDisposable>(); 

        #endregion

        #region Public interface

        /// <summary>
        /// Called on IA start
        /// </summary>
        public static void Start()
        {
            Console.WriteLine("Start IntuiLab.Kinect DLL");
            m_nbInterfaceStart++;
        }

        /// <summary>
        /// Register a facade
        /// </summary>
        /// <param name="facade"></param>
        public static void RegisterFacade(IDisposable facade)
        {
            if (m_lstFacades.Contains(facade) == false)
            {
                m_lstFacades.Add(facade);
            }
        }

        /// <summary>
        /// Called on IA stop
        /// </summary>
        public static void Stop()
        {
            m_nbInterfaceStart--;
            if (m_nbInterfaceStart == 0)
            {
                // All IA have been stopped
                foreach (var facade in m_lstFacades)
                {
                    facade.Dispose();
                }
                m_lstFacades.Clear();

                if (PluginKinect.InstancePluginKinect != null)
                {
                    PluginKinect.InstancePluginKinect.Dispose();    
                }
                Console.WriteLine("IntuiLab.Kinect DLL stopped");
            }
        }

        #endregion
    }
}
