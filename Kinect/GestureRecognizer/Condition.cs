            // ****************************************************************************
            // <copyright file="Condition.cs" company="IntuiLab">
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

using Microsoft.Kinect;

using IntuiLab.Kinect.DataUserTracking;

namespace IntuiLab.Kinect.GestureRecognizer
{
    internal abstract class Condition
    {
       /// <summary>
        /// User who has to fullfill this condition</summary>
        protected UserData m_User;

        /// <summary>
        /// Create a gesture part, whose fullfillment is checked on User refUser.</summary>
        /// <param name="refUser">
        /// User who has to fullfill this condition</param>
        public Condition(UserData refUser) 
        { 
            m_User = refUser;
        }

        /// <summary>
        /// Begin checking new skeletons.
        /// Save performance and enable only gestures you really need to check.</summary>
        public void Enable()
        {
            m_User.NewSkeletonData += ExtendedCheck;
        }

        /// <summary>
        /// Dont react on new skeletons (anymore).
        /// Use this to save performance after a gesture isn't used anymore.</summary>
        public void Disable()
        {
            m_User.NewSkeletonData -= ExtendedCheck;
        }

        /// <summary>
        /// Implement this to check for the fullfillment of a gesture part.
        /// This method is called every time when a person gets a new skeleton.
        /// It is good practice to fire success or fail in the implementation
        /// of this method. For the checking itself you can use information
        /// about the persons skeletons.</summary>
        /// <param name="src">
        /// The object which fired the event. (This is probably the Device class.)</param>
        /// <param name="e">
        /// NewSkeletonEventArgs contains the person which got a new skeleton.</param>
        protected abstract void Check(object src, NewSkeletonEventArgs e);

        /// <summary>
        /// Since it's up to the user to override "check" correctly, there's the
        /// possibility that he never triggered an event to make the GestureChecker proceed. 
        /// Therefore we publish that we performed a "check" and consumed time in the 
        /// GestureChecker state machine.</summary>
        /// <param name="src">
        /// The object which fired the event. (This is probably the Device class.)</param>
        /// <param name="args">
        /// NewSkeletonEventArgs contains the person which got a new skeleton.</param>
        private void ExtendedCheck(object src, NewSkeletonEventArgs args)
        {
            Check(src, args);
            OnCheck(this, new EventArgs());
        }

        #region Events

        /// <summary>
        /// Called every time a condition is checked.</summary>
        public event EventHandler<EventArgs> OnCheck;
        /// <summary>
        /// Called every time a condition successfully completed</summary>
        public event EventHandler<GesturesEventArgs> Succeeded;
        /// <summary>
        /// Called every time a condition failed</summary>
        public event EventHandler<FailedGestureEventArgs> Failed;

        public event EventHandler<BeginGestureEventArgs> GestureBegining;
        public event EventHandler<EndGestureEventArgs> GestureEnded;
        public event EventHandler<ProgressGestureEventArgs> GestureProgressed;


        /// <summary>
        /// Indicate a call to registered Success Eventhandlers</summary>
        /// <param name="sender">
        /// Probably an implementation of the GestureChecker class</param>
        /// <param name="e">
        /// Detailed arguments for a gesture part</param>
        protected void FireSucceeded(object sender, GesturesEventArgs e)
        {
            if (Succeeded != null)
            {
                Succeeded(sender, e);
            }
        }

        /// <summary>
        /// Indicate a call to registered Failed Eventhandlers</summary>
        /// <param name="sender">
        /// Probably an implementation of the GestureChecker class</param>
        /// <param name="e">
        /// Detailed arguments for a gesture part</param>
        protected void FireFailed(object sender, FailedGestureEventArgs e)
        {
            if (Failed != null)
            {
                Failed(sender, e);
            }
        }

        protected void RaiseGestureBegining(object sender, BeginGestureEventArgs e)
        {
            if (GestureBegining != null)
            {
                GestureBegining(sender, e);
            }
        }

        protected void RaiseGestureEnded(object sender, EndGestureEventArgs e)
        {
            if (GestureEnded != null)
            {
                GestureEnded(sender, e);
            }
        }

        protected void RaiseGestureProgressed(object sender, ProgressGestureEventArgs e)
        {
            if (GestureProgressed != null)
            {
                GestureProgressed(sender, e);
            }
        }

        #endregion
    }
}
