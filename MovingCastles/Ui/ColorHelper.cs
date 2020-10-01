using Microsoft.Xna.Framework;
using SadConsole.Themes;

namespace MovingCastles.Ui
{
    public static class ColorHelper
    {
        public static Color MidnightestBlue = new Color(3, 3, 15);
        public static Color MidnighterBlue = new Color(5, 5, 25);
        public static Color ManaBlue = new Color(45, 105, 175);
        public static Color DepletedManaBlue = new Color(10, 25, 45);
        public static Color HealthRed = new Color(135, 0, 0);
        public static Color DepletedHealthRed = new Color(30, 0, 0);
        public static Color WhiteHighlight = new Color(255, 255, 255, 200);
        public static Color GreyHighlight = new Color(100, 100, 100, 100);

        public static Colors GetTransparentBackgroundThemeColors()
        {
            var colors = Library.Default.Colors.Clone();

            colors.ControlBack = Color.Transparent;
            colors.ControlBackLight = Color.Transparent;
            colors.ControlBackSelected = Color.Transparent;
            colors.ControlBackDark = Color.Transparent;
            colors.ControlHostBack = Color.Transparent;

            colors.RebuildAppearances();

            return colors;
        }

        public static Colors GetProgressBarThemeColors(Color back, Color fore)
        {
            var colors = Library.Default.Colors.Clone();

            colors.Text = fore;

            colors.ControlBack = back;
            colors.ControlBackLight = back;
            colors.ControlBackSelected = back;
            colors.ControlBackDark = back;
            colors.ControlHostBack = back;

            colors.RebuildAppearances();

            return colors;
        }
    }
}
