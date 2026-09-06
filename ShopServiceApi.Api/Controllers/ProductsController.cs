using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopServiceApi.Application.DTOs.Site.Products;
using ShopServiceApi.Application.Services.Products;
using ShopServiceApi.Application.Services.Products.Interface;
using ShopServiceApi.Domain.Entities.Products;

namespace ShopServiceApi.Api.Controllers
{
    [Authorize]

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }
        // ====================
        // برای همه کاربران
        // ====================
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
                var products = await _service.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        // ====================
        // فقط ادمین
        // ====================
       // [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Product product)
        {
            var created = await _service.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Product product)
        {
            if (id != product.Id) return BadRequest();
            var updated = await _service.UpdateAsync(product);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetLazyLoadProduct([FromQuery] ProductQueryDto query)
        {
           return StatusCode(200,await _service.GetLazyLoadProduct(query));
        }
    }
}
