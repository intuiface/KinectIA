using System;
using System.Collections.Generic;

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
