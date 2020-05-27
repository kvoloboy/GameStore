using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.Common.Models;
using GameStore.Web.Mapping.Converters;
using GameStore.Web.Models.ViewModels.CommentViewModels;
using GameStore.Web.Models.ViewModels.FilterViewModels;
using GameStore.Web.Models.ViewModels.GameViewModels;
using GameStore.Web.Models.ViewModels.GenreViewModels;
using GameStore.Web.Models.ViewModels.OrderViewModels;
using GameStore.Web.Models.ViewModels.PlatformViewModels;
using GameStore.Web.Models.ViewModels.PublisherViewModels;
using GameStore.Web.Models.ViewModels.RoleViewModels;

namespace GameStore.Web.Mapping
{
    public class ViewModelToDto : Profile
    {
        public ViewModelToDto()
        {
            CreateMap<CommentViewModel, CommentDto>();

            CreateMap<ModifyGenreViewModel, GenreDto>();

            CreateMap<PlatformViewModel, PlatformDto>();

            CreateMap<PublisherViewModel, PublisherDto>();

            CreateMap<ModifyGameViewModel, GameDto>();

            CreateMap<ModifyGameViewModel, ModifyGameDto>()
                .ConvertUsing<ModifyGameViewModelToModifyGameDtoConverter>();

            CreateMap<ModifyGameViewModel, GameLocalizationDto>()
                .ForMember(dto => dto.Id, options =>
                    options.MapFrom(model => model.LocalizationId))
                .ForMember(dto => dto.CultureName, options =>
                    options.MapFrom(model => Culture.En))
                .ForMember(dto => dto.GameId, options =>
                    options.MapFrom(model => model.Id));

            CreateMap<GameLocalizationViewModel, GameLocalizationDto>()
                .ForMember(dto => dto.CultureName, options =>
                    options.MapFrom(viewModel => Culture.Ru));

            CreateMap<DisplayGameViewModel, GameDto>();

            CreateMap<FilterSelectedOptionsViewModel, GameFilterData>();

            CreateMap<ShipmentViewModel, ShipmentDto>();

            CreateMap<ModifyOrderDetailsViewModel, OrderDetailsDto>();

            CreateMap<ShipmentViewModel, ShipmentDto>();

            CreateMap<ModifyRoleViewModel, RoleDto>()
                .ForMember(dto => dto.Permissions, opts
                    => opts.MapFrom(viewModel => viewModel.SelectedPermissions));

            CreateMap<ModifyPublisherViewModel, ModifyPublisherDto>()
                .ConvertUsing<ModifyPublisherViewModelToModifyPublisherDtoConverter>();

            CreateMap<PublisherLocalizationViewModel, PublisherLocalizationDto>(MemberList.None)
                .ForMember(dto => dto.CultureName, options =>
                    options.MapFrom(viewModel => Culture.Ru));

            CreateMap<ModifyPublisherViewModel, PublisherLocalizationDto>()
                .ForMember(dto => dto.PublisherId, options => options.MapFrom(model => model.Id))
                .ForMember(dto => dto.Id, options => options.MapFrom(model => model.LocalizationId))
                .ForMember(dto => dto.CultureName, options => options.MapFrom(model => Culture.En));

            CreateMap<ModifyOrderDetailsViewModel, OrderDetailsDto>();

            CreateMap<ModifyOrderViewModel, OrderDto>();
        }
    }
}