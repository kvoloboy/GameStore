using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.Common.Extensions;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.Core.Models;

namespace GameStore.Common.Pipeline.PipelineNodes.GameRootNodes
{
    public class PublishersPipelineNode : IPipelineNode<GameRoot>
    {
        private readonly IEnumerable<string> _publishers;

        public PublishersPipelineNode(IEnumerable<string> publishers)
        {
            _publishers = publishers;
        }

        public Expression<Func<GameRoot, bool>> Execute(Expression<Func<GameRoot, bool>> input)
        {
            if (_publishers == null || !_publishers.Any())
            {
                return input;
            }

            Expression<Func<GameRoot, bool>> filter = gameRoot =>
                _publishers.Contains(gameRoot.PublisherEntityId) ||
                gameRoot.PublisherEntityId == null && gameRoot.Details == null;

            if (input == null)
            {
                return filter;
            }

            var newChain = input.AndAlso(filter);

            return newChain;
        }
    }
}