using MovingCastles.Entities;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Saving;
using MovingCastles.GameSystems.TurnBased;
using MovingCastles.Maps;
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

        public GameManager(
            IUiManager uiManager,
            ILogManager logManager,
            ISaveManager saveManager,
            IStructureFactory structureFactory)
        {
            _uiManager = uiManager;
            _logManager = logManager;
            _saveManager = saveManager;
            _structureFactory = structureFactory;
        }

        public IDungeonMaster DungeonMaster
        {
            get { return _dungeonMaster; }
            private set
            {
                if (_dungeonMaster != null)
                {
                    _dungeonMaster.LevelChanged -= DungeonMaster_LevelChanged;
                }

                _dungeonMaster = value;
                _dungeonMaster.LevelChanged += DungeonMaster_LevelChanged;
            }
        }

        public void StartNewGame()
        {
            var tilesetFont = Global.Fonts[UiManager.TilesetFontName].GetFont(Font.FontSizes.One);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);

            var player = Player.PlayerInfo.CreateDefault();

            var structure = _structureFactory.CreateById(Structure.StructureId_AlwardsTower, entityFactory);
            var level = structure.GetLevel(LevelId.AlwardsTower1, player, new SpawnConditions(Spawn.Default, 0));

            DungeonMaster = new DungeonMaster(player, _structureFactory, entityFactory)
            {
                Level = level,
                Structure = structure,
            };

            var game = new TurnBasedGame(_logManager, DungeonMaster);
            Global.CurrentScreen = _uiManager.CreateDungeonMapScreen(this, game, level.Map, tilesetFont);
        }

        public void Save()
        {
            if (DungeonMaster == null)
            {
                return;
            }

            var entities = DungeonMaster.Level.Map.Entities.Items.OfType<McEntity>().ToList();
            var wizard = entities.OfType<Wizard>().Single();
            var doors = entities.OfType<Door>().ToList();
            entities.Remove(wizard);
            foreach(var door in doors)
            {
                entities.Remove(door);
            }

            var mapState = new MapState()
            {
                Id = DungeonMaster.Level.Id,
                Seed = DungeonMaster.Level.Seed,
                Width = DungeonMaster.Level.Map.Width,
                Height = DungeonMaster.Level.Map.Height,
                Explored = DungeonMaster.Level.Map.Explored,
                Entities = entities,
                Doors = doors,
                StructureId = DungeonMaster.Structure.Id,
            };

            var save = new Save()
            {
                MapState = mapState,
                Wizard = wizard,
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
                // TODO better error handling here
                return;
            }

            var tilesetFont = Global.Fonts[UiManager.TilesetFontName].GetFont(Font.FontSizes.One);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);

            var player = Player.PlayerInfo.CreateDefault();

            var structure = _structureFactory.CreateById(save.MapState.StructureId, entityFactory);
            var level = structure.GetLevel(save);

            DungeonMaster = new DungeonMaster(player, _structureFactory, entityFactory)
            {
                Level = level,
                Structure = structure,
            };

            var game = new TurnBasedGame(_logManager, DungeonMaster);
            Global.CurrentScreen = _uiManager.CreateDungeonMapScreen(this, game, level.Map, tilesetFont);
        }

        public void StartDungeonModeDemo()
        {
            var tilesetFont = Global.Fonts[UiManager.TilesetFontName].GetFont(Font.FontSizes.One);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);
            var mapFactory = new MapFactory(entityFactory);

            var player = Player.PlayerInfo.CreateDefault();

            var dungeonModeDemoMap = mapFactory.CreateDungeonMap(100, 60, MapAtlas.CombatTestArea, player);

            DungeonMaster = new DungeonMaster(player, _structureFactory, entityFactory);

            var game = new TurnBasedGame(_logManager, DungeonMaster);
            Global.CurrentScreen = _uiManager.CreateDungeonMapScreen(this, game, dungeonModeDemoMap, tilesetFont);
        }

        public void StartCastleModeDemo()
        {
            var tilesetFont = Global.Fonts[UiManager.TilesetFontName].GetFont(Font.FontSizes.Three);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);
            var mapFactory = new MapFactory(entityFactory);

            var player = Player.PlayerInfo.CreateDefault();

            var dungeonModeDemoMap = mapFactory.CreateCastleMap(100, 60, null, player);

            Global.CurrentScreen = _uiManager.CreateCastleMapScreen(this, dungeonModeDemoMap, tilesetFont);
        }

        public void StartMapGenDemo()
        {
            var tilesetFont = Global.Fonts[UiManager.TilesetFontName].GetFont(Font.FontSizes.One);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);
            var mapFactory = new MapFactory(entityFactory);

            var player = Player.PlayerInfo.CreateDefault();

            var mapGenTestAreaMap = mapFactory.CreateMapGenTestAreaMap(100, 60, null, player);

            DungeonMaster = new DungeonMaster(player, _structureFactory, entityFactory);

            var game = new TurnBasedGame(_logManager, DungeonMaster);
            Global.CurrentScreen = _uiManager.CreateDungeonMapScreen(this, game, mapGenTestAreaMap, tilesetFont);
        }

        private void DungeonMaster_LevelChanged(object sender, EventArgs args)
        {
            ((DungeonModeConsole)Global.CurrentScreen).SetMap(DungeonMaster.Level.Map);
        }
    }
}
