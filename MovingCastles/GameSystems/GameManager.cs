using MovingCastles.GameSystems.Items;
using MovingCastles.Maps;

namespace MovingCastles.GameSystems
{
    public class GameManager : IGameManager
    {
        private readonly IItemTemplateLoader _itemLoader;
        private readonly IMapTemplateLoader _mapLoader;

        public GameManager(
            IItemTemplateLoader itemLoader,
            IMapTemplateLoader mapLoader)
        {
            _itemLoader = itemLoader;
            _mapLoader = mapLoader;
        }

        public void Start()
        {
        }
    }
}
