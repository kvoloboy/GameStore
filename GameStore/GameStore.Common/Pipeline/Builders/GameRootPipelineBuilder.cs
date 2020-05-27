using System.Collections.Generic;
using GameStore.Common.Pipeline.Builders.Interfaces;
using GameStore.Common.Pipeline.Interfaces;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;

namespace GameStore.Common.Pipeline.Builders
{
    public class GameRootPipelineBuilder : IBuilder<IPipeline<IEnumerable<GameRoot>>, IPipelineNode<GameRoot>>
    {
        private readonly IAsyncRepository<GameRoot> _gameRootRepository;
        private readonly ICollection<IPipelineNode<GameRoot>> _pipelineNodes = new List<IPipelineNode<GameRoot>>();

        public GameRootPipelineBuilder(IAsyncRepository<GameRoot> gameRootRepository)
        {
            _gameRootRepository = gameRootRepository;
        }
        
        public IBuilder<IPipeline<IEnumerable<GameRoot>>, IPipelineNode<GameRoot>> WithNode(IPipelineNode<GameRoot> node)
        {
            _pipelineNodes.Add(node);
            
            return this;
        }

        public IPipeline<IEnumerable<GameRoot>> Build()
        {
            var nodes = GetNodesCopy();
            _pipelineNodes.Clear();

            var pipeline = new GameRootPipeline(nodes, _gameRootRepository);
            
            return pipeline;
        }
        
        private IEnumerable<IPipelineNode<GameRoot>> GetNodesCopy()
        {
            var nodes = new IPipelineNode<GameRoot>[_pipelineNodes.Count];
            _pipelineNodes.CopyTo(nodes, 0);
            
            return nodes;
        }
    }
}