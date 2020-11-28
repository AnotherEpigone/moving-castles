using MovingCastles.Entities;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Levels.Generators;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Saving;
using MovingCastles.Maps;
using MovingCastles.Serialization.Map;
using MovingCastles.Ui;
using SadConsole;
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

        public IDungeonMaster Dm { get; private set; }

        public void StartNewGame()
        {
            var tilesetFont = Global.Fonts[UiManager.TilesetFontName].GetFont(Font.FontSizes.One);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);

            var player = Player.PlayerInfo.CreateDefault();

            var structure = _structureFactory.CreateById(Structure.StructureId_AlwardsTower, entityFactory);
            var level = structure.GetLevel(LevelId.AlwardsTower1, player);

            Dm = new DungeonMaster(player)
            {
                Level = level,
                Structure = structure,
            };

            Global.CurrentScreen = _uiManager.CreateDungeonMapScreen(this, level.Map, tilesetFont);
        }

        public void Save()
        {
            if (Dm == null)
            {
                return;
            }

            var entities = Dm.Level.Map.Entities.Items.OfType<McEntity>().ToList();
            var wizard = entities.OfType<Wizard>().Single();
            var doors = entities.OfType<Door>().ToList();
            entities.Remove(wizard);
            foreach(var door in doors)
            {
                entities.Remove(door);
            }

            var mapState = new MapState()
            {
                Id = Dm.Level.Id,
                Seed = Dm.Level.Seed,
                Width = Dm.Level.Map.Width,
                Height = Dm.Level.Map.Height,
                Explored = Dm.Level.Map.Explored,
                Entities = entities,
                Doors = doors,
                StructureId = Dm.Structure.Id,
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

            Dm = new DungeonMaster(player)
            {
                Level = level,
                Structure = structure,
            };

            Global.CurrentScreen = _uiManager.CreateDungeonMapScreen(this, level.Map, tilesetFont);
        }

        public void StartDungeonModeDemo()
        {
            var tilesetFont = Global.Fonts[UiManager.TilesetFontName].GetFont(Font.FontSizes.One);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);
            var mapFactory = new MapFactory(entityFactory);

            var player = Player.PlayerInfo.CreateDefault();

            var dungeonModeDemoMap = mapFactory.CreateDungeonMap(100, 60, MapAtlas.CombatTestArea, player);

            Global.CurrentScreen = _uiManager.CreateDungeonMapScreen(this, dungeonModeDemoMap, tilesetFont);
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

            Global.CurrentScreen = _uiManager.CreateDungeonMapScreen(this, mapGenTestAreaMap, tilesetFont);
        }
    }
}
