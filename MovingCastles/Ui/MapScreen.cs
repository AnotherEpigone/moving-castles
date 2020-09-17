using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using SadConsole;
using SadConsole.Controls;

namespace MovingCastles.Ui
{
    public class MapScreen : ContainerConsole
    {
        private const int leftPaneWidth = 30;
        private const int topPaneHeight = 3;
        private const int eventLogHeight = 4;

        public MapScreen(
            int width,
            int height,
            Font tilesetFont,
            IMenuProvider menuProvider,
            IEntityFactory entityFactory,
            ILogManager logManager)
        {
            var leftPane = new ControlsConsole(leftPaneWidth, height);
            var manaBar = new ProgressBar(30, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 4),
            };
            manaBar.ThemeColors = ColorHelper.GetProgressBarThemeColors(Color.White, UiManager.ManaBlue);
            manaBar.Progress = 1;
            leftPane.Add(manaBar);

            var healthBar = new ProgressBar(30, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 3),
            };
            healthBar.ThemeColors = ColorHelper.GetProgressBarThemeColors(Color.White, Color.Red);
            healthBar.Progress = 1;
            leftPane.Add(healthBar);
            leftPane.Add(new Label("Vede of Tattersail") { Position = new Point(1, 0), TextColor = Color.White });
            leftPane.Add(new Label("Material Plane, Ayen") { Position = new Point(1, 1), TextColor = Color.DarkGray });
            leftPane.Add(new Label("Old Alward's Tower") { Position = new Point(1, 2), TextColor = Color.DarkGray });

            var rightSectionWidth = width - leftPaneWidth;

            var topPane = new Console(rightSectionWidth, topPaneHeight);
            topPane.Position = new Point(leftPaneWidth, 0);

            var tileSizeXFactor = tilesetFont.Size.X / Global.FontDefault.Size.X;
            var mapConsole = new MapConsole(
                80,
                45,
                rightSectionWidth / tileSizeXFactor,
                height - eventLogHeight - topPaneHeight,
                tilesetFont,
                menuProvider,
                entityFactory)
            {
                Position = new Point(leftPaneWidth, topPaneHeight)
            };

            var eventLog = new MessageLog(width, eventLogHeight, Global.FontDefault);
            eventLog.Position = new Point(leftPaneWidth, mapConsole.MapRenderer.ViewPort.Height + topPaneHeight);
            logManager.RegisterEventListener(s => eventLog.Add(s));

            // test data...
            mapConsole.Player.GetGoRogueComponent<IInventoryComponent>().Items.Add(new InventoryItem(
                    "trusty oak staff",
                    "Cut from the woods of the Academy at Kurisau, this staff has served you since you first learned to sense the Wellspring."));
            eventLog.Add("Find the tower's core.");

            Children.Add(leftPane);
            Children.Add(topPane);
            Children.Add(mapConsole);
            Children.Add(eventLog);
        }
    }
}
