using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.Components.Stats;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Time;
using MovingCastles.GameSystems.TurnBased;
using MovingCastles.Maps;
using MovingCastles.Serialization.Settings;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;
using System.Diagnostics;
using System.Linq;

namespace MovingCastles.Ui.Consoles
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class MainConsole : ContainerConsole, System.IDisposable
    {
        private const int InfoPanelHeight = 8;
        private const int EquipmentPanelHeight = 25;

        private readonly IDungeonMaster _dungeonMaster;

        private ProgressBar _healthBar;
        private ProgressBar _endowmentBar;
        private Console _statOverlay;
        private Console _infoPanel;
        private Console _rightPanel;
        private Console _equipmentPanel;
        private InventoryConsole _inventoryPanel;
        private int _leftPaneWidth;
        private int _rightPaneWidth;
        private bool disposedValue;

        public MainConsole(
            int width,
            int height,
            Font tilesetFont,
            IMapModeMenuProvider menuProvider,
            IDungeonMaster dungeonMaster,
            ILogManager logManager,
            ITurnBasedGame game,
            IAppSettings appSettings,
            ITurnBasedGameConsoleFactory gameConsoleFactory,
            IUiManager uiManager)
        {
            UseMouse = false;

            _dungeonMaster = dungeonMaster;

            _leftPaneWidth = uiManager.GetSidePanelWidth();
            _rightPaneWidth = uiManager.GetSidePanelWidth();

            var middleSectionWidth = width - _leftPaneWidth - _rightPaneWidth;

            var tileSizeXFactor = (double)tilesetFont.Size.X / Font.Size.X;
            var tileSizeYFactor = (double)tilesetFont.Size.Y / Font.Size.Y;
            MapConsole = gameConsoleFactory.Create(
                (int)(_leftPaneWidth / tileSizeXFactor),
                (int)(UiManager.TopPaneHeight / tileSizeYFactor),
                (int)(middleSectionWidth / tileSizeXFactor),
                (int)((height - UiManager.TopPaneHeight) / tileSizeYFactor),
                tilesetFont,
                menuProvider,
                game,
                appSettings,
                _dungeonMaster.LevelMaster.Level.Map);

            CreateInfoPanel();
            CreateCharacterPanel(width, height);
            CreateEquipmentPanel(menuProvider);

            var centralWindowSize = uiManager.GetCentralWindowSize();
            _inventoryPanel = new InventoryConsole(
                centralWindowSize.X,
                centralWindowSize.Y,
                _dungeonMaster,
                logManager)
            {
                Position = new Coord(_leftPaneWidth, UiManager.TopPaneHeight + 1),
            };
            menuProvider.SetInventoryPanel(_inventoryPanel);

            var combatEventLog = new MessageLogConsole(
                _leftPaneWidth,
                (height - InfoPanelHeight) / 2,
                Global.FontDefault,
                Color.Transparent,
                Color.Black,
                System.TimeSpan.FromSeconds(5))
            {
                Position = new Point(_leftPaneWidth, UiManager.TopPaneHeight),
            };
            logManager.RegisterEventListener(LogType.Combat, (s, h) => combatEventLog.Add(s, h));
            var storyEventLog = new MessageLogConsole(_leftPaneWidth, height - InfoPanelHeight - EquipmentPanelHeight, Global.FontDefault)
            {
                Position = new Point(0, InfoPanelHeight + EquipmentPanelHeight),
            };
            logManager.RegisterEventListener(LogType.Story, (s, h) => storyEventLog.Add(s, h));

            _dungeonMaster.TimeMaster.TimeUpdated += TimeMaster_TimeUpdated;

            Children.Add(MapConsole.ThisConsole);
            Children.Add(CreateTopPane(middleSectionWidth, menuProvider));
            Children.Add(combatEventLog);
            Children.Add(storyEventLog);
            Children.Add(_infoPanel);
            Children.Add(_equipmentPanel);
            Children.Add(_inventoryPanel);
            Children.Add(_rightPanel);
        }

        public void SetMap(McMap map)
        {
            var healthComponent = MapConsole.Player.GetGoRogueComponent<IHealthComponent>();
            healthComponent.HealthChanged -= Player_HealthChanged;

            MapConsole.SetMap(map, _dungeonMaster.ModeMaster.Font);

            healthComponent = MapConsole.Player.GetGoRogueComponent<IHealthComponent>();
            healthComponent.HealthChanged += Player_HealthChanged;
        }

        public void UnsetMap()
        {
            MapConsole.UnsetMap();
        }

        private void CreateEquipmentPanel(IMapModeMenuProvider menuProvider)
        {
            _equipmentPanel = new Console(_leftPaneWidth, EquipmentPanelHeight)
            {
                Position = new Point(0, InfoPanelHeight),
                DefaultBackground = ColorHelper.ControlBackDark,
            };

            var inventoryComponent = MapConsole.Player.GetGoRogueComponent<IInventoryComponent>();
            var equipmentComponent = MapConsole.Player.GetGoRogueComponent<IEquipmentComponent>();
            const string inventoryMenuText = "Inventory (I):";
            var inventoryMenuButtonWidth = inventoryMenuText.Length;
            var inventoryMenuButton = new Button(inventoryMenuButtonWidth)
            {
                Text = inventoryMenuText,
                Position = new Point(1, 24),
            };
            inventoryMenuButton.Click += (_, __) =>
            {
                menuProvider.ShowInventoryPanel(inventoryComponent, equipmentComponent);
            };
            var buttonTheme = (ButtonTheme)inventoryMenuButton.Theme;
            buttonTheme.ShowEnds = false;
            inventoryMenuButton.Theme = buttonTheme;

            var controlPanel = new ControlsConsole(_equipmentPanel.Width, _equipmentPanel.Height)
            {
                ThemeColors = ColorHelper.GetThemeColorsForBackgroundColor(Color.Transparent)
            };

            controlPanel.Add(inventoryMenuButton);

            _equipmentPanel.Children.Add(controlPanel);

            PrintEquipmentPanel(inventoryComponent, equipmentComponent);
            inventoryComponent.ContentsChanged += (_, __) => PrintEquipmentPanel(inventoryComponent, equipmentComponent);
            equipmentComponent.EquipmentChanged += (_, __) => PrintEquipmentPanel(inventoryComponent, equipmentComponent);
        }

        private ControlsConsole CreateTopPane(
            int rightSectionWidth,
            IMapModeMenuProvider menuProvider)
        {
            var console = new ControlsConsole(rightSectionWidth, UiManager.TopPaneHeight)
            {
                Position = new Point(_leftPaneWidth, 0),
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

            const string spellMenuText = "Spells (S)";
            var spellMenuButtonWidth = spellMenuText.Length + 4;
            var spellMenuButton = new Button(spellMenuButtonWidth)
            {
                Text = spellMenuText,
                Position = new Point((rightSectionWidth * 2 / 5) - (spellMenuButtonWidth / 2), 0),
            };
            spellMenuButton.Click += (_, __) =>
            {
                menuProvider.SpellSelect.Show(
                    MapConsole.Player.GetGoRogueComponent<ISpellCastingComponent>().Spells,
                    selectedSpell => MapConsole.StartTargetting(selectedSpell),
                    MapConsole.Player.GetGoRogueComponent<IEndowmentPoolComponent>().Value);
            };

            const string journalMenuText = "Journal (J)";
            var journalMenuButtonWidth = journalMenuText.Length + 4;
            var journalMenuButton = new Button(journalMenuButtonWidth)
            {
                Text = journalMenuText,
                Position = new Point((rightSectionWidth * 3 / 5) - (journalMenuButtonWidth / 2), 0),
            };
            journalMenuButton.Click += (_, __) =>
            {
                menuProvider.Journal.Show(MapConsole.Player.JournalEntries);
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
            MapConsole.FlavorMessageChanged += (_, message) =>
            {
                flavorMessage.Clear();
                flavorMessage.Cursor.Position = new Point(0, 0);

                var coloredMessage = new ColoredString(message, new Cell(Color.Gainsboro, MapConsole.DefaultBackground));
                flavorMessage.Cursor.Print(coloredMessage);
            };

            console.Add(popupMenuButton);
            console.Add(spellMenuButton);
            console.Add(commandMenuButton);
            console.Add(journalMenuButton);
            console.Children.Add(flavorMessage);

            return console;
        }

        private Console CreateCharacterPanel(int viewportWidth, int viewportHeight)
        {
            _rightPanel = new Console(_rightPaneWidth, viewportHeight)
            {
                DefaultBackground = ColorHelper.ControlBack,
                Position = new Point(viewportWidth - _rightPaneWidth, 0),
            };

            PrintCharacterPanel();

            // stat bar panel setup
            var controlPanel = new ControlsConsole(_leftPaneWidth, 2)
            {
                Position = new Point(0, 2),
            };
            _endowmentBar = new ProgressBar(_leftPaneWidth, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 1),
            };
            _endowmentBar.ThemeColors = ColorHelper.GetProgressBarThemeColors(ColorHelper.DepletedManaBlue, ColorHelper.ManaBlue);

            var endowmentComponent = MapConsole.Player.GetGoRogueComponent<IEndowmentPoolComponent>();
            endowmentComponent.ValueChanged += Player_EndowmentChanged;
            _endowmentBar.Progress = endowmentComponent.Value / endowmentComponent.MaxValue;

            _healthBar = new ProgressBar(_leftPaneWidth, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 0),
            };
            _healthBar.ThemeColors = ColorHelper.GetProgressBarThemeColors(ColorHelper.DepletedHealthRed, ColorHelper.HealthRed);

            var healthComponent = MapConsole.Player.GetGoRogueComponent<IHealthComponent>();
            healthComponent.HealthChanged += Player_HealthChanged;
            _healthBar.Progress = healthComponent.Health / healthComponent.MaxHealth;

            controlPanel.Add(_endowmentBar);
            controlPanel.Add(_healthBar);

            // stat overlay setup
            _statOverlay = new Console(_leftPaneWidth, InfoPanelHeight)
            {
                Position = controlPanel.Position,
                DefaultBackground = Color.Transparent,
            };

            PrintStatOverlays(healthComponent, endowmentComponent);

            _rightPanel.Children.Add(controlPanel);
            _rightPanel.Children.Add(_statOverlay);

            return _rightPanel;
        }

        private Console CreateInfoPanel()
        {
            // base info panel
            _infoPanel = new Console(_leftPaneWidth, InfoPanelHeight)
            {
                DefaultBackground = ColorHelper.ControlBack,
            };

            PrintInfoPanel();

            return _infoPanel;
        }

        private void PrintCharacterPanel()
        {
            var defaultCell = new Cell(_rightPanel.DefaultForeground, _rightPanel.DefaultBackground);
            _rightPanel.Clear();
            _rightPanel.Cursor.Position = new Point(0, 0);
            _rightPanel.Cursor.Print(new ColoredString($" {ColorHelper.GetParserString("Vede of Tattersail", Color.Gainsboro)}\r\n\n\n\n\n", defaultCell));
            _rightPanel.Cursor.Print(new ColoredString($" {ColorHelper.GetParserString($"Walk speed: {TimeHelper.GetWalkSpeed(MapConsole.Player):f2}", Color.Gainsboro)}\r\n", defaultCell));
            _rightPanel.Cursor.Print(new ColoredString($" {ColorHelper.GetParserString($"Attack speed: {TimeHelper.GetAttackSpeed(MapConsole.Player):f2}", Color.Gainsboro)}\r\n", defaultCell));
            _rightPanel.Cursor.Print(new ColoredString($" {ColorHelper.GetParserString($"Cast speed: {TimeHelper.GetCastSpeed(MapConsole.Player):f2}", Color.Gainsboro)}\r\n", defaultCell));
        }

        private void PrintEquipmentPanel(IInventoryComponent inventory, IEquipmentComponent equipment)
        {
            var defaultCell = new Cell(_equipmentPanel.DefaultForeground, _equipmentPanel.DefaultBackground);
            _equipmentPanel.Clear();
            _equipmentPanel.Cursor.Position = new Point(0, 0);
            _equipmentPanel.Cursor.Print("\n");

            var categories = equipment.Equipment.Values.ToList();
            foreach (var category in categories)
            {
                var itemName = category.Items.FirstOrDefault()?.ColoredName ?? string.Empty;
                _equipmentPanel.Cursor.Print(new ColoredString($" {ColorHelper.GetParserString($"{category.Name}:", Color.DarkGray)} {itemName}\r\n", defaultCell));
            }

            _equipmentPanel.Cursor.Position = new Point(0, 24);
            _equipmentPanel.Cursor.Print(new ColoredString($"                {inventory.FilledCapacity}/{inventory.Capacity}", defaultCell));
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
            var healthComponent = MapConsole.Player.GetGoRogueComponent<IHealthComponent>();
            var endowmentComponent = MapConsole.Player.GetGoRogueComponent<IEndowmentPoolComponent>();

            _endowmentBar.Progress = endowmentComponent.Value / endowmentComponent.MaxValue;

            PrintStatOverlays(healthComponent, endowmentComponent);
        }

        private void Player_HealthChanged(object sender, float e)
        {
            var healthComponent = MapConsole.Player.GetGoRogueComponent<IHealthComponent>();
            var endowmentComponent = MapConsole.Player.GetGoRogueComponent<IEndowmentPoolComponent>();

            _healthBar.Progress = healthComponent.Health / healthComponent.MaxHealth;

            PrintStatOverlays(healthComponent, endowmentComponent);
        }

        private void TimeMaster_TimeUpdated(object sender, McTimeSpan args)
        {
            PrintInfoPanel();
            PrintCharacterPanel();
        }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(MainConsole)} ({Position.X}, {Position.Y})");
            }
        }

        public ITurnBasedGameConsole MapConsole { get; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dungeonMaster.TimeMaster.TimeUpdated -= TimeMaster_TimeUpdated;

                    var endowmentComponent = MapConsole.Player.GetGoRogueComponent<IEndowmentPoolComponent>();
                    endowmentComponent.ValueChanged -= Player_EndowmentChanged;

                    var healthComponent = MapConsole.Player.GetGoRogueComponent<IHealthComponent>();
                    healthComponent.HealthChanged -= Player_HealthChanged;

                    MapConsole.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
    }
}
