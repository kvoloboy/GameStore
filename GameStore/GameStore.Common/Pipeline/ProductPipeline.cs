using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.Common.Pipeline.Interfaces;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.DataAccess.Mongo.Models;
using GameStore.DataAccess.Mongo.Repositories.Interfaces;

namespace GameStore.Common.Pipeline
{
    public class ProductPipeline : IPipeline<IEnumerable<Product>>
    {
        private readonly IEnumerable<IPipelineNode<Product>> _pipelineNodes;
        private readonly IProductRepository _productRepository;

        public ProductPipeline(IEnumerable<IPipelineNode<Product>> pipelineNodes, IProductRepository productRepository)
        {
            _pipelineNodes = pipelineNodes;
            _productRepository = productRepository;
        }
        
        public async Task<IEnumerable<Product>> ExecuteAsync()
        {
            var filter = CreateExpression();
            var products = await _productRepository.FindAllAsync(filter);

            return products;
        }
        
        private Expression<Func<Product, bool>> CreateExpression()
        {
            var expression = _pipelineNodes.Aggregate<IPipelineNode<Product>, Expression<Func<Product, bool>>>(null,
                (current, pipelineNode) => pipelineNode.Execute(current));

            return expression;
        }
    }
}