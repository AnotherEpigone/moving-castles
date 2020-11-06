using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.Fonts;
using MovingCastles.Maps;
using SadConsole;

namespace MovingCastles.Entities
{
    public class Door : McEntity
    {
        private const string OpenAnimationKey = "DoorOpen";
        private const string ClosedAnimationKey = "DoorClose";

        private bool _open;

        public Door(Coord position, Font font)
            : base("Door",
                  Color.White,
                  Color.Transparent,
                  SpriteAtlas.Door_Wood_Closed,
                  position,
                  (int)DungeonMapLayer.ITEMS,
                  isWalkable: false,
                  isTransparent: false,
                  Color.SaddleBrown)
        {
            // door open/close animations
            var closedAnimation = new AnimatedConsole(ClosedAnimationKey, 1, 1, font);
            closedAnimation.CreateFrame().SetGlyph(0, 0, SpriteAtlas.Door_Wood_Closed, Color.White, Color.Transparent);
            var openAnimation = new AnimatedConsole(OpenAnimationKey, 1, 1, font);
            openAnimation.CreateFrame().SetGlyph(0, 0, SpriteAtlas.Door_Wood_Open, Color.White, Color.Transparent);
            Animations.Clear();
            Animations.Add(ClosedAnimationKey, closedAnimation);
            Animations.Add(OpenAnimationKey, openAnimation);
            Animation = Animations[ClosedAnimationKey];

            AddGoRogueComponent(new OpenDoorComponent());
            AddGoRogueComponent(new UseDoorComponent());

            // workaround Entity construction bugs by setting font afterward
            Font = font;
            OnCalculateRenderPosition();

            _open = false;
        }

        public void Open()
        {
            Animation = Animations[OpenAnimationKey];
            IsWalkable = true;
            IsTransparent = true;
            _open = true;
        }

        public void Toggle()
        {
            _open = !_open;
            Animation = _open
                ? Animations[OpenAnimationKey]
                : Animations[ClosedAnimationKey];
            IsWalkable = _open;
            IsTransparent = _open;
        }
    }
}
