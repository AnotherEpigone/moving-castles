using MovingCastles.Ui;
using SadConsole;
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
            Game.Create(_uiManager.ViewPortWidth, _uiManager.ViewPortHeight);
            Game.OnInitialize = Init;

            // Start the game.
            Game.Instance.Run();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Init()
        {
            Global.LoadFont(UiManager.TilesetFontPath);
            _uiManager.GuiTextFont = Global.FontDefault;
            Global.FontDefault = Global.Fonts[UiManager.TilesetFontName].GetFont(Font.FontSizes.One);

            Global.CurrentScreen = _uiManager.Screen;
        }

        private void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                Game.Instance.Dispose();
            }

            disposedValue = true;
        }
    }
}
