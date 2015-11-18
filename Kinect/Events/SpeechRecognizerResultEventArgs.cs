using System;

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
