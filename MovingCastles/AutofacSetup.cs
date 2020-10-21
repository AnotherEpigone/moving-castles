using Autofac;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using MovingCastles.Ui;

namespace MovingCastles
{
    public static class AutofacSetup
    {
        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<LogManager>()
                .As<ILogManager>();
            builder.RegisterType<UiManager>()
                .As<IUiManager>();
            builder.RegisterType<GameManager>()
                .As<IGameManager>();
            builder.RegisterType<ItemTemplateLoader>()
                .As<IItemTemplateLoader>();
            builder.RegisterType<MapTemplateLoader>()
                .As<IMapTemplateLoader>();

            builder.RegisterType<MovingCastles>().AsSelf();
            return builder.Build();
        }
    }
}
