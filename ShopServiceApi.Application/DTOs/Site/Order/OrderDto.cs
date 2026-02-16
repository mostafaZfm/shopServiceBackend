using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Application.DTOs.Site.Order
{
    public class OrderResponseDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class OrderRequestDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
