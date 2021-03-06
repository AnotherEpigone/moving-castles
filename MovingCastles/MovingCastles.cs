﻿using MovingCastles.GameSystems;
using MovingCastles.Serialization.Settings;
using MovingCastles.Ui;
using MovingCastles.Ui.Controls;
using SadConsole;
using SadConsole.Controls;
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
            Settings.AllowWindowResize = false;

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
            Global.LoadFont(UiManager.DungeonFontPath);
            Global.LoadFont(UiManager.CastleFontPath);

            InitColors();
            InitControls();

            Game.Instance.Window.Title = "Moving Castles";
            Settings.ResizeMode = Settings.WindowResizeOptions.None;
            Settings.FullScreenPreventScaleChangeForNone = true;

            if (_appSettings.FullScreen)
            {
                _uiManager.ToggleFullScreen();
            }
            else
            {
                _uiManager.SetViewport(_appSettings.Viewport.width, _appSettings.Viewport.height);
            }

            _uiManager.ShowMainMenu(_gameManager);
        }

        private static void InitColors()
        {
            var colors = Library.Default.Colors;

            colors.TitleText = colors.Orange;

            colors.TextBright = ColorHelper.TextBright;
            colors.Text = ColorHelper.Text;
            colors.TextSelected = ColorHelper.TextBright;
            colors.TextSelectedDark = ColorHelper.TextBright;
            colors.TextLight = ColorHelper.SelectedBackground;
            colors.TextDark = ColorHelper.TextBright;
            colors.TextFocused = ColorHelper.TextBright;

            colors.Lines = colors.Gray;

            colors.ControlBack = ColorHelper.ControlBack;
            colors.ControlBackLight = ColorHelper.SelectedBackground;
            colors.ControlBackSelected = ColorHelper.SelectedBackground;
            colors.ControlBackDark = ColorHelper.ControlBack;
            colors.ControlHostBack = ColorHelper.ControlBack;
            colors.ControlHostFore = ColorHelper.Text;

            colors.RebuildAppearances();
        }

        private static void InitControls()
        {
            var buttonTheme = new ButtonTheme
            {
                EndCharacterLeft = 4,
                EndCharacterRight = 4,
            };

            Library.Default.SetControlTheme(typeof(McSelectionButton), buttonTheme);
            Library.Default.SetControlTheme(typeof(Button), buttonTheme);
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
