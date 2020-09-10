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

            InitColors();

            Global.CurrentScreen = _uiManager.CreateMainMenu();
        }

        private void InitColors()
        {
            var colors = SadConsole.Themes.Library.Default.Colors;

            colors.TitleText = colors.Orange;

            colors.TextBright = colors.White;
            colors.Text = colors.Blue;
            colors.TextSelected = colors.White;
            colors.TextSelectedDark = colors.White;
            colors.TextLight = colors.GrayDark;
            colors.TextDark = colors.Green;
            colors.TextFocused = colors.Cyan;

            colors.Lines = colors.Gray;

            colors.ControlBack = UiManager.MidnightestBlue;
            colors.ControlBackLight = UiManager.MidnightestBlue;
            colors.ControlBackSelected = colors.GrayDark;
            colors.ControlBackDark = UiManager.MidnightestBlue;
            colors.ControlHostBack = UiManager.MidnightestBlue;
            colors.ControlHostFore = colors.Text;

            colors.RebuildAppearances();
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
