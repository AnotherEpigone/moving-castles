using MovingCastles.Entities;
using MovingCastles.GameSystems.Levels.Generators;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using MovingCastles.Ui;
using SadConsole;

namespace MovingCastles.GameSystems
{
    public class GameManager : IGameManager
    {
        private readonly IUiManager _uiManager;
        private readonly ILogManager _logManager;

        public GameManager(
            IUiManager uiManager,
            ILogManager logManager)
        {
            _uiManager = uiManager;
            _logManager = logManager;
        }

        public void StartNewGame()
        {
            var tilesetFont = Global.Fonts[UiManager.TilesetFontName].GetFont(Font.FontSizes.One);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);

            var player = Player.PlayerInfo.CreateDefault();

            var firstLevelGen = new AlwardsTowerLevelGenerator(entityFactory);
            var level = firstLevelGen.Generate(McRandom.GetSeed(), player);

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
