﻿using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.Components.Stats;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.TurnBased;
using MovingCastles.Maps;
using MovingCastles.Serialization.Settings;
using SadConsole;
using SadConsole.Controls;

namespace MovingCastles.Ui.Consoles
{
    public class DungeonModeConsole : ContainerConsole
    {
        private const int LeftPaneWidth = 40;
        private const int RightPaneWidth = 40;
        private const int TopPaneHeight = 2;
        private const int InfoPanelHeight = 8;

        private DungeonMapConsole _mapConsole;
        private ProgressBar _healthBar;
        private ProgressBar _endowmentBar;
        private Console _statOverlay;
        private Console _infoPanel;
        private Console _characterPanel;
        private readonly IDungeonMaster _dungeonMaster;

        public DungeonModeConsole(
            int width,
            int height,
            Font tilesetFont,
            IMapModeMenuProvider menuProvider,
            IDungeonMaster dungeonMaster,
            ILogManager logManager,
            IAppSettings appSettings,
            ITurnBasedGame game)
        {
            _dungeonMaster = dungeonMaster;

            var middleSectionWidth = width - LeftPaneWidth - RightPaneWidth;

            var tileSizeXFactor = tilesetFont.Size.X / Global.FontDefault.Size.X;
            _mapConsole = new DungeonMapConsole(
                middleSectionWidth / tileSizeXFactor,
                height - TopPaneHeight,
                tilesetFont,
                menuProvider,
                game,
                appSettings,
                _dungeonMaster.LevelMaster.Level.Map)
            {
                Position = new Point(LeftPaneWidth, TopPaneHeight)
            };

            CreateInfoPanel();
            CreateCharacterPanel(width, height);

            var combatEventLog = new MessageLogConsole(
                LeftPaneWidth,
                (height - InfoPanelHeight) / 2,
                Global.FontDefault,
                Color.Transparent,
                Color.Black,
                System.TimeSpan.FromSeconds(5))
            {
                Position = new Point(LeftPaneWidth, TopPaneHeight),
            };
            logManager.RegisterEventListener(LogType.Combat, (s, h) => combatEventLog.Add(s, h));
            var storyEventLog = new MessageLogConsole(LeftPaneWidth, height - InfoPanelHeight, Global.FontDefault)
            {
                Position = new Point(0, InfoPanelHeight),
            };
            logManager.RegisterEventListener(LogType.Story, (s, h) => storyEventLog.Add(s, h));

            _dungeonMaster.TimeMaster.TimeUpdated += (_, __) =>
            {
                PrintInfoPanel();
                PrintCharacterPanel();
            };

            Children.Add(_mapConsole);
            Children.Add(CreateTopPane(middleSectionWidth, menuProvider));
            Children.Add(combatEventLog);
            Children.Add(storyEventLog);
            Children.Add(_infoPanel);
            Children.Add(_characterPanel);
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
                Position = new Point((rightSectionWidth / 6) - (popupMenuButtonWidth / 2), 0),
            };
            popupMenuButton.Click += (_, __) => menuProvider.Pop.Show();

            const string inventoryMenuText = "Inventory (I)";
            var inventoryMenuButtonWidth = inventoryMenuText.Length + 4;
            var inventoryMenuButton = new Button(inventoryMenuButtonWidth)
            {
                Text = inventoryMenuText,
                Position = new Point((rightSectionWidth * 2 / 6) - (inventoryMenuButtonWidth / 2), 0),
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
                Position = new Point((rightSectionWidth * 3 / 6) - (spellMenuButtonWidth / 2), 0),
            };
            spellMenuButton.Click += (_, __) =>
            {
                menuProvider.SpellSelect.Show(
                    _mapConsole.Player.GetGoRogueComponent<ISpellCastingComponent>().Spells,
                    selectedSpell => _mapConsole.StartTargetting(selectedSpell),
                    _mapConsole.Player.GetGoRogueComponent<IEndowmentPoolComponent>().Value);
            };

            const string journalMenuText = "Journal (J)";
            var journalMenuButtonWidth = journalMenuText.Length + 4;
            var journalMenuButton = new Button(journalMenuButtonWidth)
            {
                Text = journalMenuText,
                Position = new Point((rightSectionWidth * 4 / 6) - (journalMenuButtonWidth / 2), 0),
            };
            journalMenuButton.Click += (_, __) =>
            {
                menuProvider.Journal.Show(_mapConsole.Player.JournalEntries);
            };

            const string commandMenuText = "Commands (C)";
            var commandMenuButtonWidth = commandMenuText.Length + 4;
            var commandMenuButton = new Button(commandMenuButtonWidth)
            {
                Text = commandMenuText,
                Position = new Point((rightSectionWidth * 5 / 6) - (commandMenuButtonWidth / 2), 0),
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
            console.Add(journalMenuButton);
            console.Children.Add(flavorMessage);

            return console;
        }

        private Console CreateCharacterPanel(int viewportWidth, int viewportHeight)
        {
            _characterPanel = new Console(RightPaneWidth, viewportHeight)
            {
                DefaultBackground = ColorHelper.ControlBack,
                Position = new Point(viewportWidth - RightPaneWidth, 0),
            };

            PrintCharacterPanel();

            // stat bar panel setup
            var controlPanel = new ControlsConsole(LeftPaneWidth, 2)
            {
                Position = new Point(0, 2),
            };
            _endowmentBar = new ProgressBar(LeftPaneWidth, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 1),
            };
            _endowmentBar.ThemeColors = ColorHelper.GetProgressBarThemeColors(ColorHelper.DepletedManaBlue, ColorHelper.ManaBlue);

