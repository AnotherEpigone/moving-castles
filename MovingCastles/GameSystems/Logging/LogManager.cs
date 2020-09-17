using System;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.Logging
{
    public class LogManager : ILogManager
    {
        private readonly List<Action<string>> _eventListeners;

        public LogManager()
        {
            _eventListeners = new List<Action<string>>();
        }

        public void RegisterEventListener(Action<string> listener)
        {
            _eventListeners.Add(listener);
        }

        public void UnregisterEventListener(Action<string> listener)
        {
            _eventListeners.Remove(listener);
        }

        public void EventLog(string message)
        {
            _eventListeners.ForEach(action => action(message));
        }
    }
}
