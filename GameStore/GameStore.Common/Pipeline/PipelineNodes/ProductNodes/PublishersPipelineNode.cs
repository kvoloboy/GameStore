using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.Common.Extensions;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.DataAccess.Mongo.Models;

namespace GameStore.Common.Pipeline.PipelineNodes.ProductNodes
{
    public class PublishersPipelineNode : IPipelineNode<Product>
    {
        private readonly IEnumerable<string> _publishersId;
        private IEnumerable<string> _keysWherePublishersAreInitialized;

        public PublishersPipelineNode(
            IEnumerable<string> keysWherePublishersAreInitialized,
            IEnumerable<string> publishersId)
        {
            _keysWherePublishersAreInitialized = keysWherePublishersAreInitialized;
            _publishersId = publishersId;
        }

        public Expression<Func<Product, bool>> Execute(Expression<Func<Product, bool>> input)
        {
            var areNotValidPublishers = _publishersId == null || !_publishersId.Any();
            
            if (areNotValidPublishers)
            {
                return input;
            }

            if (_keysWherePublishersAreInitialized == null)
            {
                _keysWherePublishersAreInitialized = new List<string>();
            }

            Expression<Func<Product, bool>> filter = product =>
                _keysWherePublishersAreInitialized.Contains(product.Key) || _publishersId.Contains(product.SupplierId);

            if (input == null)
            {
                return filter;
            }

            var newChain = input.AndAlso(filter);

            return newChain;
        }
    }
}