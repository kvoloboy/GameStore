using System.Collections.Generic;
using AutoMapper;
using GameStore.Core.Models;
using GameStore.DataAccess.Mongo.Models;

namespace GameStore.Common.Mappings.Converters
{
    public class ProductToGameRootConverter : ITypeConverter<Product, GameRoot>
    {
        public GameRoot Convert(Product source, GameRoot destination, ResolutionContext context)
        {
            var localization = context.Mapper.Map<GameLocalization>(source);
            var details = context.Mapper.Map<GameDetails>(source);
            var gameRoot = new GameRoot
            {
                Key = source.Key,
                Details = details,
                Localizations = new List<GameLocalization>
                {
                    localization
                }
            };

            return gameRoot;
        }
    }
}