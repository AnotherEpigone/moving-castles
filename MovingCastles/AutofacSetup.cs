using Autofac;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Saving;
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
            builder.RegisterType<GameManager>()
                .As<IGameManager>();
            builder.RegisterType<SaveManager>()
                .As<ISaveManager>();

            builder.RegisterType<LogManager>()
                .As<ILogManager>();

            builder.RegisterType<MovingCastles>().AsSelf();
            return builder.Build();
        }
    }
}
