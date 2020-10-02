using Microsoft.Xna.Framework;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using SadConsole;
using SadConsole.Controls;

namespace MovingCastles.Ui.Consoles
{
    public sealed class CastleModeConsole : Console
    {
        private const int LeftPaneWidth = 30;
        private const int TopPaneHeight = 3;
        private const int RightPaneWidth = 30;

        private readonly ControlsConsole _leftPane;
        private readonly MessageLogConsole _rightPane;
        private readonly CastleMapConsole _centerPane;

        public CastleModeConsole(
            int width,
            int height,
            Font tilesetFont,
            IMapModeMenuProvider menuProvider,
            IMapFactory mapFactory,
            IMapPlan mapPlan,
            ILogManager logManager)
            : base(width, height)
        {
            _leftPane = CreateLeftPane();

            _rightPane = new MessageLogConsole(RightPaneWidth, height, Global.FontDefault)
            {
                Position = new Point(width - RightPaneWidth, 0),
            };
            _rightPane.Add("Started a new game, and here's the first message.");

            var map = mapFactory.CreateCastleMap(7, 7, mapPlan);
            var tileSizeXFactor = tilesetFont.Size.X / Global.FontDefault.Size.X;
            var centerPaneWidth = width - LeftPaneWidth - RightPaneWidth;

            _centerPane = new CastleMapConsole(
                centerPaneWidth / tileSizeXFactor,
                height,
                tilesetFont,
                menuProvider,
                map)
            {
                Position = new Point(LeftPaneWidth, 0),
            };

            logManager.RegisterEventListener(s => _rightPane.Add(s));
            logManager.RegisterDebugListener(s => _rightPane.Add($"DEBUG: {s}")); // todo put debug logs somewhere else

            Children.Add(_leftPane);
            Children.Add(_rightPane);
            Children.Add(_centerPane);
        }

        private ControlsConsole CreateLeftPane()
        {
            var leftPane = new ControlsConsole(LeftPaneWidth, Height);
            var manaBar = new ProgressBar(30, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 4),
            };
            manaBar.ThemeColors = ColorHelper.GetProgressBarThemeColors(ColorHelper.DepletedManaBlue, ColorHelper.ManaBlue);
            manaBar.Progress = 1;

            // TODO need a player to reference here, but not an entity!
            //var healthComponent = mapConsole.Player.GetGoRogueComponent<IHealthComponent>();
            var healthBar = new ProgressBar(30, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 3),
            };
            healthBar.ThemeColors = ColorHelper.GetProgressBarThemeColors(ColorHelper.DepletedHealthRed, ColorHelper.HealthRed);

            //mapConsole.Player.GetGoRogueComponent<IHealthComponent>().HealthChanged += (_, __) =>
            //{
            //    healthBar.Progress = healthComponent.Health / healthComponent.MaxHealth;
            //};
            //healthBar.Progress = healthComponent.Health / healthComponent.MaxHealth;

            // test data
            leftPane.Add(new Label("Vede of Tattersail") { Position = new Point(1, 0), TextColor = Color.Gainsboro });
            leftPane.Add(new Label("Material Plane, Ayen") { Position = new Point(1, 1), TextColor = Color.DarkGray });
            leftPane.Add(new Label("Old Alward's Tower") { Position = new Point(1, 2), TextColor = Color.DarkGray });

            leftPane.Add(manaBar);
            leftPane.Add(healthBar);

            return leftPane;
        }
    }
}
