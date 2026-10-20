using Microsoft.AspNetCore.Mvc;
using ShopServiceApi.Application.DTOs.Site.Products.Categories;
using ShopServiceApi.Application.Services.Products.Interface;

namespace ShopServiceApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CategoriesController : ControllerBase
    {
        private readonly IProductService _service;

        public CategoriesController(IProductService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto request)
        {
            var created = await _service.CreateAsync(request);

            return StatusCode(
                StatusCodes.Status201Created,
                created);
        }
    }
}
