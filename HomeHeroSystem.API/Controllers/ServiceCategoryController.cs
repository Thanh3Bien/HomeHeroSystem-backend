using HomeHeroSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeHeroSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceCategoryController : ControllerBase
    {
        private readonly IServiceCategoryService _serviceCategoryService;
        public ServiceCategoryController(IServiceCategoryService serviceCategoryService)
        {
            _serviceCategoryService = serviceCategoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetServiceCategories()
        {
            try
            {
                var result = await _serviceCategoryService.GetAllCategoriesAsync();
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
            

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceCategory(int id)
        {
            try
            {
                var result = await _serviceCategoryService.GetCategoryByIdAsync(id);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
            

        }
    }
}
