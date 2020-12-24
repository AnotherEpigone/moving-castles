using Microsoft.Xna.Framework;
using SadConsole.Themes;

namespace MovingCastles.Ui
{
    public static class ColorHelper
    {
        public static Color PlayerBlue = new Color(140, 180, 190);
        public static Color MidnightestBlue = new Color(3, 3, 15);
        public static Color MidnighterBlue = new Color(5, 5, 25);
        public static Color MidnightBlue = new Color(15, 15, 75);
        public static Color SelectedControlBackBlue = new Color(15, 15, 45);
        public static Color ManaBlue = new Color(45, 105, 175);
        public static Color DepletedManaBlue = new Color(10, 25, 45);
        public static Color HealthRed = new Color(135, 0, 0);
        public static Color DepletedHealthRed = new Color(30, 0, 0);
        public static Color RedHighlight = new Color(280, 0, 0, 150);
        public static Color YellowHighlight = new Color(280, 180, 0, 150);
        public static Color WhiteHighlight = new Color(255, 255, 255, 150);
        public static Color GreyHighlight = new Color(200, 200, 200, 200);
        public static Color DarkGreyHighlight = new Color(100, 100, 100, 100);

        public static Colors GetThemeColorsForBackgroundColor(Color bgColor)
        {
            var colors = Library.Default.Colors.Clone();

            colors.ControlBack = bgColor;
            colors.ControlBackLight = bgColor;
            colors.ControlBackSelected = bgColor;
            colors.ControlBackDark = bgColor;
            colors.ControlHostBack = bgColor;

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

        public static string GetParserString(string text, Color color)
        {
            return $"[c:r f:{color.ToParser()}]{text}[c:u]";
        }
    }
}
