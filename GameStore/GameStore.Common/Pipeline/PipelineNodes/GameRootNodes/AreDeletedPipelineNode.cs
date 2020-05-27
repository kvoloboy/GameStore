using System;
using System.Linq.Expressions;
using GameStore.Common.Extensions;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.Core.Models;

namespace GameStore.Common.Pipeline.PipelineNodes.GameRootNodes
{
    public class AreDeletedPipelineNode : IPipelineNode<GameRoot>
    {
        private readonly bool _isDeleted;

        public AreDeletedPipelineNode(bool isDeleted)
        {
            _isDeleted = isDeleted;
        }

        public Expression<Func<GameRoot, bool>> Execute(Expression<Func<GameRoot, bool>> input)
        {
            Expression<Func<GameRoot, bool>> filter = game => game.IsDeleted == _isDeleted;

            if (input == null)
            {
                return filter;
            }

            var newChain = input.AndAlso(filter);
    
            return newChain;
        }
    }
}