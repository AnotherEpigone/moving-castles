using System;

namespace MovingCastles.GameSystems.Logging
{
    public interface ILogManager
    {
        void DebugLog(string message);
        void EventLog(string message);
        void RegisterEventListener(Action<string> listener);
        void UnregisterEventListener(Action<string> listener);
        void RegisterDebugListener(Action<string> listener);
        void UnregisterDebugListener(Action<string> listener);
    }
}