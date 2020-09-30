using Autofac;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Items;
using MovingCastles.Maps;
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
            builder.RegisterType<ItemTemplateLoader>()
                .As<IItemTemplateLoader>();
            builder.RegisterType<MapTemplateLoader>()
                .As<IMapTemplateLoader>();

            builder.RegisterType<MovingCastles>().AsSelf();
            return builder.Build();
        }
    }
}
