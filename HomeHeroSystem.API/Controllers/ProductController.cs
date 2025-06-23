using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Models.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeHeroSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetProductsAsync([FromQuery] GetProductRequest request)
        {
            try
            {
                var result = await _productService.GetProductsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductDetailAsync(int id)
        {
            try
            {
                var result = await _productService.GetProductDetailAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
