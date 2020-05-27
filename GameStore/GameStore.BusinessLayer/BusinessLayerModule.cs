using Autofac;
using GameStore.BusinessLayer.Factories;
using GameStore.BusinessLayer.Models;
using GameStore.BusinessLayer.Models.Interfaces;
using GameStore.BusinessLayer.Services;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.BusinessLayer.Services.Notification;
using GameStore.BusinessLayer.Services.Notification.Interfaces;
using GameStore.BusinessLayer.Sort.Factories;
using GameStore.BusinessLayer.Sort.Factories.Interfaces;
using GameStore.BusinessLayer.Sort.Options;
using GameStore.BusinessLayer.Sort.Options.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Models;
using Microsoft.Extensions.Configuration;

namespace GameStore.BusinessLayer
{
    public class BusinessLayerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GameService>()
                .As<IGameService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GenreService>()
                .As<IGenreService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PlatformService>()
                .As<IPlatformService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PublisherService>()
                .As<IPublisherService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CommentService>()
                .As<ICommentService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BasketService>()
                .As<IBasketService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrderService>()
                .As<IOrderService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PaymentTypeService>()
                .As<IPaymentTypeService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<InvoiceService>()
                .As<IInvoiceService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ShipperService>()
                .As<IShipperService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MostCommentedSortOption>()
                .Keyed<ISortOption<GameRoot>>(SortOptions.MostCommented)
                .InstancePerLifetimeScope();

            builder.RegisterType<NewSortOption>()
                .Keyed<ISortOption<GameRoot>>(SortOptions.New)
                .InstancePerLifetimeScope();

            builder.RegisterType<MostPopularSortOption>()
                .Keyed<ISortOption<GameRoot>>(SortOptions.MostPopular)
                .InstancePerLifetimeScope();

            builder.RegisterType<PriceAscSortOption>()
                .Keyed<ISortOption<GameRoot>>(SortOptions.PriceAsc)
                .InstancePerLifetimeScope();

            builder.RegisterType<PriceDescSortOption>()
                .Named<ISortOption<GameRoot>>(SortOptions.PriceDesc)
                .InstancePerLifetimeScope();

            builder.RegisterType<GameSortOptionFactory>()
                .As<ISortOptionFactory<GameRoot>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrderDetailsService>()
                .As<IOrderDetailsService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserService>()
                .As<IUserService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RoleService>()
                .As<IRoleService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PermissionService>()
                .As<IPermissionService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RatingService>()
                .As<IRatingService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GameImageService>()
                .As<IGameImageService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrderNotificationService>()
                .As<INotificationService<Order>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MailSenderService>()
                .Keyed<INotificationSenderService<Order>>(NotificationMethod.Email)
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(NotificationSenderServiceFactory<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.Register(c =>
                {
                    const string notificationConfigSegment = nameof(EmailNotificationSettings);
                    var config = c.Resolve<IConfiguration>();
                    var host = config.GetValue<string>($"{notificationConfigSegment}:Host");
                    var email = config.GetValue<string>($"{notificationConfigSegment}:Email");
                    var password = config.GetValue<string>($"{notificationConfigSegment}:Password");
                    var port = config.GetValue<int>($"{notificationConfigSegment}:Port");

                    var settings = new EmailNotificationSettings(host, email, password, port);

                    return settings;
                }).As<IEmailNotificationSettings>()
                .InstancePerLifetimeScope();
        }
    }
}