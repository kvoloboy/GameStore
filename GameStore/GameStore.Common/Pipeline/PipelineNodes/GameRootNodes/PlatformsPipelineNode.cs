using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.Common.Extensions;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.Core.Models;

namespace GameStore.Common.Pipeline.PipelineNodes.GameRootNodes
{
    public class PlatformsPipelineNode : IPipelineNode<GameRoot>
    {
        private readonly IEnumerable<string> _platformsId;

        public PlatformsPipelineNode(IEnumerable<string> platformsId)
        {
            _platformsId = platformsId;
        }
        
        public Expression<Func<GameRoot, bool>> Execute(Expression<Func<GameRoot, bool>> input)
        {
            if (_platformsId == null || !_platformsId.Any())
            {
                return input;
            }

            Expression<Func<GameRoot, bool>> filter = game =>
                game.GamePlatforms.Any(gamePlatform => _platformsId.Contains(gamePlatform.PlatformId));

            if (input == null)
            {
                return filter;
            }

            var newChain = input.AndAlso(filter);
            
            return newChain;
        }
    }
}