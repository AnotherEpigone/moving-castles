using MovingCastles.GameSystems.Items;
using MovingCastles.Maps;
using MovingCastles.Ui;
using MovingCastles.Ui.Consoles;
using SadConsole;
using System.Linq;

namespace MovingCastles.GameSystems
{
    public class GameManager : IGameManager
    {
        private readonly IUiManager _uiManager;
        private readonly IItemTemplateLoader _itemLoader;
        private readonly IMapTemplateLoader _mapLoader;

        public GameManager(
            IUiManager uiManager,
            IItemTemplateLoader itemLoader,
            IMapTemplateLoader mapLoader)
        {
            _uiManager = uiManager;
            _itemLoader = itemLoader;
            _mapLoader = mapLoader;
        }

        public void StartDungeonModeDemo()
        {
            var items = _itemLoader.Load();

            var mapTemplates = _mapLoader.Load();
            var mapPlanFactory = new MapPlanFactory();
            var maps = mapTemplates.ToDictionary(t => t.Key, t => mapPlanFactory.Create(t.Value, items));

            Global.CurrentScreen = _uiManager.CreateDungeonMapScreen(maps["MAP_TESTAREA"], this);
        }

        public void StartCastleModeDemo()
        {
            var items = _itemLoader.Load();

            var mapTemplates = _mapLoader.Load();
            var mapPlanFactory = new MapPlanFactory();
            var maps = mapTemplates.ToDictionary(t => t.Key, t => mapPlanFactory.Create(t.Value, items));

            Global.CurrentScreen = _uiManager.CreateCastleMapScreen(maps["MAP_TESTAREA"], this);
        }
    }
}
