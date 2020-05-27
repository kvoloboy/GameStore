using System.Linq;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Models;
using GameStore.Web.Mapping.Converters;
using GameStore.Web.Models;
using GameStore.Web.Models.ViewModels.CommentViewModels;
using GameStore.Web.Models.ViewModels.GameViewModels;
using GameStore.Web.Models.ViewModels.GenreViewModels;
using GameStore.Web.Models.ViewModels.ImageViewModels;
using GameStore.Web.Models.ViewModels.OrderViewModels;
using GameStore.Web.Models.ViewModels.PageViewModels;
using GameStore.Web.Models.ViewModels.PlatformViewModels;
using GameStore.Web.Models.ViewModels.PublisherViewModels;

namespace GameStore.Web.Mapping
{
    public class DtoToViewModel : Profile
    {
        public DtoToViewModel()
        {
            CreateMap<GenreDto, ModifyGenreViewModel>();

            CreateMap<PlatformDto, PlatformViewModel>();

            CreateMap<PublisherDto, PublisherViewModel>();

            CreateMap<PublisherDto, ListItem>()
                .ForMember(li => li.Name, opts => opts.MapFrom(p => p.CompanyName));

            CreateMap<GenreNodeDto, TreeViewListItem>(MemberList.None);

            CreateMap<PlatformDto, ListItem>();

            CreateMap<CommentDto, CommentViewModel>();

            CreateMap<GameDto, DisplayGameViewModel>()
                .ForMember(model => model.Genres, options =>
                    options.MapFrom(dto => dto.SelectedGenres))
                .ForMember(model => model.Platforms, options =>
                    options.MapFrom(dto => dto.SelectedPlatforms))
                .ForMember(model => model.Images, options => 
                    options.MapFrom(dto => dto.Images.ToList()));

            CreateMap<GameLocalizationDto, ModifyGameViewModel>(MemberList.None)
                .ForMember(model => model.Id, options => options.Ignore())
                .ForMember(model => model.LocalizationId, options =>
                    options.MapFrom(dto => dto.Id));

            CreateMap<GameLocalizationDto, GameLocalizationViewModel>();

            CreateMap<ModifyGameDto, ModifyGameViewModel>();

            CreateMap<PageOptions, PageOptionsViewModel>();

            CreateMap<OrderDetailsDto, ModifyOrderDetailsViewModel>();

            CreateMap<ShipmentDto, ShipmentViewModel>();

            CreateMap<PublisherLocalizationDto, PublisherLocalizationViewModel>(MemberList.None);

            CreateMap<PublisherLocalizationDto, ModifyPublisherViewModel>()
                .ForMember(model => model.LocalizationId, options =>
                    options.MapFrom(dto => dto.Id))
                .ForMember(model => model.Id, options => options.Ignore());

            CreateMap<ModifyPublisherDto, ModifyPublisherViewModel>(MemberList.None);

            CreateMap<OrderDetailsDto, OrderDetailsViewModel>()
                .ConvertUsing<OrderDetailsDtoToOrderDetailsViewModelConverter>();

            CreateMap<OrderDto, DisplayOrderViewModel>()
                .ConvertUsing<OrderDtoToDisplayOrderViewModelConverter>();

            CreateMap<RatingDto, RatingViewModel>();

            CreateMap<GameImageDto, GameImageViewModel>()
                .ConvertUsing<GameImageDtoToGameImageViewModelConverter>();
        }
    }
}