using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Mappings.Converters;
using GameStore.Core.Models;
using GameStore.Core.Models.Identity;

namespace GameStore.BusinessLayer.Mappings
{
    public class EntityToDto : Profile
    {
        public EntityToDto()
        {
            CreateMap<Genre, GenreNodeDto>();

            CreateMap<Genre, GenreDto>();

            CreateMap<Platform, PlatformDto>();

            CreateMap<Comment, CommentDto>()
                .ForMember(dto => dto.GameKey, options =>
                    options.MapFrom(comment => comment.GameRoot.Key));

            CreateMap<GameLocalization, GameLocalizationDto>(MemberList.None)
                .ForMember(dto => dto.GameId, options =>
                    options.MapFrom(localization => localization.GameRootId));

            CreateMap<GameRoot, ModifyGameDto>().ConvertUsing<GameRootToModifyGameDtoConverter>();

            CreateMap<GameRoot, GameDto>().ConvertUsing<GameRootToGameDtoConverter>();

            CreateMap<GameDetails, GameDto>(MemberList.None)
                .ForMember(dto => dto.DetailsId, options => options.MapFrom(details => details.Id))
                .ForMember(dto => dto.Id, options => options.Ignore());

            CreateMap<GameLocalization, GameDto>(MemberList.None)
                .ForMember(dto => dto.Id, options => options.Ignore());

            CreateMap<OrderDetails, OrderDetailsDto>()
                .ForMember(dto => dto.GameId, opts => opts.MapFrom(details => details.GameRootId));

            CreateMap<Order, ShipmentDto>(MemberList.None)
                .ForMember(dto => dto.OrderId, opts => opts.MapFrom(order => order.Id));

            CreateMap<Order, OrderDto>(MemberList.None)
                .ForMember(dto => dto.Details, options => options.Ignore());

            CreateMap<User, UserDto>().ConvertUsing<UserToUserDtoConverter>();

            CreateMap<PublisherLocalization, PublisherDto>(MemberList.None)
                .ForMember(dto => dto.Id, options => options.Ignore())
                .ForMember(dto => dto.LocalizationId, options =>
                    options.MapFrom(localization => localization.Id));

            CreateMap<Publisher, PublisherDto>(MemberList.None);

            CreateMap<Publisher, ModifyPublisherDto>();

            CreateMap<PublisherLocalization, PublisherLocalizationDto>(MemberList.None)
                .ForMember(dto => dto.PublisherId, options =>
                    options.MapFrom(l => l.PublisherEntityId));

            CreateMap<GameImage, GameImageDto>()
                .ForMember(dto => dto.GameKey, options =>
                    options.MapFrom(image => image.GameRoot.Key));
        }
    }
}