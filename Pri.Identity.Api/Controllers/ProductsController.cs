using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Pri.Identity.Api.Dtos;

namespace Pri.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly List<ProductDto> _products;

        public ProductsController()
        {
            _products = new List<ProductDto>();
            GenerateSomeProducts();
        }
        [HttpGet]
        [Authorize(Policy ="Admin")]
        public IActionResult Get()
        {
            return Ok(_products);
        }
        [HttpGet("loyalmembers")]
        [Authorize(Policy = "OnlyLoyalMembers")]
        public IActionResult GetProductsForLoyalMembers()
        {
            return Ok("A list of products for Loyal Members!");
        }

        private void GenerateSomeProducts()
        {
            _products.Add(new ProductDto { Id = Guid.NewGuid(), Name = "Product 1", Price = 23.45m });
            _products.Add(new ProductDto { Id = Guid.NewGuid(), Name = "Product 2", Price = 43.45m });
            _products.Add(new ProductDto { Id = Guid.NewGuid(), Name = "Product 3", Price = 63.45m });
        }
        

    }
}
