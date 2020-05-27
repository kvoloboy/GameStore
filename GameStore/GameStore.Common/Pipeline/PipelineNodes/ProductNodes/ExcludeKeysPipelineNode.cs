using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.Common.Extensions;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.DataAccess.Mongo.Models;

namespace GameStore.Common.Pipeline.PipelineNodes.ProductNodes
{
    public class ExcludeKeysPipelineNode : IPipelineNode<Product>
    {
        private readonly IEnumerable<string> _keys;

        public ExcludeKeysPipelineNode(IEnumerable<string> keys)
        {
            _keys = keys;
        }

        public Expression<Func<Product, bool>> Execute(Expression<Func<Product, bool>> input)
        {
            if (_keys == null || !_keys.Any())
            {
                return input;
            }

            Expression<Func<Product, bool>> filter = product => !_keys.Contains(product.Key);

            if (input == null)
            {
                return filter;
            }

            var newChain = input.AndAlso(filter);

            return newChain;
        }
    }
}