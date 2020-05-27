using Autofac;
using GameStore.Infrastructure.DatabaseSettings;
using GameStore.Infrastructure.DatabaseSettings.Interfaces;
using GameStore.Infrastructure.Logging;
using GameStore.Infrastructure.Logging.Interfaces;

namespace GameStore.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(MongoDatabaseSettings<>)).As(typeof(IMongoDatabaseSettings<>))
                .InstancePerLifetimeScope();
            builder.RegisterType<Logger>().As<ILogger>().InstancePerLifetimeScope();
        }
    }
}