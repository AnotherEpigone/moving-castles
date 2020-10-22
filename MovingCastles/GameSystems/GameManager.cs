using MovingCastles.Entities;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using MovingCastles.Ui;
using SadConsole;
using System.Linq;

namespace MovingCastles.GameSystems
{
    public class GameManager : IGameManager
    {
        private readonly IUiManager _uiManager;
        private readonly IItemTemplateLoader _itemLoader;
        private readonly IMapTemplateLoader _mapLoader;
        private readonly IMapPlanFactory _mapPlanFactory;
        private readonly ILogManager _logManager;

        public GameManager(
            IUiManager uiManager,
            IItemTemplateLoader itemLoader,
            IMapTemplateLoader mapLoader,
            IMapPlanFactory mapPlanFactory,
            ILogManager logManager)
        {
            _uiManager = uiManager;
            _itemLoader = itemLoader;
            _mapLoader = mapLoader;
            _mapPlanFactory = mapPlanFactory;
            _logManager = logManager;
        }

        public void StartDungeonModeDemo()
        {
            var items = _itemLoader.Load();

            var mapTemplates = _mapLoader.Load();
            var maps = mapTemplates.ToDictionary(t => t.Key, t => _mapPlanFactory.Create(t.Value, items));
            var dungeonModeDemoMapPlan = maps["MAP_TESTAREA"];

            var tilesetFont = Global.Fonts[UiManager.TilesetFontName].GetFont(Font.FontSizes.One);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);
            var mapFactory = new MapFactory(entityFactory);

            var player = Player.Player.CreateDefault();

            var dungeonModeDemoMap = mapFactory.CreateDungeonMap(100, 60, dungeonModeDemoMapPlan, player);

            Global.CurrentScreen = _uiManager.CreateDungeonMapScreen(this, dungeonModeDemoMap, tilesetFont);
        }

        public void StartCastleModeDemo()
        {
            var items = _itemLoader.Load();

            var mapTemplates = _mapLoader.Load();
            var maps = mapTemplates.ToDictionary(t => t.Key, t => _mapPlanFactory.Create(t.Value, items));
            var dummyMapPlan = maps["MAP_TESTAREA"];

            var tilesetFont = Global.Fonts[UiManager.TilesetFontName].GetFont(Font.FontSizes.Three);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);
            var mapFactory = new MapFactory(entityFactory);

            var player = Player.Player.CreateDefault();

            var dungeonModeDemoMap = mapFactory.CreateCastleMap(100, 60, dummyMapPlan, player);

            Global.CurrentScreen = _uiManager.CreateCastleMapScreen(this, dungeonModeDemoMap, tilesetFont);
        }

        public void StartMapGenDemo()
        {
            throw new System.NotImplementedException();
        }
    }
}
