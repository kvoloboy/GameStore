using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.Web.Models.ViewModels.PublisherViewModels;

namespace GameStore.Web.Mapping.Converters
{
    public class
        ModifyPublisherViewModelToModifyPublisherDtoConverter :
            ITypeConverter<ModifyPublisherViewModel, ModifyPublisherDto>
    {
        public ModifyPublisherDto Convert(ModifyPublisherViewModel source, ModifyPublisherDto destination,
            ResolutionContext context)
        {
            var dto = new ModifyPublisherDto
            {
                Id = source.Id,
                CompanyName = source.CompanyName,
                Phone = source.Phone,
                Fax = source.Fax,
                HomePage = source.HomePage,
                PostalCode = source.PostalCode,
                UserId = source.UserId
            };
            
            var defaultLocalization = context.Mapper.Map<PublisherLocalizationDto>(source);
            dto.Localizations.Add(defaultLocalization);

            if (!(source.PublisherLocalization?.IsAssigned() ?? false))
            {
                return dto;
            }

            var localizationDto = context.Mapper.Map<PublisherLocalizationDto>(source.PublisherLocalization);
            dto.Localizations.Add(localizationDto);

            return dto;
        }
    }
}