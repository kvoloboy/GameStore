using Autofac;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Models;
using GameStore.Core.Models;
using GameStore.Identity.CookieValidation;
using GameStore.Web.Factories;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Filters;
using GameStore.Web.Mapping;
using GameStore.Web.Mapping.Converters;
using GameStore.Web.Models.ViewModels.CommentViewModels;
using GameStore.Web.Models.ViewModels.FilterViewModels;
using GameStore.Web.Models.ViewModels.GameViewModels;
using GameStore.Web.Models.ViewModels.GenreViewModels;
using GameStore.Web.Models.ViewModels.ImageViewModels;
using GameStore.Web.Models.ViewModels.OrderViewModels;
using GameStore.Web.Models.ViewModels.PageViewModels;
using GameStore.Web.Models.ViewModels.RoleViewModels;
using GameStore.Web.Models.ViewModels.UserViewModels;
using GameStore.Web.WebServices;
using GameStore.Web.WebServices.Payments;
using GameStore.Web.WebServices.Payments.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace GameStore.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GameViewModelFactory>()
                .As<IAsyncViewModelFactory<ModifyGameViewModel, GameViewModel>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GenreViewModelFactory>()
                .As<IAsyncViewModelFactory<ModifyGenreViewModel, GenreViewModel>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DisplayCommentViewModelFactory>()
                .As<IAsyncViewModelFactory<CommentViewModel, DisplayCommentsViewModel>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrderViewModelFactory>()
                .As<IAsyncViewModelFactory<OrderDto, OrderViewModel>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FilterViewModelFactory>()
                .As<IAsyncViewModelFactory<FilterSelectedOptionsViewModel, FilterViewModel>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PageOptionsViewModelFactory>()
                .As<IViewModelFactory<PageOptions, PageOptionsViewModel>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ShipmentViewModelFactory>()
                .As<IAsyncViewModelFactory<ShipmentViewModel, ShipmentViewModel>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DetailedOrderViewModelFactory>()
                .As<IAsyncViewModelFactory<string, DetailedOrderViewModel>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BankPaymentStrategy>()
                .Named<IPaymentStrategy>("Bank")
                .InstancePerLifetimeScope();

            builder.RegisterType<BoxPaymentStrategy>()
                .Named<IPaymentStrategy>("IBox terminal")
                .InstancePerLifetimeScope();

            builder.RegisterType<VisaPaymentStrategy>()
                .Named<IPaymentStrategy>("Visa")
                .InstancePerLifetimeScope();

            builder.RegisterType<PaymentStrategyFactory>()
                .As<IPaymentStrategyFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrderDtoToDisplayOrderViewModelConverter>()
                .As<ITypeConverter<OrderDto, DisplayOrderViewModel>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ModifyOrderDetailsViewModelFactory>()
                .As<IAsyncViewModelFactory<ModifyOrderDetailsViewModel, ModifyOrderDetailsViewModel>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrdersListViewModelFactory>()
                .As<IAsyncViewModelFactory<OrdersListViewModel, OrdersListViewModel>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ModifyRoleViewModelFactory>()
                .As<IAsyncViewModelFactory<ModifyRoleViewModel, ModifyRoleViewModel>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ModifyUserViewModelFactory>()
                .As<IAsyncViewModelFactory<string, ModifyUserViewModel>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<IsAllowedUpdateGameFilter>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<IsAllowedUpdatePublisherFilter>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<DisplayGameViewModelFactory>()
                .As<IAsyncViewModelFactory<GameDto, DisplayGameViewModel>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<HttpContextAccessor>()
                .As<IHttpContextAccessor>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GameImageViewModelFactory>()
                .As<IViewModelFactory<string, GameImageViewModel>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrderDetailsDtoToOrderDetailsViewModelConverter>()
                .As<ITypeConverter<OrderDetailsDto, OrderDetailsViewModel>>()
                .InstancePerLifetimeScope();
        }
    }
}