            var endowmentComponent = _mapConsole.Player.GetGoRogueComponent<IEndowmentPoolComponent>();
            endowmentComponent.ValueChanged += Player_EndowmentChanged;
            _endowmentBar.Progress = endowmentComponent.Value / endowmentComponent.MaxValue;

            _healthBar = new ProgressBar(LeftPaneWidth, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 0),
            };
            _healthBar.ThemeColors = ColorHelper.GetProgressBarThemeColors(ColorHelper.DepletedHealthRed, ColorHelper.HealthRed);

            var healthComponent = _mapConsole.Player.GetGoRogueComponent<IHealthComponent>();
            healthComponent.HealthChanged += Player_HealthChanged;
            _healthBar.Progress = healthComponent.Health / healthComponent.MaxHealth;

            controlPanel.Add(_endowmentBar);
            controlPanel.Add(_healthBar);

            // stat overlay setup
            _statOverlay = new Console(LeftPaneWidth, InfoPanelHeight)
            {
                Position = controlPanel.Position,
                DefaultBackground = Color.Transparent,
            };

            PrintStatOverlays(healthComponent, endowmentComponent);

            _characterPanel.Children.Add(controlPanel);
            _characterPanel.Children.Add(_statOverlay);

            return _characterPanel;
        }

        private Console CreateInfoPanel()
        {
            // base info panel
            _infoPanel = new Console(LeftPaneWidth, InfoPanelHeight)
            {
                DefaultBackground = ColorHelper.ControlBack,
            };

            PrintInfoPanel();

            return _infoPanel;
        }

        private void PrintCharacterPanel()
        {
            var defaultCell = new Cell(_characterPanel.DefaultForeground, _characterPanel.DefaultBackground);
            _characterPanel.Clear();
            _characterPanel.Cursor.Position = new Point(0, 0);
            _characterPanel.Cursor.Print(new ColoredString($" {ColorHelper.GetParserString("Vede of Tattersail", Color.Gainsboro)}\r\n\n\n\n\n", defaultCell));
            var stats = _mapConsole.Player.GetGoRogueComponent<IActorStatComponent>();
            _characterPanel.Cursor.Print(new ColoredString($" {ColorHelper.GetParserString($"Walk speed: {stats.WalkSpeed:f2}", Color.Gainsboro)}\r\n", defaultCell));
            _characterPanel.Cursor.Print(new ColoredString($" {ColorHelper.GetParserString($"Attack speed: {stats.AttackSpeed:f2}", Color.Gainsboro)}\r\n", defaultCell));
            _characterPanel.Cursor.Print(new ColoredString($" {ColorHelper.GetParserString($"Cast speed: {stats.CastSpeed:f2}", Color.Gainsboro)}\r\n", defaultCell));
        }

        private void PrintInfoPanel()
        {
            var defaultCell = new Cell(_infoPanel.DefaultForeground, _infoPanel.DefaultBackground);
            _infoPanel.Clear();
            _infoPanel.Cursor.Position = new Point(0, 0);
            _infoPanel.Cursor.Print(new ColoredString($" {ColorHelper.GetParserString("Old Alward's Tower", Color.DarkGray)}\r\n", defaultCell));
            _infoPanel.Cursor.Print(new ColoredString($" {ColorHelper.GetParserString("Material Plane, Ayen", Color.DarkGray)}\r\n\n", defaultCell));
            var timeString = $"Time: {_dungeonMaster.TimeMaster.JourneyTime.Seconds}";
            _infoPanel.Cursor.Print(new ColoredString($" {ColorHelper.GetParserString(timeString, Color.DarkGray)}\r\n", defaultCell));
        }

        private void PrintStatOverlays(IHealthComponent healthComponent, IEndowmentPoolComponent endowmentComponent)
        {
            _statOverlay.Clear();
            _statOverlay.Cursor.Position = new Point(0, 0);

            var overlayDefaultCell = new Cell(_statOverlay.DefaultForeground, _statOverlay.DefaultBackground);
            var healthString = $"{healthComponent.Health} / {healthComponent.MaxHealth}";
            _statOverlay.Cursor.Print(new ColoredString($" {ColorHelper.GetParserString(healthString, Color.Gainsboro)}\r\n", overlayDefaultCell));
            var endowmentString = $"{endowmentComponent.Value} / {endowmentComponent.MaxValue}";
            _statOverlay.Cursor.Print(new ColoredString($" {ColorHelper.GetParserString(endowmentString, Color.Gainsboro)}\r\n", overlayDefaultCell));
        }

        private void Player_EndowmentChanged(object sender, float e)
        {
            var healthComponent = _mapConsole.Player.GetGoRogueComponent<IHealthComponent>();
            var endowmentComponent = _mapConsole.Player.GetGoRogueComponent<IEndowmentPoolComponent>();

            _endowmentBar.Progress = endowmentComponent.Value / endowmentComponent.MaxValue;

            PrintStatOverlays(healthComponent, endowmentComponent);
        }

        private void Player_HealthChanged(object sender, float e)
        {
            var healthComponent = _mapConsole.Player.GetGoRogueComponent<IHealthComponent>();
            var endowmentComponent = _mapConsole.Player.GetGoRogueComponent<IEndowmentPoolComponent>();

            _healthBar.Progress = healthComponent.Health / healthComponent.MaxHealth;

            PrintStatOverlays(healthComponent, endowmentComponent);
        }
    }
}
