using System;
using System.Collections.Generic;
using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Helpers/Debugger")]
    public class Debugger : DuMonoBehaviour
    {
        [SerializeField]
        private bool m_LogInMessageBox = true;
        public bool logInMessageBox
        {
            get => m_LogInMessageBox;
            set => m_LogInMessageBox = value;
        }

        [SerializeField]
        private bool m_LogInConsole = false;
        public bool logInConsole
        {
            get => m_LogInConsole;
            set => m_LogInConsole = value;
        }

        [SerializeField]
        private int m_MessagesLimit = 16;
        public int messagesLimit
        {
            get => m_MessagesLimit;
            set
            {
                m_MessagesLimit = value;
                OptimizeLogMessages();
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private List<string> m_LogMessages;
        private List<string> logMessages
        {
            get
            {
                if (Dust.IsNull(m_LogMessages))
                    m_LogMessages = new List<string>();

                return m_LogMessages;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Log: string

        public void LogNotice(string message)
        {
            if (logInConsole)
                Debug.Log($"Debugger [{gameObject.name}]: {message}");

            if (logInMessageBox)
                AppendLogMessages($"[N] {message}");
        }

        public void LogWarning(string message)
        {
            if (logInConsole)
                Debug.LogWarning($"Debugger [{gameObject.name}]: {message}");

            if (logInMessageBox)
                AppendLogMessages($"[W] {message}");
        }

        public void LogError(string message)
        {
            if (logInConsole)
                Debug.LogError($"Debugger [{gameObject.name}]: {message}");

            if (logInMessageBox)
                AppendLogMessages($"[E] {message}");
        }

        //--------------------------------------------------------------------------------------------------------------

        private void AppendLogMessages(string message)
        {
            logMessages.Add(message);

            OptimizeLogMessages();
        }

        private void OptimizeLogMessages()
        {
            if (messagesLimit <= 0)
                return;

            while (logMessages.Count > messagesLimit)
                logMessages.RemoveAt(0);
        }

        public string GetPlainLogMessages()
        {
            return String.Join("\n", logMessages);
        }

        public void ClearLogMessages()
        {
            logMessages.Clear();
        }
    }
}
