using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.Common.Extensions;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.Core.Models;

namespace GameStore.Common.Pipeline.PipelineNodes.GameRootNodes
{
    public class GenresPipelineNode : IPipelineNode<Core.Models.GameRoot>
    {
        private readonly IEnumerable<string> _genresId;

        public GenresPipelineNode(IEnumerable<string> genresId)
        {
            _genresId = genresId;
        }
        
        public Expression<Func<GameRoot, bool>> Execute(Expression<Func<GameRoot, bool>> input)
        {
            if (_genresId == null || !_genresId.Any())
            {
                return input;
            }
            
            Expression<Func<GameRoot, bool>> filter = game =>
                game.GameGenres.Any(gameGenre => _genresId.Contains(gameGenre.GenreId));

            if (input == null)
            {
                return filter;
            }

            var newChain = input.AndAlso(filter);

            return newChain;
        }
    }
}