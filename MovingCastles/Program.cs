namespace MovingCastles
{
    internal static class Program
    {
        private static void Main()
        {
            using var game = new MovingCastles();
            game.Run();
        }
    }
}
