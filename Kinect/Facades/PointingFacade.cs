using IntuiLab.Kinect.DataUserTracking.Events;
using IntuiLab.Kinect.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntuiLab.Kinect
{
    public class PointingFacade : IDisposable
    {
        #region Fields

        /// <summary>
        /// Dictionary to manage the feedback hand's ID in function to the user
        /// Diactionary <UserID,List<Hand's feedback ID>> 
        /// In the list, the index 0 correspond to the hand left and the index 1 correspond to the hand right
        /// </summary>
        private Dictionary<int,List<int>> m_refIdHandFeedback;

        /// <summary>
        /// Event compression.
        /// Store for each event the last date to prevent too many events
        /// </summary>
        private Dictionary<int, DateTime> m_refLastEvents = new Dictionary<int, DateTime>();

        #endregion

        #region Properties

        #region PointingModeEnabled

        /// <summary>
        /// Indicates if the pointing mode is enabled or not
        /// </summary>
        public bool PointingModeEnabled
        {
            get
            {
                return PropertiesPluginKinect.Instance.KinectPointingModeEnabled;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.KinectPointingModeEnabled != value)
                {
                    PropertiesPluginKinect.Instance.KinectPointingModeEnabled = value;
                }
            }
        }
        #endregion

        #region SpaceBetweenHands
        /// <summary>
        /// Permit to parametrize the minimal space betwwen the user's hands
        /// </summary>
        public float SpaceBetweenHands
        {
            get
            {
                return PropertiesPluginKinect.Instance.PointingSpaceBetweenHands;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.PointingSpaceBetweenHands != value)
                {
                    PropertiesPluginKinect.Instance.PointingSpaceBetweenHands = value;
                }
            }
        }
        #endregion

        #region HandsAmplitude
        /// <summary>
        /// Permit to parametrize the amplitude of the user's hands
        /// </summary>
        public float HandsAmplitude
        {
            get
            {
                return PropertiesPluginKinect.Instance.PointingHandsAmplitude;
            }
            set
            {
                if (PropertiesPluginKinect.Instance.PointingHandsAmplitude != value)
                {
                    PropertiesPluginKinect.Instance.PointingHandsAmplitude = value;
                }
            }
        }
        #endregion

        #endregion

        #region Events

        public event NewHandActiveEventHandler NewHandActive;

        protected void RaiseNewHandActive(int handID, int posX, int posY)
        {
            if (NewHandActive != null)
            {
                NewHandActive(this, new NewHandActiveEventArgs(handID, posX, posY));
            }
        }

        public event UserHandLostEventHandler UserHandLost;

        protected void RaiseUserHandLost(int handID)
        {
            if (UserHandLost != null)
            {
                UserHandLost(this, new UserHandLostEventArgs(handID));
            }
        }

        private void OnNewHandActive(object sender, HandActiveEventArgs e)
        {
            // Precondition checking
            if (m_refIdHandFeedback.ContainsKey(e.userID) == false)
            {
                // Unknown user
                return;
            }

            // Raise associated event
            if (e.HandType == Microsoft.Kinect.Toolkit.Interaction.InteractionHandType.Left)
            {
                var handId = m_refIdHandFeedback[e.userID].ElementAt(0);
                if (e.IsActive)
                {
                    RaiseNewHandActive(handId, (int)(e.PositionOnScreen.X), (int)(e.PositionOnScreen.Y));
                }
                else
                {
                    RaiseUserHandLost(handId);
                }
            }
            else if (e.HandType == Microsoft.Kinect.Toolkit.Interaction.InteractionHandType.Right)
            {
                var handId = m_refIdHandFeedback[e.userID].ElementAt(1);
                if (e.IsActive)
                {
                    RaiseNewHandActive(handId, (int)(e.PositionOnScreen.X), (int)(e.PositionOnScreen.Y));
                }
                else
                {
                    RaiseUserHandLost(handId);
                }
            }
        }

        public event UserHandMoveEventHandler UserHandMove;

        protected void RaiseUserHandMove(int handID, int posX, int posY, bool isGrip)
        {
            if (UserHandMove != null)
            {
                // Event compression
                if (ShouldSendEvent(handID))
                {
                    UserHandMove(this, new UserHandActiveEventArgs(handID, posX, posY, isGrip));                   
                }
            }
        }

        private void OnUserHandMove(object sender, HandMoveEventArgs e)
        {
            // Precondition checking
            if (m_refIdHandFeedback.ContainsKey(e.userID) == false)
            {
                // Unknown user
                return;
            }

            // Raise associated event
            if (e.HandType == Microsoft.Kinect.Toolkit.Interaction.InteractionHandType.Left)
            {
                var handId = m_refIdHandFeedback[e.userID].ElementAt(0);
                RaiseUserHandMove(handId, (int)(e.PositionOnScreen.X), (int)(e.PositionOnScreen.Y), e.IsGrip);
            }
            else if (e.HandType == Microsoft.Kinect.Toolkit.Interaction.InteractionHandType.Right)
            {
                var handId = m_refIdHandFeedback[e.userID].ElementAt(1);
                RaiseUserHandMove(handId, (int)(e.PositionOnScreen.X), (int)(e.PositionOnScreen.Y), e.IsGrip);
            }
        }


        public event UserHandGripEventHandler UserHandGrip;
        protected void RaiseUserHandGrip(int handID)
        {
            if (UserHandGrip != null)
            {
                UserHandGrip(this, new UserHandGripEventArgs(handID));
            }
        }

        public event UserHandGripReleasedEventHandler UserHandGripReleased;
        protected void RaiseUserHandGripReleased(int handID)
        {
            if (UserHandGripReleased != null)
            {
                UserHandGripReleased(this, new UserHandGripReleasedEventArgs(handID));
            }
        }

        private void OnUserHandGripStateChnged(object sender, HandGripStateChangeEventArgs e)
        {
            // Precondition checking
            if (m_refIdHandFeedback.ContainsKey(e.userID) == false)
            {
                return;
            }

            if (e.HandType == Microsoft.Kinect.Toolkit.Interaction.InteractionHandType.Left)
            {
                var handId = m_refIdHandFeedback[e.userID].ElementAt(0);
                if (e.IsGrip)
                {
                    RaiseUserHandGrip(handId);
                }
                else
                {
                    RaiseUserHandGripReleased(handId);
                }
            }
            else if (e.HandType == Microsoft.Kinect.Toolkit.Interaction.InteractionHandType.Right)
            {
                var handId = m_refIdHandFeedback[e.userID].ElementAt(1);
                if (e.IsGrip)
                {
                    RaiseUserHandGrip(handId);
                }
                else
                {
                    RaiseUserHandGripReleased(handId);
                }
            }
        }

        private void OnNewUserPointing(object sender, NewUserPointingEventArgs e)
        {
            List<int> handsID = new List<int>();

            if (m_refIdHandFeedback.Count == 0)
            {
                handsID.Add(0);
                handsID.Add(1);
                m_refIdHandFeedback.Add(e.UserID, handsID);
            }
            else
            {
                if (!m_refIdHandFeedback.ContainsKey(e.UserID))
                {
                    if (!m_refIdHandFeedback.ElementAt(0).Value.Contains(0))
                    {
                        handsID.Add(0);
                        handsID.Add(1);
                    }
                    else
                    {
                        handsID.Add(2);
                        handsID.Add(3);
                    }
                    m_refIdHandFeedback.Add(e.UserID, handsID);
                }
            }
            
        }

        private void OnDeleteUserPointing(object sender, DeleteUserPointingEventArgs e)
        {
            if (m_refIdHandFeedback.ContainsKey(e.UserID))
            {
                RaiseUserHandLost(m_refIdHandFeedback[e.UserID].ElementAt(0));
                RaiseUserHandLost(m_refIdHandFeedback[e.UserID].ElementAt(1));
                m_refIdHandFeedback.Remove(e.UserID);
            }
        }

        /// <summary>
        /// Should we compress this event ?
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns>true if we should send the event, false otherwise</returns>
        private bool ShouldSendEvent(int eventId)
        {
            var now = DateTime.Now;

            // Event compression
            if (m_refLastEvents.ContainsKey(eventId))
            {
                var lastEvent = m_refLastEvents[eventId];
                var elapsed = (now - lastEvent).TotalMilliseconds;
                if (elapsed < 100) // ms
                {
                    // No more than 10 events / seconds
                    return false;
                }
            }

            // Update last event date.
            m_refLastEvents[eventId] = now;
            return true;
        }

        #endregion

        #region Constructor
        public PointingFacade()
        {
            if (PluginKinect.InstancePluginKinect == null)
            {
                PluginKinect plugin = new PluginKinect();
            }

            m_refIdHandFeedback = new Dictionary<int,List<int>>();
            PointingModeEnabled = true;

            ConnectHandler();
            Main.RegisterFacade(this);
        }
        #endregion

        #region Public Methods
        public void ConnectHandler()
        {
            if (PluginKinect.InstancePluginKinect != null)
            {
                PluginKinect.InstancePluginKinect.Kinect.NewHandActive += OnNewHandActive;
                PluginKinect.InstancePluginKinect.Kinect.UserHandMove += OnUserHandMove;
                PluginKinect.InstancePluginKinect.Kinect.UserHandGripStateChanged += OnUserHandGripStateChnged;
                PluginKinect.InstancePluginKinect.Kinect.NewUserPointing += OnNewUserPointing;
                PluginKinect.InstancePluginKinect.Kinect.DeleteUserPointing += OnDeleteUserPointing;
            }
        }

        public void ResetHandler()
        {
            if (PluginKinect.InstancePluginKinect != null)
            {
                PluginKinect.InstancePluginKinect.Kinect.NewHandActive -= OnNewHandActive;
                PluginKinect.InstancePluginKinect.Kinect.UserHandMove -= OnUserHandMove;
                PluginKinect.InstancePluginKinect.Kinect.UserHandGripStateChanged -= OnUserHandGripStateChnged;
                PluginKinect.InstancePluginKinect.Kinect.NewUserPointing -= OnNewUserPointing;
                PluginKinect.InstancePluginKinect.Kinect.DeleteUserPointing -= OnDeleteUserPointing;
            }
        }

        #endregion

        #region IDisposable's members

        /// <summary>
        /// Dispose this instance.
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                ResetHandler();
            }
        }
        #endregion
    }
}
