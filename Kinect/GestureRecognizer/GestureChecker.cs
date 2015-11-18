using IntuiLab.Kinect.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntuiLab.Kinect.GestureRecognizer
{
    internal class GestureChecker : IDisposable
    {
        /// <summary>
        /// List of gesture parts</summary>
        private List<Condition> conditions;

        /// <summary>
        /// Gesture state: Points to current gesture part (Condition)</summary>
        private IEnumerator<Condition> index;

        /// <summary>
        /// How long a full gesture can take in maximum</summary>
        private long timeout;

        /// <summary>
        /// Time keeper: Points to the time when the gesture (re)started</summary>
        private long _startTime;

        /// <summary>
        /// Taking a list of conditions, which are gesture parts to be checked in order
        /// and a timeout indicating how long a full gesture can take in maximum.
        /// </summary>
        /// <param name="gestureConditions">
        /// List of condition which are to fullfill for a successful gesture</param>
        /// <param name="timeout">
        /// Maximum time a gesture is allowed to run for.</param>
        public GestureChecker(IEnumerable<Condition> gestureConditions, int timeout)
        {
            this.timeout = timeout;

            conditions = gestureConditions.ToList();
            conditions.ForEach(delegate(Condition c)
            {
                c.OnCheck += ConditionChecked;
                c.Succeeded += ConditionComplete;
                c.Failed += ConditionFailed;
                c.GestureBegining += OnGestureBegining;
                c.GestureEnded += OnGestureEnded;
                c.GestureProgressed += OnGestureProgressed;
            });

            index = conditions.GetEnumerator();
            index.MoveNext();
            Reset();
        }

        /// <summary>
        /// Reset state machine. Includes timeouts and condition list.</summary>
        public void Reset()
        {
            _startTime = CurrentMillis.Millis;
            /*
             * Disable all conditions although there should be only one enabled: the last on index.Current
             * But since it can be NULL and there could occurr Exceptions in user code,
             * we invest a bit performance to securely save gesture checking performance.
             */
            foreach (Condition c in conditions)
            {
                c.Disable();
            }
            index.Reset();
            index.MoveNext();
            index.Current.Enable();
        }

        #region Events

        /// <summary>
        /// Called when a gesture was recognized. That means that all gesture 
        /// parts were sucessfully recognized.</summary>
        public virtual event EventHandler<GesturesEventArgs> Successful;

        /// <summary>
        /// Called when at least one gesture part failed.</summary>
        public virtual event EventHandler<FailedGestureEventArgs> Failed;

        public virtual event EventHandler<BeginGestureEventArgs> GestureBegin;
        public virtual event EventHandler<EndGestureEventArgs> GestureEnd;
        public virtual event EventHandler<ProgressGestureEventArgs> GestureProgress;

        /// <summary>
        /// Every time when a condition is checked, we check if its in time.</summary>
        /// <param name="src">
        /// The checked Condition</param>
        /// <param name="e">
        /// Probably empty</param>
        private void ConditionChecked(Object src, EventArgs e)
        {
            if (_startTime <= CurrentMillis.Millis - timeout)
            {
                ConditionFailed(this, new FailedGestureEventArgs
                {
                    refCondition = (Condition)src
                });
            }
        }

        /// <summary>
        /// A gesture part failed. Lets start from the beginning.</summary>
        /// <param name="src">
        /// The checked Condition</param>
        /// <param name="e">
        /// Details about the fail</param>
        private void ConditionFailed(Object src, FailedGestureEventArgs e)
        {
            Reset();
            FireFailed(this, e);
        }

        /// <summary>
        /// Current gesture part was sucessful. Continue with next.</summary>
        /// <param name="src">
        /// The checked condition</param>
        /// <param name="e">
        /// Details about the success</param>
        private void ConditionComplete(Object src, GesturesEventArgs e)
        {
            Condition previous = index.Current;
            Boolean hasNext = index.MoveNext();
            Condition next = index.Current;

            if (hasNext) // no further gesture parts -> success!
            {
                previous.Disable();
                next.Enable();
            }
            else
            {
                Reset();
                FireSucessful(this, e);
            }
        }

        private void OnGestureBegining(Object src, BeginGestureEventArgs e)
        {
            RaiseGestureBegin(src, e);
        }

        private void OnGestureEnded(Object src, EndGestureEventArgs e)
        {
            RaiseGestureEnd(src, e);
        }

        private void OnGestureProgressed(Object src, ProgressGestureEventArgs e)
        {
            RaiseGestureProgress(src, e);
        }

        protected virtual void FireSucessful(Object sender, GesturesEventArgs e)
        {
            if (Successful != null)
            {
                Successful(this, e);
            }
        }


        protected virtual void FireFailed(Object sender, FailedGestureEventArgs e)
        {
            if (Failed != null)
            {
                Failed(this, e);
            }
        }

        protected virtual void RaiseGestureBegin(Object sender, BeginGestureEventArgs e)
        {
            if (GestureBegin != null)
            {
                GestureBegin(this, e);
            }
        }

        protected virtual void RaiseGestureEnd(Object sender, EndGestureEventArgs e)
        {
            if (GestureEnd != null)
            {
                GestureEnd(this, e);
            }
        }

        protected virtual void RaiseGestureProgress(Object sender, ProgressGestureEventArgs e)
        {
            if (GestureProgress != null)
            {
                GestureProgress(this, e);
            }
        }

        #endregion

        #region IDisposable's members

        /// <summary>
        /// Dispose this instance.
        /// </summary>
        public void Dispose()
        {
            foreach(Condition c in conditions)
            {
                c.OnCheck -= ConditionChecked;
                c.Succeeded -= ConditionComplete;
                c.Failed -= ConditionFailed;
                c.GestureBegining -= OnGestureBegining;
                c.GestureEnded -= OnGestureEnded;
                c.GestureProgressed -= OnGestureProgressed;

                c.Disable();
            }
        }
        #endregion
    }
}
