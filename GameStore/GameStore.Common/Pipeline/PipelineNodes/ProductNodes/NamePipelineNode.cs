using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.Common.Extensions;
using GameStore.Common.Models;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.DataAccess.Mongo.Models;

namespace GameStore.Common.Pipeline.PipelineNodes.ProductNodes
{
    public class NamePipelineNode : IPipelineNode<Product>
    {
        private readonly string _name;
        private readonly IEnumerable<string> _keysWhereExistLocalization;

        public NamePipelineNode(string name, IEnumerable<string> keysWhereExistLocalization)
        {
            _name = name;
            _keysWhereExistLocalization = keysWhereExistLocalization;
        }

        public Expression<Func<Product, bool>> Execute(Expression<Func<Product, bool>> input)
        {
            var isNotValidName = string.IsNullOrEmpty(_name) || _name.Length < ValidationOptions.MinNameLength;

            if (isNotValidName)
            {
                return input;
            }

            Expression<Func<Product, bool>> filter = product =>
                product.ProductName.Contains(_name) || _keysWhereExistLocalization.Contains(product.Key);

            if (input == null)
            {
                return filter;
            }

            var newChain = input.AndAlso(filter);

            return newChain;
        }
    }
}