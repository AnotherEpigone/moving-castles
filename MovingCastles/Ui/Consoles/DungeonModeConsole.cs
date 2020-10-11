using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Player;
using MovingCastles.GameSystems.TurnBasedGame;
using MovingCastles.Maps;
using SadConsole;
using SadConsole.Controls;
using System.Collections.Generic;

namespace MovingCastles.Ui.Consoles
{
    public class DungeonModeConsole : ContainerConsole
    {
        private const int LeftPaneWidth = 30;
        private const int TopPaneHeight = 3;
        private const int InfoPanelHeight = 8;

        private readonly ControlsConsole _leftPane;
        private List<Console> _entitySummaryConsoles;

        public DungeonModeConsole(
            int width,
            int height,
            Font tilesetFont,
            IMapModeMenuProvider menuProvider,
            IMapFactory mapFactory,
            IMapPlan mapPlan,
            ILogManager logManager,
            ITurnBasedGame game,
            Player playerInfo)
        {
            var rightSectionWidth = width - LeftPaneWidth;

            var tileSizeXFactor = tilesetFont.Size.X / Global.FontDefault.Size.X;
            var map = mapFactory.CreateDungeonMap(80, 45, mapPlan, playerInfo);
            var mapConsole = new DungeonMapConsole(
                rightSectionWidth / tileSizeXFactor,
                height - TopPaneHeight,
                tilesetFont,
                menuProvider,
                game,
                map)
            {
                Position = new Point(LeftPaneWidth, TopPaneHeight)
            };
            mapConsole.SummaryConsolesChanged += (_, args) => HandleNewSummaryConsoles(args.Consoles);

            _leftPane = CreateLeftPane(height, mapConsole);

            var eventLog = new MessageLogConsole(LeftPaneWidth, height - InfoPanelHeight, Global.FontDefault)
            {
                Position = new Point(0, InfoPanelHeight),
            };
            logManager.RegisterEventListener(s => eventLog.Add(s));
            logManager.RegisterDebugListener(s => eventLog.Add($"DEBUG: {s}")); // todo put debug logs somewhere else

            Children.Add(CreateTopPane(rightSectionWidth, mapConsole, menuProvider));
            Children.Add(mapConsole);
            Children.Add(eventLog);
            Children.Add(_leftPane);
        }

        private ControlsConsole CreateTopPane(
            int rightSectionWidth,
            DungeonMapConsole mapConsole,
            IMapModeMenuProvider menuProvider)
        {
            var console = new ControlsConsole(rightSectionWidth, TopPaneHeight)
            {
                Position = new Point(LeftPaneWidth, 0)
            };

            const string popupMenuText = "Menu";
            var popupMenuButtonWidth = popupMenuText.Length + 4;
            var popupMenuButton = new Button(popupMenuButtonWidth)
            {
                Text = popupMenuText,
                Position = new Point((rightSectionWidth / 3) - (popupMenuButtonWidth / 2), 0),
            };
            popupMenuButton.Click += (_, __) => menuProvider.Pop.Show();

            const string inventoryMenuText = "Inventory";
            var inventoryMenuButtonWidth = inventoryMenuText.Length + 4;
            var inventoryMenuButton = new Button(inventoryMenuButtonWidth)
            {
                Text = inventoryMenuText,
                Position = new Point((rightSectionWidth * 2 / 3) - (inventoryMenuButtonWidth / 2), 0),
            };
            inventoryMenuButton.Click += (_, __) =>
            {
                var inventory = mapConsole.Player.GetGoRogueComponent<IInventoryComponent>();
                menuProvider.Inventory.Show(inventory);
            };

            console.Add(popupMenuButton);
            console.Add(inventoryMenuButton);

            return console;
        }

        private ControlsConsole CreateLeftPane(int height, DungeonMapConsole dungeonMapConsole)
        {
            var leftPane = new ControlsConsole(LeftPaneWidth, height)
            {
                ThemeColors = ColorHelper.GetThemeColorsForBackgroundColor(Color.Transparent),
            };
            var infoPanel = new ControlsConsole(LeftPaneWidth, InfoPanelHeight);
            var manaBar = new ProgressBar(30, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 4),
            };
            manaBar.ThemeColors = ColorHelper.GetProgressBarThemeColors(ColorHelper.DepletedManaBlue, ColorHelper.ManaBlue);
            manaBar.Progress = 1;

            var healthComponent = dungeonMapConsole.Player.GetGoRogueComponent<IHealthComponent>();
            var healthBar = new ProgressBar(30, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 3),
            };
            healthBar.ThemeColors = ColorHelper.GetProgressBarThemeColors(ColorHelper.DepletedHealthRed, ColorHelper.HealthRed);
            dungeonMapConsole.Player.GetGoRogueComponent<IHealthComponent>().HealthChanged += (_, __) =>
            {
                healthBar.Progress = healthComponent.Health / healthComponent.MaxHealth;
            };
            healthBar.Progress = healthComponent.Health / healthComponent.MaxHealth;

            infoPanel.Add(manaBar);
            infoPanel.Add(healthBar);

            // test data
            infoPanel.Add(new Label("Vede of Tattersail") { Position = new Point(1, 0), TextColor = Color.Gainsboro });
            infoPanel.Add(new Label("Material Plane, Ayen") { Position = new Point(1, 1), TextColor = Color.DarkGray });
            infoPanel.Add(new Label("Old Alward's Tower") { Position = new Point(1, 2), TextColor = Color.DarkGray });

            leftPane.Children.Add(infoPanel);
            return leftPane;
        }

        private void HandleNewSummaryConsoles(List<Console> consoles)
        {
            _entitySummaryConsoles?.ForEach(c => _leftPane.Children.Remove(c));

            _entitySummaryConsoles = consoles;

            var yOffset = 8;
            _entitySummaryConsoles.ForEach(c =>
            {
                c.Position = new Point(0, yOffset);
                yOffset += c.Height;
                _leftPane.Children.Add(c);
            });
        }
    }
}
