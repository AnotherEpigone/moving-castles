using GoRogue.GameFramework;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;
using SadConsole;

namespace MovingCastles.Components
{
    public class OpenDoorComponent : IBumpTriggeredComponent
    {
        public const string OpenAnimationKey = "DoorOpen";
        public const string CloseAnimationKey = "DoorClose";

        public IGameObject Parent { get; set; }

        private readonly int _openGlyph;

        public OpenDoorComponent(int openGlyph)
        {
            _openGlyph = openGlyph;
        }

        public void Bump(McEntity bumpingEntity)
        {
            if (!(Parent is McEntity mcParent))
            {
                return;
            }

            ////var animation = new AnimatedConsole("default", 1, 1, mcParent.Font);
            ////animation.CreateFrame().SetGlyph(0, 0, _openGlyph, mcParent.GetForeground(0, 0), mcParent.GetBackground(0, 0));
            ////mcParent.Animations.Clear();
            ////mcParent.Animations.Add("default", animation);
            ////mcParent.Animation = mcParent.Animations["default"];

            mcParent.Animation = mcParent.Animations[OpenAnimationKey];
            mcParent.IsWalkable = true;
            mcParent.IsDirty = true;
        }
    }
}
