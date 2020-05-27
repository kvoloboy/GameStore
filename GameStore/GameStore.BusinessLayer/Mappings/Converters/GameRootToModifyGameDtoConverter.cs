using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Mappings.Converters
{
    public class GameRootToModifyGameDtoConverter : ITypeConverter<GameRoot, ModifyGameDto>
    {
        public ModifyGameDto Convert(GameRoot source, ModifyGameDto destination, ResolutionContext context)
        {
            var localizations = context.Mapper.Map<IEnumerable<GameLocalizationDto>>(source.Localizations);
            
            var modifyGameDto = new ModifyGameDto
            {
                Id = source.Id,
                Key = source.Key,
                PublisherEntityId = source.PublisherEntityId,
                DetailsId = source.Details.Id,
                Price = source.Details.Price,
                Discount = source.Details.Discount,
                UnitsInStock = source.Details.UnitsInStock,
                CreationDate = source.Details.CreationDate,
                UnitsOnOrder = source.Details.UnitsOnOrder,
                IsDeleted = source.IsDeleted,
                SelectedGenres = source.GameGenres.Select(genre => genre.GenreId),
                SelectedPlatforms = source.GamePlatforms.Select(platform => platform.PlatformId),
                Localizations = localizations.ToList()
            };

            return modifyGameDto;
        }
    }
}