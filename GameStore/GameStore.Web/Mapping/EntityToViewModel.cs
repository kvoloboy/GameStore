using System.Linq;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Models;
using GameStore.Common.Models;
using GameStore.Core.Models;
using GameStore.Core.Models.Identity;
using GameStore.Web.Mapping.Converters;
using GameStore.Web.Models;
using GameStore.Web.Models.ViewModels;
using GameStore.Web.Models.ViewModels.GameViewModels;
using GameStore.Web.Models.ViewModels.GenreViewModels;
using GameStore.Web.Models.ViewModels.OrderViewModels;
using GameStore.Web.Models.ViewModels.PaymentViewModels;
using GameStore.Web.Models.ViewModels.PublisherViewModels;
using GameStore.Web.Models.ViewModels.RoleViewModels;

namespace GameStore.Web.Mapping
{
    public class EntityToViewModel : Profile
    {
        public EntityToViewModel()
        {
            CreateMap<Publisher, PublisherViewModel>();

            CreateMap<Genre, ModifyGenreViewModel>();

            CreateMap<Genre, ListItem>();

            CreateMap<OrderDetails, OrderDetailsViewModel>(MemberList.None);

            CreateMap<Basket, BasketViewModel>();

            CreateMap<PaymentType, PaymentTypeViewModel>();

            CreateMap<Order, ShipmentViewModel>(MemberList.None);

            CreateMap<Role, RoleViewModel>();

            CreateMap<Role, ModifyRoleViewModel>(MemberList.None)
                .ForMember(viewModel => viewModel.SelectedPermissions,
                    options => options.MapFrom(role =>
                        role.RolePermissions.Select(rp => rp.PermissionId)));
        }
    }
}