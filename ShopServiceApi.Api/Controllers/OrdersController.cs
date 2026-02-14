using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopServiceApi.Application.Services.Orders;
using ShopServiceApi.Infrastructure.Site.Enum;
using ShopServiceApi.Infrastructure.Site.Orders;
using System.Security.Claims;

namespace ShopServiceApi.Api.Controllers
{
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }
        // ====================
        // Helper: گرفتن UserId از JWT
        // ====================
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }

        // ====================
        // افزودن محصول به سبد خرید
        // ====================
        [HttpPost("add/{productId:guid}")]
        public async Task<IActionResult> AddProductToCart(Guid productId, int quantity = 1)
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            await _orderService.AddProductToOrderAsync(userId, productId, quantity);
            var cart = await _orderService.GetPendingOrderAsync(userId);
            return Ok(cart);
        }

        // ====================
        // حذف محصول از سبد خرید
        // ====================
        [HttpDelete("remove/{productId:guid}")]
        public async Task<IActionResult> RemoveProductFromCart(Guid productId)
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            await _orderService.RemoveProductFromOrderAsync(userId, productId);
            var cart = await _orderService.GetPendingOrderAsync(userId);
            return Ok(cart);
        }

        // ====================
        // مشاهده سبد خرید فعلی
        // ====================
        [HttpGet("cart")]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var cart = await _orderService.GetPendingOrderAsync(userId);
            if (cart == null) return NotFound("Cart is empty");
            return Ok(cart);
        }
    }
}
