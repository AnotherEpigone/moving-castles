using System;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.Logging
{
    public class LogManager : ILogManager
    {
        private readonly Dictionary<LogType, List<Action<string, bool>>> _eventListeners;

        public LogManager()
        {
            _eventListeners = new Dictionary<LogType, List<Action<string, bool>>>();
        }

        public void RegisterEventListener(LogType type, Action<string, bool> listener)
        {
            if (!_eventListeners.ContainsKey(type))
            {
                _eventListeners[type] = new List<Action<string, bool>>();
            }

            _eventListeners[type].Add(listener);
        }

        public void UnregisterEventListener(LogType type, Action<string, bool> listener)
        {
            _eventListeners[type].Remove(listener);
        }

        public void CombatLog(string message)
        {
            EventLog(LogType.Combat, message);
        }

        public void CombatLog(string message, bool highlight)
        {
            EventLog(LogType.Combat, message, highlight);
        }

        public void StoryLog(string message)
        {
            EventLog(LogType.Story, message);
        }

        public void StoryLog(string message, bool highlight)
        {
            EventLog(LogType.Story, message, highlight);
        }

        public void EventLog(LogType type, string message)
        {
            EventLog(type, message, false);
        }

        public void EventLog(LogType type, string message, bool highlight)
        {
            _eventListeners[type].ForEach(action => action(message, highlight));
        }
    }
}
