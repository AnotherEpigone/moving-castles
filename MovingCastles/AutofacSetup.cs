using Autofac;

namespace MovingCastles
{
    public static class AutofacSetup
    {
        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MovingCastles>().AsSelf();
            return builder.Build();
        }
    }
}
