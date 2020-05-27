using Autofac;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using GameStore.Core.Models.Identity;
using GameStore.Core.Models.Notification;
using GameStore.DataAccess.Sql.Context;
using GameStore.DataAccess.Sql.Factories;
using GameStore.DataAccess.Sql.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GameStore.Common
{
    public class SqlPersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var config = c.Resolve<IConfiguration>();
                var connectionString = config["ConnectionStrings:GameStoreContext"];
                var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
                optionBuilder.UseSqlServer(connectionString);

                return new AppDbContext(optionBuilder.Options);
            }).InstancePerLifetimeScope();

            builder.RegisterType<GameRootRepository>()
                .As<IAsyncRepository<GameRoot>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GameDetailsAsyncRepository>()
                .As<IAsyncRepository<GameDetails>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GenreAsyncRepository>()
                .As<IAsyncRepository<Genre>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PlatformAsyncRepository>()
                .As<IAsyncRepository<Platform>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CommentRepository>()
                .As<IAsyncRepository<Comment>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrderAsyncRepository>()
                .As<IAsyncRepository<Order>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrderDetailsAsyncRepository>()
                .As<IAsyncRepository<OrderDetails>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<VisitAsyncRepository>()
                .As<IAsyncRepository<Visit>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PaymentAsyncRepository>()
                .As<IAsyncRepository<PaymentType>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RepositoryFactory>()
                .As<IRepositoryFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PublisherAsyncRepository>()
                .As<IAsyncRepository<Publisher>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserAsyncRepository>()
                .As<IAsyncRepository<User>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RoleAsyncRepository>()
                .As<IAsyncRepository<Role>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PermissionsReadonlyRepository>()
                .As<IAsyncReadonlyRepository<Permission>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PublisherLocalizationRepository>()
                .As<IAsyncRepository<PublisherLocalization>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GameLocalizationAsyncRepository>()
                .As<IAsyncRepository<GameLocalization>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RatingAsyncRepository>()
                .As<IAsyncRepository<Rating>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GameImageAsyncRepository>()
                .As<IAsyncRepository<GameImage>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<NotificationAsyncRepository>()
                .As<IAsyncReadonlyRepository<Notification>>()
                .InstancePerLifetimeScope();
        }
    }
}