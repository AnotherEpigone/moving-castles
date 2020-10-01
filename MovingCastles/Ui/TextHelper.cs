namespace MovingCastles.Ui
{
    public static class TextHelper
    {
        public static string TruncateString(string input, int maxLen)
        {
            if (input.Length > maxLen)
            {
                return input.Substring(0, maxLen - 3) + "...";
            }

            return input;
        }
    }
}
