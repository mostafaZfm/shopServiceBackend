using Microsoft.EntityFrameworkCore;
using ShopServiceApi.Application.DTOs.Site.Order;
using ShopServiceApi.Application.Services.Orders.Interface;
using ShopServiceApi.Infrastructure.Data;
using ShopServiceApi.Infrastructure.Site.Enum;
using ShopServiceApi.Infrastructure.Site.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Application.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderResponseDto> AddProductToOrderAsync(string userId, OrderRequestDto request)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.OrderStatus == OrderStatus.Pending);

            if (order == null)
            {
                order = new Order
                {
                    UserId = userId,
                    OrderStatus = OrderStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    Items = new List<OrderItem>()
                };

                _context.Orders.Add(order);
            }

            var existingItem = order.Items
                .FirstOrDefault(i => i.ProductId == request.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                var product = await _context.Products.FindAsync(request.ProductId);

                if (product == null)
                    throw new Exception("Product not found");

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = request.Quantity,
                    UnitPrice = product.Price
                });
            }

            await _context.SaveChangesAsync();

            return await MapToResponseDto(order);
        }
        public async Task<Order?> CheckoutAsync(string userId)
        {
            var order = await _context.Orders
                       .Include(o => o.Items)
                       .FirstOrDefaultAsync(o => o.UserId == userId && o.OrderStatus == OrderStatus.Pending);

            if (order == null || !order.Items.Any())
                throw new Exception("Cart is empty");

            order.OrderStatus = OrderStatus.Paid;
            order.PaidAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                       .Include(o => o.Items)
                       .ThenInclude(i => i.Product)
                       .ToListAsync();
        }
        public async Task ChangeOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
                throw new Exception("Order not found");

            order.OrderStatus = status;

            if (status == OrderStatus.Paid)
                order.PaidAt = DateTime.UtcNow;

            if (status == OrderStatus.Shipped)
                order.ShippedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }


        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                       .Include(o => o.Items)
                       .ThenInclude(i => i.Product)
                       .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<Order?> GetPendingOrderAsync(string userId)
        {
            return await _context.Orders
                           .Include(o => o.Items)
                           .ThenInclude(i => i.Product)
                           .FirstOrDefaultAsync(o => o.UserId == userId && o.OrderStatus == OrderStatus.Pending);
        }

        public async Task RemoveProductFromOrderAsync(string userId, Guid productId)
        {
            var order = await _context.Orders
                          .Include(o => o.Items)
                          .FirstOrDefaultAsync(o => o.UserId == userId && o.OrderStatus == OrderStatus.Pending);

            if (order == null) return;

            var item = order.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                order.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public Task<Order?> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            throw new NotImplementedException();
        }

        private OrderResponseDto MapToResponseDto(Order order)
        {
            return new OrderResponseDto
            {
                Id = order.Id,
                UserId = order.UserId,
                Status = order.OrderStatus.ToString(),
                TotalPrice = order.TotalPrice,
                PaidAt = order.PaidAt,
                ShippedAt = order.ShippedAt,
                CompletedAt = order.CompletedAt,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };
        }

    }
}
