using ShopServiceApi.Application.DTOs.Site.Order;
using ShopServiceApi.Domain.Entities.Orders;
using ShopServiceApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Application.Services.Orders.Interface
{
    public interface IOrderService
    {
        // سبد خرید
        Task<OrderResponseDto> AddProductToOrderAsync(string userId, OrderRequestDto request);
        public Task RemoveProductFromOrderAsync(string userId, Guid productId);
        public Task<Order?> GetPendingOrderAsync(string userId);
        Task ChangeOrderStatusAsync(Guid orderId, OrderStatus status);


        // Checkout
        Task<Order?> CheckoutAsync(string userId);

        // Admin
        Task<Order?> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
    }
}
