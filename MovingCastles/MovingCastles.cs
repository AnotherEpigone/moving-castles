using MovingCastles.Ui;
using System;

namespace MovingCastles
{
    internal sealed class MovingCastles : IDisposable
    {
        private readonly UiManager _uiManager;

        private bool disposedValue;

        public MovingCastles()
        {
            _uiManager = new UiManager();
        }

        public void Run()
        {
            SadConsole.Game.Create(_uiManager.ViewPortWidth, _uiManager.ViewPortHeight);
            SadConsole.Game.OnInitialize = Init;

            // Start the game.
            SadConsole.Game.Instance.Run();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Init()
        {
            SadConsole.Global.CurrentScreen = _uiManager.Screen;
        }

        private void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                SadConsole.Game.Instance.Dispose();
            }

            disposedValue = true;
        }
    }
}
