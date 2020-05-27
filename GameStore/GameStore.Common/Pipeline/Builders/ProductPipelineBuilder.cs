using System.Collections.Generic;
using GameStore.Common.Pipeline.Builders.Interfaces;
using GameStore.Common.Pipeline.Interfaces;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.DataAccess.Mongo.Models;
using GameStore.DataAccess.Mongo.Repositories.Interfaces;

namespace GameStore.Common.Pipeline.Builders
{
    public class ProductPipelineBuilder : IBuilder<IPipeline<IEnumerable<Product>>, IPipelineNode<Product>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICollection<IPipelineNode<Product>> _pipelineNodes = new List<IPipelineNode<Product>>();

        public ProductPipelineBuilder(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IBuilder<IPipeline<IEnumerable<Product>>, IPipelineNode<Product>> WithNode(IPipelineNode<Product> node)
        {
            _pipelineNodes.Add(node);

            return this;
        }

        public IPipeline<IEnumerable<Product>> Build()
        {
            var nodes = GetNodesCopy();
            _pipelineNodes.Clear();

            var pipeline = new ProductPipeline(nodes, _productRepository);
            
            return pipeline;
        }
        
        private IEnumerable<IPipelineNode<Product>> GetNodesCopy()
        {
            var nodes = new IPipelineNode<Product>[_pipelineNodes.Count];
            _pipelineNodes.CopyTo(nodes, 0);
            
            return nodes;
        }
    }
}