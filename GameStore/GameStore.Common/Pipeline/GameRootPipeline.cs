using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.Common.Pipeline.Interfaces;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;

namespace GameStore.Common.Pipeline
{
    public class GameRootPipeline : IPipeline<IEnumerable<GameRoot>>
    {
        private readonly IEnumerable<IPipelineNode<GameRoot>> _pipelineNodes;
        private readonly IAsyncRepository<GameRoot> _gamesRepository;

        public GameRootPipeline(
            IEnumerable<IPipelineNode<GameRoot>> pipelineNodes,
            IAsyncRepository<GameRoot> gamesRepository)
        {
            _pipelineNodes = pipelineNodes;
            _gamesRepository = gamesRepository;
        }

        public async Task<IEnumerable<GameRoot>> ExecuteAsync()
        {
            var filter = CreateExpression();
            var games = await _gamesRepository.FindAllAsync(filter);

            return games;
        }

        private Expression<Func<GameRoot, bool>> CreateExpression()
        {
            var expression = _pipelineNodes.Aggregate<IPipelineNode<GameRoot>, Expression<Func<GameRoot, bool>>>(null,
                (current, pipelineNode) => pipelineNode.Execute(current));

            return expression;
        }
    }
}