using Autofac;
using MovingCastles.Ui;

namespace MovingCastles
{
    public static class AutofacSetup
    {
        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<UiManager>()
                .As<IUiManager>();

            builder.RegisterType<MovingCastles>().AsSelf();
            return builder.Build();
        }
    }
}
