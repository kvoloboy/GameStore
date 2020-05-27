using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.Common.Extensions;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.Core.Models;

namespace GameStore.Common.Pipeline.PipelineNodes.GameRootNodes
{
    public class KeyPipelineNode : IPipelineNode<GameRoot>
    {
        private readonly IEnumerable<string> _keys;

        public KeyPipelineNode(IEnumerable<string> keys)
        {
            _keys = keys;
        }

        public Expression<Func<GameRoot, bool>> Execute(Expression<Func<GameRoot, bool>> input)
        {
            if (_keys == null || !_keys.Any())
            {
                return input;
            }
            
            Expression<Func<GameRoot, bool>> filter = game => _keys.Contains(game.Key);

            if (input == null)
            {
                return filter;
            }
            
            var newChain = input.AndAlso(filter);
            
            return newChain;
        }
    }
}