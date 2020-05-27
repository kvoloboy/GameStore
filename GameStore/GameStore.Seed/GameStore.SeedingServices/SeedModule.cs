using Autofac;
using GameStore.Core.Models;
using GameStore.SeedingServices.Repositories;
using GameStore.SeedingServices.Repositories.Interfaces;
using GameStore.SeedingServices.Services;
using GameStore.SeedingServices.Services.Interfaces;

namespace GameStore.SeedingServices
{
    public class SeedModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CategoryRepository>()
                .As<IRepository<Genre>>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ProductRepository>()
                .As<IProductRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<MongoProductKeyGenerator>().As<IMongoProductKeyGenerator>()
                .InstancePerLifetimeScope();
        }
    }
}