using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Services.Models.ServiceCategory;

namespace HomeHeroSystem.Services.Interfaces
{
    public interface IServiceCategoryService
    {
        Task<IEnumerable<ServiceCategoryDto>> GetAllCategoriesAsync();
        Task<ServiceCategoryDto> GetCategoryByIdAsync(int id);
    }
}
