using System;

namespace MovingCastles.GameSystems.Logging
{
    public interface ILogManager
    {
        void DebugLog(string message);
        void EventLog(string message);
        void EventLog(string message, bool highlight);
        void RegisterEventListener(Action<string, bool> listener);
        void UnregisterEventListener(Action<string, bool> listener);
        void RegisterDebugListener(Action<string> listener);
        void UnregisterDebugListener(Action<string> listener);
    }
}