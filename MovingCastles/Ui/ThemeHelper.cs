using SadConsole.Controls;
using SadConsole.Themes;

namespace MovingCastles.Ui
{
    public static class ThemeHelper
    {
        public static ButtonTheme ButtonThemeNoEnds()
        {
            var buttonTheme = (ButtonTheme)Library.Default.GetControlTheme(typeof(Button));
            buttonTheme.ShowEnds = false;
            return buttonTheme;
        }
    }
}
