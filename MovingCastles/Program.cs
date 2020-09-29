using Autofac;

namespace MovingCastles
{
    internal static class Program
    {
        private static void Main()
        {
            var container = AutofacSetup.CreateContainer();

            using var game = container.Resolve<MovingCastles>();
            game.Run();
        }
    }
}
