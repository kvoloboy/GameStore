using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Mappings.Converters;
using GameStore.Common.Models;

namespace GameStore.BusinessLayer.Mappings
{
    public class DtoToDto : Profile
    {
        public DtoToDto()
        {
            CreateMap<PublisherDto, PublisherLocalizationDto>(MemberList.None)
                .ForMember(localizationDto => localizationDto.Id, options =>
                    options.MapFrom(dto => dto.LocalizationId))
                .ForMember(localizationDto => localizationDto.PublisherId, options =>
                    options.MapFrom(dto => dto.Id))
                .ForMember(p => p.CultureName, options =>
                    options.MapFrom(dto => Culture.En));

            CreateMap<PublisherDto, ModifyPublisherDto>().ConvertUsing<PublisherDtoToModifyPublisherDtoConverter>();
        }
    }
}