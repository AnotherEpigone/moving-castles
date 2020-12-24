using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.TurnBased;
using MovingCastles.Maps;
using MovingCastles.Serialization.Settings;
using SadConsole;
using SadConsole.Controls;
using System.Collections.Generic;

namespace MovingCastles.Ui.Consoles
{
    public class DungeonModeConsole : ContainerConsole
    {
        private const int LeftPaneWidth = 40;
        private const int TopPaneHeight = 2;
        private const int InfoPanelHeight = 8;

        private readonly ControlsConsole _leftPane;
        private List<Console> _entitySummaryConsoles;
        private DungeonMapConsole _mapConsole;
        private ProgressBar _healthBar;

        public DungeonModeConsole(
            int width,
            int height,
            Font tilesetFont,
            IMapModeMenuProvider menuProvider,
            DungeonMap map,
            ILogManager logManager,
            IAppSettings appSettings,
            ITurnBasedGame game)
        {
            var rightSectionWidth = width - LeftPaneWidth;

            var tileSizeXFactor = tilesetFont.Size.X / Global.FontDefault.Size.X;
            _mapConsole = new DungeonMapConsole(
                rightSectionWidth / tileSizeXFactor,
                height - TopPaneHeight,
                tilesetFont,
                menuProvider,
                game,
                appSettings,
                map)
            {
                Position = new Point(LeftPaneWidth, TopPaneHeight)
            };
            _mapConsole.SummaryConsolesChanged += (_, args) => HandleNewSummaryConsoles(args.Consoles);

            _leftPane = CreateLeftPane(height);

            var eventLog = new MessageLogConsole(LeftPaneWidth, height - InfoPanelHeight, Global.FontDefault)
            {
                Position = new Point(0, InfoPanelHeight),
            };
            logManager.RegisterEventListener((s, h) => eventLog.Add(s, h));
            logManager.RegisterDebugListener(s => eventLog.Add($"DEBUG: {s}", false)); // todo put debug logs somewhere else

            var healthComponent = _mapConsole.Player.GetGoRogueComponent<IHealthComponent>();
            healthComponent.HealthChanged += Player_HealthChanged;

            Children.Add(CreateTopPane(rightSectionWidth, menuProvider));
            Children.Add(_mapConsole);
            Children.Add(eventLog);
            Children.Add(_leftPane);
        }

        public void SetMap(DungeonMap map)
        {
            var healthComponent = _mapConsole.Player.GetGoRogueComponent<IHealthComponent>();
            healthComponent.HealthChanged -= Player_HealthChanged;

            _mapConsole.SetMap(map);

            healthComponent = _mapConsole.Player.GetGoRogueComponent<IHealthComponent>();
            healthComponent.HealthChanged += Player_HealthChanged;
        }

