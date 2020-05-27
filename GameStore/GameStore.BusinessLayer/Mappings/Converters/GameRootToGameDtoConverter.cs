using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Mappings.Converters
{
    public class GameRootToGameDtoConverter : ITypeConverter<GameRoot, GameDto>
    {
        public GameDto Convert(GameRoot source, GameDto destination, ResolutionContext context)
        {
            var gameDto = new GameDto
            {
                Id = source.Id,
                Key = source.Key,
                PublisherEntityId = source.PublisherEntityId,
                DetailsId = source.Details?.Id,
                Price = source.Details?.Price ?? default,
                Discount = source.Details?.Discount ?? default,
                UnitsInStock = source.Details?.UnitsInStock,
                CreationDate = source.Details?.CreationDate ?? default,
                UnitsOnOrder = source.Details?.UnitsOnOrder ?? default,
                SelectedGenres = source.GameGenres?.Select(genre => genre.Genre?.Name).ToList(),
                SelectedPlatforms = source.GamePlatforms?.Select(platform => platform.Platform?.Name).ToList(),
                Images = GetImagesDto(source.GameImages),
                RatingDto = GetRatingDto(source)
            };

            return gameDto;
        }

        private static RatingDto GetRatingDto(GameRoot gameRoot)
        {
            var ratingDto = new RatingDto(gameRoot.Id,
                    gameRoot.GameRatings.Sum(rating => rating.Value),
                    gameRoot.GameRatings.Count);

            return ratingDto;
        }

        private static ICollection<GameImageDto> GetImagesDto(IEnumerable<GameImage> images)
        {
            var gameImages = images.Select(gameImage => new GameImageDto
            {
                Id = gameImage.Id,
                Content = gameImage.Content,
                ContentType = gameImage.ContentType,
                GameKey = gameImage.GameRoot.Key
            }).ToList();

            return gameImages;
        }
    }
}