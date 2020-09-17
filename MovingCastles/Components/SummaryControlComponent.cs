using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using Microsoft.Xna.Framework;
using MovingCastles.Ui;
using SadConsole;
using SadConsole.Controls;

namespace MovingCastles.Components
{
    public class SummaryControlComponent : ISummaryControlComponent, IGameObjectComponent
    {
        public IGameObject Parent { get; set; }

        public Console GetSidebarSummary()
        {
            if (!(Parent is BasicEntity parentEntity))
            {
                return null;
            }

            var nameLabel = new Label(parentEntity.Name) { Position = new Point(1, 0), TextColor = Color.Gainsboro };
            var controlsHeight = 1;

            var sidebarConsole = new ControlsConsole(30, controlsHeight);
            sidebarConsole.Add(nameLabel);
            ////var healthBar = new ProgressBar(30, 1, HorizontalAlignment.Left)
            ////{
            ////    Position = new Point(0, 1),
            ////};
            ////healthBar.ThemeColors = ColorHelper.GetProgressBarThemeColors(Color.White, Color.Red);
            ////healthBar.Progress = 1;

            return sidebarConsole;
        }
    }
}
