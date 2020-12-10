using MovingCastles.GameSystems;
using MovingCastles.Serialization.Settings;
using MovingCastles.Ui;
using MovingCastles.Ui.Controls;
using SadConsole;
using SadConsole.Themes;
using System;

namespace MovingCastles
{
    internal sealed class MovingCastles : IDisposable
    {
        private readonly IUiManager _uiManager;
        private readonly IGameManager _gameManager;
        private readonly IAppSettings _appSettings;

        private bool disposedValue;

        public MovingCastles(IUiManager uiManager, IGameManager gameManager, IAppSettings appSettings)
        {
            _uiManager = uiManager;
            _gameManager = gameManager;
            _appSettings = appSettings;
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
            InitControls();

            Game.Instance.Window.Title = "Moving Castles";
            Settings.ResizeMode = Settings.WindowResizeOptions.None;
            Settings.FullScreenPreventScaleChangeForNone = true;

            if (_appSettings.FullScreen)
            {
                _uiManager.ToggleFullScreen();
            }

            _uiManager.ShowMainMenu(_gameManager);
        }

        private void InitColors()
        {
            var colors = Library.Default.Colors;

            colors.TitleText = colors.Orange;

            colors.TextBright = colors.White;
            colors.Text = colors.Blue;
            colors.TextSelected = colors.White;
            colors.TextSelectedDark = colors.White;
            colors.TextLight = colors.GrayDark;
            colors.TextDark = colors.Green;
            colors.TextFocused = colors.Cyan;

            colors.Lines = colors.Gray;

            colors.ControlBack = ColorHelper.MidnightestBlue;
            colors.ControlBackLight = ColorHelper.SelectedControlBackBlue;
            colors.ControlBackSelected = colors.GrayDark;
            colors.ControlBackDark = ColorHelper.MidnightestBlue;
            colors.ControlHostBack = ColorHelper.MidnightestBlue;
            colors.ControlHostFore = colors.Text;

            colors.RebuildAppearances();
        }

        private void InitControls()
        {
            Library.Default.SetControlTheme(typeof(McSelectionButton), new ButtonTheme());
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
