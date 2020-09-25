using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using Microsoft.Xna.Framework;
using MovingCastles.Ui;
using SadConsole;
using SadConsole.Controls;
using System.Collections.Generic;

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

            var controlsList = new List<ControlBase>();
            var nameLabel = new Label(parentEntity.Name) { Position = new Point(1, 0), TextColor = Color.Gainsboro };
            controlsList.Add(nameLabel);

            var healthComponent = Parent.GetComponent<IHealthComponent>();
            if (healthComponent != null)
            {
                var healthBar = new ProgressBar(30, 1, HorizontalAlignment.Left)
                {
                    Position = new Point(0, 1),
                };
                healthBar.ThemeColors = ColorHelper.GetProgressBarThemeColors(ColorHelper.DepletedHealthRed, ColorHelper.HealthRed);
                healthBar.Progress = healthComponent.Health / healthComponent.MaxHealth;
                controlsList.Add(healthBar);
            }

            var sidebarConsole = new ControlsConsole(30, controlsList.Count);
            controlsList.ForEach(c => sidebarConsole.Add(c));

            return sidebarConsole;
        }
    }
}
