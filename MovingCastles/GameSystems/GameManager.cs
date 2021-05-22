using GoRogue;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Journal.Entries;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Player;
using MovingCastles.GameSystems.Saving;
using MovingCastles.GameSystems.Time;
using MovingCastles.GameSystems.TurnBased;
using MovingCastles.Serialization.Map;
using MovingCastles.Ui;
using MovingCastles.Ui.Consoles;
using SadConsole;
using System;
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
            var gameModeMaster = new GameModeMaster(_logManager, GameMode.Dungeon);

            var playerTemplate = new WizardTemplate();
            playerTemplate.JournalEntries.Add(AlwardsTowerJournalEntries.Quest(new McTimeSpan(-1)));
            playerTemplate.JournalEntries.Add(AlwardsTowerJournalEntries.FirstEntrance(new McTimeSpan(0)));

            var player = gameModeMaster.EntityFactory.CreatePlayer(Coord.NONE, playerTemplate);

            var structure = _structureFactory.CreateById(Structure.StructureId_AlwardsTower, gameModeMaster);
            var level = structure.GetLevel(LevelId.AlwardsTower1, player, new SpawnConditions(Spawn.Default, 0));

            DungeonMaster = _dungeonMasterFactory.Create(player, level, structure, gameModeMaster, _structureFactory, new TimeMaster(), _uiManager);

            gameModeMaster.ModeChanged += GameModeMaster_ModeChanged;

            var game = new TurnBasedGame(_logManager, DungeonMaster);
            Global.CurrentScreen = _uiManager.CreateMapScreen(this, game, gameModeMaster.GameConsoleFactory, DungeonMaster, gameModeMaster.Font);
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
                GameMode = DungeonMaster.ModeMaster.Mode,
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

            var gameModeMaster = new GameModeMaster(_logManager, save.GameMode);

            var player = save.Wizard;

            var structure = _structureFactory.CreateById(save.MapState.StructureId, gameModeMaster);
            var level = structure.GetLevel(save);

            DungeonMaster = _dungeonMasterFactory.Create(player, level, structure, gameModeMaster, _structureFactory, save.TimeMaster, _uiManager);
            gameModeMaster.ModeChanged += GameModeMaster_ModeChanged;

            var game = new TurnBasedGame(_logManager, DungeonMaster);
            Global.CurrentScreen = _uiManager.CreateMapScreen(this, game, gameModeMaster.GameConsoleFactory, DungeonMaster, gameModeMaster.Font);
        }

        public void StartCastleModeDemo()
        {
            var gameModeMaster = new GameModeMaster(_logManager, GameMode.Castle);

            var playerTemplate = new WizardTemplate();
            playerTemplate.JournalEntries.Add(AlwardsTowerJournalEntries.Quest(new McTimeSpan(-1)));

            var player = gameModeMaster.EntityFactory.CreatePlayer(Coord.NONE, playerTemplate);

            var structure = _structureFactory.CreateById(Structure.StructureId_SaraniDesert_Highlands, gameModeMaster);
            var level = structure.GetLevel(LevelId.SaraniHighlands, player, new SpawnConditions(Spawn.Default, 0));

            DungeonMaster = _dungeonMasterFactory.Create(player, level, structure, gameModeMaster, _structureFactory, new TimeMaster(), _uiManager);

            gameModeMaster.ModeChanged += GameModeMaster_ModeChanged;

            var game = new TurnBasedGame(_logManager, DungeonMaster);
            Global.CurrentScreen = _uiManager.CreateMapScreen(this, game, gameModeMaster.GameConsoleFactory, DungeonMaster, gameModeMaster.Font);
        }

        public void StartMapGenDemo()
        {
            var gameModeMaster = new GameModeMaster(_logManager, GameMode.Dungeon);

            var playerTemplate = new WizardTemplate();
            var player = gameModeMaster.EntityFactory.CreatePlayer(Coord.NONE, playerTemplate);

            var structure = _structureFactory.CreateById(Structure.StructureId_MapgenDemo, gameModeMaster);
            var level = structure.GetLevel(LevelId.MapgenTest, player, new SpawnConditions(Spawn.Default, 0));

            DungeonMaster = _dungeonMasterFactory.Create(player, level, structure, gameModeMaster, _structureFactory, new TimeMaster(), _uiManager);

            gameModeMaster.ModeChanged += GameModeMaster_ModeChanged;

            var game = new TurnBasedGame(_logManager, DungeonMaster);
            Global.CurrentScreen = _uiManager.CreateMapScreen(this, game, gameModeMaster.GameConsoleFactory, DungeonMaster, gameModeMaster.Font);
        }

        private void DungeonMaster_LevelChanged(object sender, EventArgs args)
        {
            ((MainConsole)Global.CurrentScreen).SetMap(DungeonMaster.LevelMaster.Level.Map);
        }

        private void DungeonMaster_LevelChanging(object sender, EventArgs args)
        {
            ((MainConsole)Global.CurrentScreen).UnsetMap();
        }

        private void GameModeMaster_ModeChanged(object sender, EventArgs e)
        {
            var oldConsole = (MainConsole)Global.CurrentScreen;
            oldConsole.IsPaused = true;
            oldConsole.Dispose();

            DungeonMaster.Player.Font = DungeonMaster.ModeMaster.Font;
            DungeonMaster.Player.OnCalculateRenderPosition();

            var game = new TurnBasedGame(_logManager, DungeonMaster);
            Global.CurrentScreen = _uiManager.CreateMapScreen(
                                        this,
                                        game,
                                        DungeonMaster.ModeMaster.GameConsoleFactory,
                                        DungeonMaster,
                                        DungeonMaster.ModeMaster.Font);
        }
    }
}
