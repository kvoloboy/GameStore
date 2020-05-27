using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Mappings.Converters;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Mappings
{
    public class DtoToEntity : Profile
    {
        public DtoToEntity()
        {
            CreateMap<ModifyPublisherDto, Publisher>(MemberList.None);

            CreateMap<PublisherLocalizationDto, PublisherLocalization>(MemberList.None)
                .ForMember(localization => localization.PublisherEntityId,
                    options => options.MapFrom(dto => dto.PublisherId));

            CreateMap<PlatformDto, Platform>();

            CreateMap<GenreDto, Genre>(MemberList.None);

            CreateMap<CommentDto, Comment>(MemberList.None)
                .ForMember(comment => comment.GameRoot, opts => opts.MapFrom(dto =>
                    new GameRoot {Key = dto.GameKey}));

            CreateMap<ModifyGameDto, GameRoot>(MemberList.None).ConvertUsing<ModifyGameDtoToGameRootConverter>();

            CreateMap<ModifyGameDto, GameDetails>()
                .ForMember(details => details.Id, options =>
                    options.MapFrom(dto => dto.DetailsId))
                .ForMember(details => details.GameRootId, options =>
                    options.MapFrom(dto => dto.Id));

            CreateMap<GameLocalizationDto, GameLocalization>()
                .ForMember(localization => localization.GameRootId, options =>
                    options.MapFrom(dto => dto.GameId));

            CreateMap<ShipmentDto, Order>(MemberList.None);

            CreateMap<OrderDetailsDto, OrderDetails>(MemberList.None)
                .ForMember(details => details.GameRootId, opts => opts.MapFrom(dto => dto.GameId));

            CreateMap<OrderDto, Order>();
        }
    }
}