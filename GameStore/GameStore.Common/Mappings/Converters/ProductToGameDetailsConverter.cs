using System;
using AutoMapper;
using GameStore.Core.Models;
using GameStore.DataAccess.Mongo.Models;

namespace GameStore.Common.Mappings.Converters
{
    public class ProductToGameDetailsConverter : ITypeConverter<Product, GameDetails>
    {
        public GameDetails Convert(Product source, GameDetails destination, ResolutionContext context)
        {
            var details = new GameDetails
            {
                Id = default,
                Price = source.UnitPrice,
                IsDiscontinued = source.Discontinued,
                UnitsInStock = source.UnitsInStock,
                UnitsOnOrder = source.UnitOnOrder,
                CreationDate = DateTime.UtcNow
            };

            return details;
        }
    }
}