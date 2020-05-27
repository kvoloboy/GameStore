using System.Collections.Generic;
using AutoMapper;
using GameStore.BusinessLayer.DTO;

namespace GameStore.BusinessLayer.Mappings.Converters
{
    public class PublisherDtoToModifyPublisherDtoConverter : ITypeConverter<PublisherDto, ModifyPublisherDto>
    {
        public ModifyPublisherDto Convert(
            PublisherDto source,
            ModifyPublisherDto destination,
            ResolutionContext context)
        {
            var modifyPublisherDto = new ModifyPublisherDto
            {
                Id = source.Id,
                CompanyName = source.CompanyName,
                HomePage = source.HomePage,
                 Fax = source.Fax,
                 Phone = source.Phone,
                 PostalCode = source.PostalCode,
                 UserId = source.UserId
            };
            
            var localization = context.Mapper.Map<PublisherLocalizationDto>(source);
            modifyPublisherDto.Localizations = new List<PublisherLocalizationDto>
            {
                localization
            };

            return modifyPublisherDto;
        }
    }
}