using Autofac;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Saving;
using MovingCastles.Serialization.Settings;
using MovingCastles.Ui;

namespace MovingCastles
{
    public static class AutofacSetup
    {
        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<UiManager>()
                .As<IUiManager>()
                .SingleInstance();
            builder.RegisterType<GameManager>()
                .As<IGameManager>()
                .SingleInstance();
            builder.RegisterType<SaveManager>()
                .As<ISaveManager>()
                .SingleInstance();

            builder.RegisterType<LogManager>()
                .As<ILogManager>()
                .SingleInstance();

            builder.RegisterType<StructureFactory>()
                .As<IStructureFactory>()
                .SingleInstance();

            builder.RegisterType<DungeonMasterFactory>()
                .As<IDungeonMasterFactory>()
                .SingleInstance();

            builder.RegisterType<AppSettings>()
                .As<IAppSettings>()
                .SingleInstance();

            builder.RegisterType<MovingCastles>().AsSelf();
            return builder.Build();
        }
    }
}
