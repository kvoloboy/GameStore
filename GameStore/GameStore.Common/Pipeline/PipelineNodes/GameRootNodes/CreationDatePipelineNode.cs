using System;
using System.Linq.Expressions;
using GameStore.Common.Extensions;
using GameStore.Common.Models;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.Core.Models;

namespace GameStore.Common.Pipeline.PipelineNodes.GameRootNodes
{
    public class CreationDatePipelineNode : IPipelineNode<GameRoot>
    {
        private readonly string _creationDate;

        public CreationDatePipelineNode(string creationDate)
        {
            _creationDate = creationDate;
        }
        
        public Expression<Func<GameRoot, bool>> Execute(Expression<Func<GameRoot, bool>> input)
        {
            if (string.IsNullOrEmpty(_creationDate))
            {
                return input;
            }
            
            var minDate = GetMinDate();
            
            Expression<Func<GameRoot, bool>> filter = root => root.Details.CreationDate >= minDate || root.Details == null;

            if (input == null)
            {
                return filter;
            }
            
            var newChain = input.AndAlso(filter);
            
            return newChain;
        }
        
        private DateTime GetMinDate()
        {
            var minDate = _creationDate switch
            {
                CreationTerm.LastWeek => DateTime.UtcNow.AddDays(-Period.Week),
                CreationTerm.LastMonth => DateTime.UtcNow.AddDays(-Period.Month),
                CreationTerm.LastYear => DateTime.UtcNow.AddDays(-Period.Year),
                CreationTerm.TwoYears => DateTime.UtcNow.AddDays(-Period.TwoYears),
                CreationTerm.ThreeYears => DateTime.UtcNow.AddDays(-Period.ThreeYears),
                _ => default
            };

            return minDate;
        }
    }
}