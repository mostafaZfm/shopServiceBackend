using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopServiceApi.Application.DTOs.Site.Order;
using ShopServiceApi.Application.Services.Orders;
using ShopServiceApi.Application.Services.Orders.Interface;
using ShopServiceApi.Infrastructure.Site.Enum;
using ShopServiceApi.Infrastructure.Site.Orders;
using System.Security.Claims;

namespace ShopServiceApi.Api.Controllers
{
    [Authorize] // حتما روی controller یا action باشه
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
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
        [HttpPost("add")]
        public async Task<IActionResult> AddProductToCart([FromBody] OrderRequestDto request)
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _orderService.AddProductToOrderAsync(userId, request);
            return Ok(result);
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
