using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.Components.AiComponents;
using MovingCastles.Fonts;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Player;
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

        public McEntity CreateActor(Coord position, ActorTemplate actorTemplate)
        {
            var actor = new McEntity(
                actorTemplate.Name,
                Color.White,
                Color.Transparent,
                actorTemplate.Glyph,
                position,
                (int)Maps.DungeonMapLayer.MONSTERS,
                isWalkable: false,
                isTransparent: true,
                actorTemplate.NameColor);

            actorTemplate.CreateComponents().ForEach(c => actor.AddGoRogueComponent(c));
            actor.AddGoRogueComponent(new SummaryControlComponent());

            // workaround Entity construction bugs by setting font afterward
            actor.Font = _font;
            actor.OnCalculateRenderPosition();

            return actor;
        }

        public McEntity CreateItem(Coord position, ItemTemplate itemTemplate)
        {
            var item = new McEntity(
                    itemTemplate.Name,
                    Color.White,
                    Color.Transparent,
                    itemTemplate.Glyph,
                    position,
                    (int)Maps.DungeonMapLayer.ITEMS,
                    isWalkable: true,
                    isTransparent: true,
                    Color.DarkGray);
            item.AddGoRogueComponent(new SummaryControlComponent());
            item.AddGoRogueComponent(new PickupableComponent(
                _logManager,
                itemTemplate));

            // workaround Entity construction bugs by setting font afterward
            item.Font = _font;
            item.OnCalculateRenderPosition();

            return item;
        }

        public McEntity CreateDoor(Coord position)
        {
            var door = new McEntity(
                    "Door",
                    Color.White,
                    Color.Transparent,
                    SpriteAtlas.Door_Wood_Closed,
                    position,
                    (int)Maps.DungeonMapLayer.ITEMS,
                    isWalkable: false,
                    isTransparent: true,
                    Color.SaddleBrown);

            // door open/close animations
            var closedAnimation = new AnimatedConsole(OpenDoorComponent.CloseAnimationKey, 1, 1, _font);
            closedAnimation.CreateFrame().SetGlyph(0, 0, SpriteAtlas.Door_Wood_Closed, Color.White, Color.Transparent);
            var openAnimation = new AnimatedConsole(OpenDoorComponent.OpenAnimationKey, 1, 1, _font);
            openAnimation.CreateFrame().SetGlyph(0, 0, SpriteAtlas.Door_Wood_Open, Color.White, Color.Transparent);
            door.Animations.Clear();
            door.Animations.Add(OpenDoorComponent.CloseAnimationKey, closedAnimation);
            door.Animations.Add(OpenDoorComponent.OpenAnimationKey, openAnimation);
            door.Animation = door.Animations[OpenDoorComponent.CloseAnimationKey];

            door.AddGoRogueComponent(new OpenDoorComponent(SpriteAtlas.Door_Wood_Open));

            // workaround Entity construction bugs by setting font afterward
            door.Font = _font;
            door.OnCalculateRenderPosition();

            return door;
        }

        public Wizard CreatePlayer(Coord position, Player playerInfo)
        {
            return new Wizard(position, playerInfo, _font);
        }

        public Castle CreateCastle(Coord position, Player playerInfo)
        {
            return new Castle(position, playerInfo, _font);
        }
    }
}
