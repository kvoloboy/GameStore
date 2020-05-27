using System;
using System.Linq.Expressions;
using GameStore.Common.Extensions;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.DataAccess.Mongo.Models;

namespace GameStore.Common.Pipeline.PipelineNodes.ProductNodes
{
    public class PriceRangePipelineNode : IPipelineNode<Product>
    {
        private readonly decimal _minPrice;
        private readonly decimal _maxPrice;

        public PriceRangePipelineNode(decimal minPrice, decimal maxPrice)
        {
            _minPrice = minPrice;
            _maxPrice = maxPrice;
        }

        public Expression<Func<Product, bool>> Execute(Expression<Func<Product, bool>> input)
        {
            var areNotAssigned = _minPrice == 0 && _maxPrice == 0;
            
            if (areNotAssigned)
            {
                return input;
            }
            
            Expression<Func<Product, bool>> filter = product =>
                product.UnitPrice >= _minPrice && product.UnitPrice <= _maxPrice;

            if (input == null)
            {
                return filter;
            }

            var newChain = input.AndAlso(filter);

            return newChain;
        }
    }
}