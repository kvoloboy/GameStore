using System;
using System.Linq.Expressions;

namespace GameStore.Common.Pipeline.PipelineNodes.Interfaces
{
    public interface IPipelineNode<T> where T : class
    {
        Expression<Func<T, bool>> Execute(Expression<Func<T, bool>> input);
    }
}