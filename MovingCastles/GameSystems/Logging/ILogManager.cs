using System;

namespace MovingCastles.GameSystems.Logging
{
    public enum LogType
    {
        Debug,
        Combat,
        Story,
    }

    public interface ILogManager
    {
        void RegisterEventListener(LogType type, Action<string, bool> listener);
        void UnregisterEventListener(LogType type, Action<string, bool> listener);
        void CombatLog(string message);
        void CombatLog(string message, bool highlight);
        void StoryLog(string message);
        void StoryLog(string message, bool highlight);
        void EventLog(LogType type, string message);
        void EventLog(LogType type, string message, bool highlight);
    }
}