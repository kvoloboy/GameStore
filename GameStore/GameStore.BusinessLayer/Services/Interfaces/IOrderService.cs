using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.BusinessLayer.DTO;
using GameStore.Common.Models;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Services.Interfaces
{
    public interface IOrderService
    {
        Task SetShipmentDetailsAsync(ShipmentDto shipmentDto);
        Task UpdateShipmentDetailsAsync(ShipmentDto shipmentDto);
        Task<OrderDto> GetByIdAsync(string id, string culture = Culture.En);
        Task<ShipmentDto> GetShipmentDetailsAsync(string orderId);
        Task<IEnumerable<OrderDto>> GetByUserIdAsync(string userId, string culture = Culture.En);
        Task<IEnumerable<OrderDto>> GetByDateRangeAsync(DateTime minDate, DateTime maxDate, string culture = Culture.En);
        Task<OrderDto> FindSingleAsync(Expression<Func<Order, bool>> predicate, string culture = Culture.En);
        Task<OrderDto> GetNewOrderByUserIdAsync(string userId, string culture = Culture.En);
        Task SetNewStateWhenOrderedAsync(string orderId);
        Task ConfirmAsync(string orderId);
        Task MergeOrdersAsync(string oldUserId, string newUserId);
        int ComputeProductsQuantity(IEnumerable<OrderDetailsDto> detailsDto);
        decimal ComputeTotal(IEnumerable<OrderDetailsDto> detailsDto);
        Task SetStateAsync(string orderId, OrderState state);
        bool CanBeCanceled(OrderState orderState);
        bool CanBeUpdated(OrderState orderState, DateTime orderDate);
        Task UpdateAsync(OrderDto orderDto);
    }
}