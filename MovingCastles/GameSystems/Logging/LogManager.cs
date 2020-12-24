using System;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.Logging
{
    public class LogManager : ILogManager
    {
        private readonly List<Action<string, bool>> _eventListeners;
        private readonly List<Action<string>> _debugListeners;

        public LogManager()
        {
            _eventListeners = new List<Action<string, bool>>();
            _debugListeners = new List<Action<string>>();
        }

        public void RegisterEventListener(Action<string, bool> listener)
        {
            _eventListeners.Add(listener);
        }

        public void UnregisterEventListener(Action<string, bool> listener)
        {
            _eventListeners.Remove(listener);
        }

        public void RegisterDebugListener(Action<string> listener)
        {
            _debugListeners.Add(listener);
        }

        public void UnregisterDebugListener(Action<string> listener)
        {
            _debugListeners.Remove(listener);
        }

        public void EventLog(string message)
        {
            EventLog(message, false);
        }

        public void EventLog(string message, bool highlight)
        {
            _eventListeners.ForEach(action => action(message, highlight));
        }

        public void DebugLog(string message)
        {
            _debugListeners.ForEach(action => action(message));
        }
    }
}
