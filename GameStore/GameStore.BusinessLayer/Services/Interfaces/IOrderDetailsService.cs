using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.BusinessLayer.DTO;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Services.Interfaces
{
    public interface IOrderDetailsService
    {
        Task CreateAsync(OrderDetailsDto orderDetailsDto);
        Task UpdateAsync(OrderDetailsDto orderDetailsDto);
        Task DeleteAsync(string id);
        Task<OrderDetailsDto> GetByIdAsync(string id);
        Task<OrderDetails> FindSingleAsync(Expression<Func<OrderDetails, bool>> predicate);
    }
}