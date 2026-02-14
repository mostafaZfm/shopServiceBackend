using ShopServiceApi.Infrastructure.Site.Enum;
using ShopServiceApi.Infrastructure.Site.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Application.Services.Orders.Interface
{
    public interface IOrderService
    {
        // سبد خرید
        public Task AddProductToOrderAsync(string userId, Guid productId, int quantity = 1);
        public Task RemoveProductFromOrderAsync(string userId, Guid productId);
        public Task<Order?> GetPendingOrderAsync(string userId);

        // Checkout
        Task<Order?> CheckoutAsync(string userId);

        // Admin
        Task<Order?> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
    }
}