        private ControlsConsole CreateTopPane(
            int rightSectionWidth,
            IMapModeMenuProvider menuProvider)
        {
            var console = new ControlsConsole(rightSectionWidth, TopPaneHeight)
            {
                Position = new Point(LeftPaneWidth, 0),
                DisableControlFocusing = true,
            };

            const string popupMenuText = "Menu (M)";
            var popupMenuButtonWidth = popupMenuText.Length + 4;
            var popupMenuButton = new Button(popupMenuButtonWidth)
            {
                Text = popupMenuText,
                Position = new Point((rightSectionWidth / 5) - (popupMenuButtonWidth / 2), 0),
            };
            popupMenuButton.Click += (_, __) => menuProvider.Pop.Show();

            const string inventoryMenuText = "Inventory (I)";
            var inventoryMenuButtonWidth = inventoryMenuText.Length + 4;
            var inventoryMenuButton = new Button(inventoryMenuButtonWidth)
            {
                Text = inventoryMenuText,
                Position = new Point((rightSectionWidth * 2 / 5) - (inventoryMenuButtonWidth / 2), 0),
            };
            inventoryMenuButton.Click += (_, __) =>
            {
                var inventory = _mapConsole.Player.GetGoRogueComponent<IInventoryComponent>();
                menuProvider.Inventory.Show(inventory);
            };

            const string spellMenuText = "Spells (S)";
            var spellMenuButtonWidth = spellMenuText.Length + 4;
            var spellMenuButton = new Button(spellMenuButtonWidth)
            {
                Text = spellMenuText,
                Position = new Point((rightSectionWidth * 3 / 5) - (spellMenuButtonWidth / 2), 0),
            };
            spellMenuButton.Click += (_, __) =>
            {
                menuProvider.SpellSelect.Show(
                    _mapConsole.Player.GetGoRogueComponent<ISpellCastingComponent>().Spells,
                    selectedSpell => _mapConsole.StartTargetting(selectedSpell));
            };

            const string commandMenuText = "Commands (C)";
            var commandMenuButtonWidth = commandMenuText.Length + 4;
            var commandMenuButton = new Button(commandMenuButtonWidth)
            {
                Text = commandMenuText,
                Position = new Point((rightSectionWidth * 4 / 5) - (commandMenuButtonWidth / 2), 0),
            };
            commandMenuButton.Click += (_, __) =>
            {
                menuProvider.Command.Show();
            };

            var flavorMessage = new Console(rightSectionWidth, 1)
            {
                Position = new Point(1, 1),
            };
            _mapConsole.FlavorMessageChanged += (_, message) =>
            {
                flavorMessage.Clear();
                flavorMessage.Cursor.Position = new Point(0, 0);

                var coloredMessage = new ColoredString(message, new Cell(Color.Gainsboro, _mapConsole.DefaultBackground));
                flavorMessage.Cursor.Print(coloredMessage);
            };

            console.Add(popupMenuButton);
            console.Add(inventoryMenuButton);
            console.Add(spellMenuButton);
            console.Add(commandMenuButton);

            console.Children.Add(flavorMessage);

            return console;
        }

        private ControlsConsole CreateLeftPane(int height)
        {
            var leftPane = new ControlsConsole(LeftPaneWidth, height)
            {
                ThemeColors = ColorHelper.GetThemeColorsForBackgroundColor(Color.Transparent),
            };
            var infoPanel = new ControlsConsole(LeftPaneWidth, InfoPanelHeight);
            var manaBar = new ProgressBar(LeftPaneWidth, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 4),
            };
            manaBar.ThemeColors = ColorHelper.GetProgressBarThemeColors(ColorHelper.DepletedManaBlue, ColorHelper.ManaBlue);
            manaBar.Progress = 1;

            _healthBar = new ProgressBar(LeftPaneWidth, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 3),
            };
            _healthBar.ThemeColors = ColorHelper.GetProgressBarThemeColors(ColorHelper.DepletedHealthRed, ColorHelper.HealthRed);

            infoPanel.Add(manaBar);
            infoPanel.Add(_healthBar);

            // test data
            infoPanel.Add(new Label("Vede of Tattersail") { Position = new Point(1, 0), TextColor = Color.Gainsboro });
            infoPanel.Add(new Label("Material Plane, Ayen") { Position = new Point(1, 1), TextColor = Color.DarkGray });
            infoPanel.Add(new Label("Old Alward's Tower") { Position = new Point(1, 2), TextColor = Color.DarkGray });

            leftPane.Children.Add(infoPanel);
            return leftPane;
        }

        private void Player_HealthChanged(object sender, float e)
        {
            var healthComponent = _mapConsole.Player.GetGoRogueComponent<IHealthComponent>();
            _healthBar.Progress = healthComponent.Health / healthComponent.MaxHealth;
        }

        private void HandleNewSummaryConsoles(List<Console> consoles)
        {
            _entitySummaryConsoles?.ForEach(c => _leftPane.Children.Remove(c));

            _entitySummaryConsoles = consoles;

            var yOffset = 8;
            _entitySummaryConsoles.ForEach(c =>
            {
                c.Position = new Point(20, yOffset);
                yOffset += c.Height;
                _leftPane.Children.Add(c);
            });
        }
    }
}
