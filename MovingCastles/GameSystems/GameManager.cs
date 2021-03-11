using GoRogue;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Journal.Entries;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Player;
using MovingCastles.GameSystems.Saving;
using MovingCastles.GameSystems.Time;
using MovingCastles.GameSystems.TurnBased;
using MovingCastles.Maps;
using MovingCastles.Serialization.Map;
using MovingCastles.Ui;
using MovingCastles.Ui.Consoles;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.GameSystems
{
    public class GameManager : IGameManager
    {
        private readonly IUiManager _uiManager;
        private readonly ILogManager _logManager;
        private readonly ISaveManager _saveManager;
        private readonly IStructureFactory _structureFactory;
        private IDungeonMaster _dungeonMaster;
        private readonly IDungeonMasterFactory _dungeonMasterFactory;

        public GameManager(
            IUiManager uiManager,
            ILogManager logManager,
            ISaveManager saveManager,
            IStructureFactory structureFactory,
            IDungeonMasterFactory dungeonMasterFactory)
        {
            _uiManager = uiManager;
            _logManager = logManager;
            _saveManager = saveManager;
            _structureFactory = structureFactory;
            _dungeonMasterFactory = dungeonMasterFactory;
        }

        public IDungeonMaster DungeonMaster
        {
            get { return _dungeonMaster; }
            private set
            {
                if (_dungeonMaster != null)
                {
                    _dungeonMaster.LevelMaster.LevelChanged -= DungeonMaster_LevelChanged;
                    _dungeonMaster.LevelMaster.LevelChanging -= DungeonMaster_LevelChanging;
                }

                _dungeonMaster = value;
                _dungeonMaster.LevelMaster.LevelChanged += DungeonMaster_LevelChanged;
                _dungeonMaster.LevelMaster.LevelChanging += DungeonMaster_LevelChanging;
            }
        }

        public void StartNewGame()
        {
            var tilesetFont = Global.Fonts[UiManager.TilesetFontName].GetFont(Font.FontSizes.One);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);

            var playerTemplate = new WizardTemplate();
            playerTemplate.JournalEntries.Add(AlwardsTowerJournalEntries.Quest(new McTimeSpan(-1)));
            playerTemplate.JournalEntries.Add(AlwardsTowerJournalEntries.FirstEntrance(new McTimeSpan(0)));

            var player = entityFactory.CreatePlayer(Coord.NONE, playerTemplate);

            var structure = _structureFactory.CreateById(Structure.StructureId_AlwardsTower, entityFactory);
            var level = structure.GetLevel(LevelId.AlwardsTower1, player, new SpawnConditions(Spawn.Default, 0));

            DungeonMaster = _dungeonMasterFactory.Create(player, level, structure);

            var game = new TurnBasedGame(_logManager, DungeonMaster);
            var gameConsoleFactory = new DungeonMapConsoleFactory();

            Global.CurrentScreen = _uiManager.CreateDungeonMapScreen(this, game, gameConsoleFactory, DungeonMaster, tilesetFont);
        }

        public void Save()
        {
            if (DungeonMaster == null)
            {
                return;
            }

            var structure = DungeonMaster.LevelMaster.Structure;
            var wizard = DungeonMaster.LevelMaster.Level.Map.Entities.Items.OfType<Wizard>().Single();
            var mapState = new MapState(structure, DungeonMaster.LevelMaster.Level);
            var knownMaps = structure.SerializedLevels.Values
                .Concat(structure.GeneratedLevels
                    .Select(g => new MapState(structure, g.Value)))
                .ToArray();

            var save = new Save()
            {
                KnownMaps = knownMaps,
                MapState = mapState,
                Wizard = wizard,
                TimeMaster = (TimeMaster)DungeonMaster.TimeMaster,
            };
            _saveManager.Write(save);
        }

        public bool CanLoad()
        {
            return _saveManager.SaveExists();
        }

        public void Load()
        {
            var (success, save) = _saveManager.Read();
            if (!success)
            {
                throw new System.IO.IOException("Failed to load save file.");
            }

            var tilesetFont = Global.Fonts[UiManager.TilesetFontName].GetFont(Font.FontSizes.One);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);

            var playerTemplate = new WizardTemplate();
            var player = entityFactory.CreatePlayer(Coord.NONE, playerTemplate);

            var structure = _structureFactory.CreateById(save.MapState.StructureId, entityFactory);
            var level = structure.GetLevel(save);

            DungeonMaster = _dungeonMasterFactory.Create(player, level, structure, save.TimeMaster);

            var game = new TurnBasedGame(_logManager, DungeonMaster);
            var gameConsoleFactory = new DungeonMapConsoleFactory();

            Global.CurrentScreen = _uiManager.CreateDungeonMapScreen(this, game, gameConsoleFactory, DungeonMaster, tilesetFont);
        }

        public void StartDungeonModeDemo()
        {
        }

        public void StartCastleModeDemo()
        {
            var tilesetFont = Global.Fonts[UiManager.TilesetFontName].GetFont(Font.FontSizes.Three);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);
            var mapFactory = new MapFactory(entityFactory);

            var playerTemplate = new WizardTemplate();
            var player = entityFactory.CreatePlayer(Coord.NONE, playerTemplate);

            var dungeonModeDemoMap = mapFactory.CreateCastleMap(100, 60, null, player);

            Global.CurrentScreen = _uiManager.CreateCastleMapScreen(this, dungeonModeDemoMap, tilesetFont);
        }

        public void StartMapGenDemo()
        {
            var tilesetFont = Global.Fonts[UiManager.TilesetFontName].GetFont(Font.FontSizes.One);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);

            var playerTemplate = new WizardTemplate();
            var player = entityFactory.CreatePlayer(Coord.NONE, playerTemplate);

            var structure = _structureFactory.CreateById(Structure.StructureId_MapgenDemo, entityFactory);
            var level = structure.GetLevel(LevelId.MapgenTest, player, new SpawnConditions(Spawn.Default, 0));

            DungeonMaster = _dungeonMasterFactory.Create(player, level, structure);

            var game = new TurnBasedGame(_logManager, DungeonMaster);
            var gameConsoleFactory = new DungeonMapConsoleFactory();

            Global.CurrentScreen = _uiManager.CreateDungeonMapScreen(this, game, gameConsoleFactory, DungeonMaster, tilesetFont);
        }

        private void DungeonMaster_LevelChanged(object sender, EventArgs args)
        {
            ((MainConsole)Global.CurrentScreen).SetMap(DungeonMaster.LevelMaster.Level.Map);
        }

        private void DungeonMaster_LevelChanging(object sender, EventArgs args)
        {
            ((MainConsole)Global.CurrentScreen).UnsetMap();
        }
    }
}
