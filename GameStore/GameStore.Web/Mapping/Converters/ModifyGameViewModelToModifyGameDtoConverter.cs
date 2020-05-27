using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.Web.Models.ViewModels.GameViewModels;

namespace GameStore.Web.Mapping.Converters
{
    public class ModifyGameViewModelToModifyGameDtoConverter : ITypeConverter<ModifyGameViewModel, ModifyGameDto>
    {
        public ModifyGameDto Convert(ModifyGameViewModel source, ModifyGameDto destination, ResolutionContext context)
        {
            var dto = new ModifyGameDto
            {
               Id = source.Id,
               Key = source.Key,
               Discount = source.Discount,
               Price = source.Price,
               DetailsId = source.DetailsId,
               UnitsOnOrder = source.UnitsOnOrder,
               UnitsInStock = source.UnitsInStock,
               PublisherEntityId = source.PublisherEntityId,
               SelectedGenres = source.SelectedGenres,
               SelectedPlatforms = source.SelectedPlatforms
            };
            
            var defaultLocalization = context.Mapper.Map<GameLocalizationDto>(source);
            dto.Localizations.Add(defaultLocalization);

            if (!(source.GameLocalization?.IsAssigned() ?? false))
            {
                return dto;
            }

            var gameLocalization = context.Mapper.Map<GameLocalizationDto>(source.GameLocalization);
            dto.Localizations.Add(gameLocalization);

            return dto;
        }
    }
}