using System.Collections.Generic;
using Autofac;
using GameStore.Common.Aggregators;
using GameStore.Common.Aggregators.Interfaces;
using GameStore.Common.Decorators;
using GameStore.Common.Decorators.Interfaces;
using GameStore.Common.Models;
using GameStore.Common.Pipeline.Builders;
using GameStore.Common.Pipeline.Builders.Interfaces;
using GameStore.Common.Pipeline.Interfaces;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using GameStore.DataAccess.Mongo.Models;
using GameStore.DataAccess.Mongo.Repositories;
using GameStore.DataAccess.Mongo.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using OrderDetailsRepository = GameStore.DataAccess.Mongo.Repositories.OrderDetailsRepository;
using OrderRepository = GameStore.DataAccess.Mongo.Repositories.OrderRepository;
using Publisher = GameStore.DataAccess.Mongo.Models.Publisher;
using PublisherRepository = GameStore.DataAccess.Mongo.Repositories.PublisherRepository;

namespace GameStore.Common
{
    public class MongoPersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                const string configSegmentName = "MongoDatabaseSettings";
                const string connectionStringSegment = "ConnectionString";

                var configuration = c.Resolve<IConfiguration>();
                var connectionString = configuration[$"{configSegmentName}:{connectionStringSegment}"];
                var client = new MongoClient(connectionString);

                return client;
            }).AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<GameRootAggregator>()
                .As<IAggregator<GameFilterData, IEnumerable<GameRoot>>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProductRepository>()
                .As<IProductRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrderRepository>()
                .As<IAsyncReadonlyRepository<Order>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PublisherRepository>()
                .As<IAsyncReadonlyRepository<Publisher>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ShipperRepository>()
                .As<IAsyncReadonlyRepository<Shipper>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GameRootPipelineBuilder>()
                .As<IBuilder<IPipeline<IEnumerable<GameRoot>>, IPipelineNode<GameRoot>>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProductPipelineBuilder>()
                .As<IBuilder<IPipeline<IEnumerable<Product>>, IPipelineNode<Product>>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GameDecorator>()
                .As<IGameDecorator>()
                .InstancePerLifetimeScope();

            builder.RegisterDecorator<IAsyncRepository<Order>>((context, parameters, instance) => new OrderDecorator
            (
                instance,
                context.Resolve<IAsyncReadonlyRepository<Order>>(),
                context.Resolve<IAsyncReadonlyRepository<Shipper>>(),
                context.Resolve<IGameDecorator>()
            ));
            
            builder.RegisterDecorator<IAsyncRepository<OrderDetails>>((context, parameters, instance) =>
                new OrderDetailsDecorator
                (
                    instance,
                    context.Resolve<IGameDecorator>()
                ));

            builder.RegisterType<OrderDetailsRepository>().As<IAsyncReadonlyRepository<OrderDetails>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PublisherDecorator>().As<IPublisherDecorator>()
                .InstancePerLifetimeScope();
        }
    }
}