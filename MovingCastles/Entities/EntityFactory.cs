using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Logging;
using SadConsole;

namespace MovingCastles.Entities
{
    public class EntityFactory : IEntityFactory
    {
        private readonly Font _font;
        private readonly ILogManager _logManager;

        public EntityFactory(Font font, ILogManager logManager)
        {
            _font = font;
            _logManager = logManager;
        }

        public BasicEntity CreateActor(int glyph, Coord position, string name)
        {
            var actor = new BasicEntity(Color.White, Color.Transparent, glyph, position, (int)Maps.MapLayer.MONSTERS, isWalkable: false, isTransparent: true)
            {
                Name = name,
            };

            // workaround Entity construction bugs by setting font afterward
            actor.Font = _font;
            actor.OnCalculateRenderPosition();

            return actor;
        }

        public BasicEntity CreateItem(int glyph, Coord position, string name, string desc)
        {
            var item = new BasicEntity(
                    Color.White,
                    Color.Transparent,
                    glyph,
                    position,
                    (int)Maps.MapLayer.ITEMS,
                    isWalkable: true,
                    isTransparent: true)
            {
                Name = name,
            };
            item.AddGoRogueComponent(new PickupableComponent(
                _logManager,
                new InventoryItem(name, desc)));

            // workaround Entity construction bugs by setting font afterward
            item.Font = _font;
            item.OnCalculateRenderPosition();

            return item;
        }
    }
}
