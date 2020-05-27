using System;
using System.Linq.Expressions;
using GameStore.Common.Extensions;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.Core.Models;

namespace GameStore.Common.Pipeline.PipelineNodes.GameRootNodes
{
    public class PriceRangePipelineNode : IPipelineNode<GameRoot>
    {
        private readonly decimal _minPrice;
        private readonly decimal _maxPrice;

        public PriceRangePipelineNode(decimal minPrice, decimal maxPrice)
        {
            _minPrice = minPrice;
            _maxPrice = maxPrice;
        }

        public Expression<Func<GameRoot, bool>> Execute(Expression<Func<GameRoot, bool>> input)
        {
            var areNotAssigned = _minPrice == 0 && _maxPrice == 0;

            if (areNotAssigned)
            {
                return input;
            }

            Expression<Func<GameRoot, bool>> filter = root =>
                root.Details.Price >= _minPrice && root.Details.Price <= _maxPrice || root.Details == null;

            if (input == null)
            {
                return filter;
            }

            var newChain = input.AndAlso(filter);

            return newChain;
        }
    }
}