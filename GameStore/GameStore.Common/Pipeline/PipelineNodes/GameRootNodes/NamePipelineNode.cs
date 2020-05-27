using System;
using System.Linq;
using System.Linq.Expressions;
using GameStore.Common.Extensions;
using GameStore.Common.Models;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.Core.Models;

namespace GameStore.Common.Pipeline.PipelineNodes.GameRootNodes
{
    public class NamePipelineNode : IPipelineNode<GameRoot>
    {
        private readonly string _name;

        public NamePipelineNode(string name)
        {
            _name = name;
        }

        public Expression<Func<GameRoot, bool>> Execute(Expression<Func<GameRoot, bool>> input)
        {
            var isNotValid = string.IsNullOrEmpty(_name) || _name.Length < ValidationOptions.MinNameLength;

            if (isNotValid)
            {
                return input;
            }

            Expression<Func<GameRoot, bool>> filter = root =>
                root.Localizations.Any(l => l.Name.Contains(_name)) || root.Details == null;

            if (input == null)
            {
                return filter;
            }

            var newChain = input.AndAlso(filter);

            return newChain;
        }
    }
}