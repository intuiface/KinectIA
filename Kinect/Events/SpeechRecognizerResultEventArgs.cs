            // ****************************************************************************
            // <copyright file="SpeechRecognizerResultEventArgs.cs" company="IntuiLab">
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

namespace IntuiLab.Kinect.Events
{
    #region Delegate
    public delegate void SpeechRecognizerResultEventHandler(object sender, SpeechRecognizerResultEventArgs e);
    #endregion

    public class SpeechRecognizerResultEventArgs : EventArgs
    {
        private string m_SentenceResult;
        public string SentenceResult
        {
            get
            {
                return m_SentenceResult;
            }
        }

        private string m_SementicResult;
        public string SementicResult
        {
            get
            {
                return m_SementicResult;
            }
        }

        public SpeechRecognizerResultEventArgs(string sentenceResult, string sementicResult)
        {
            m_SentenceResult = sentenceResult;
            m_SementicResult = sementicResult;
        }
    }
}
