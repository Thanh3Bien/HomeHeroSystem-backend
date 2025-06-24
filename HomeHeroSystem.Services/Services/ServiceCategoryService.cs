using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Infrastructures;
using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Models.ServiceCategory;

namespace HomeHeroSystem.Services.Services
{
    public class ServiceCategoryService : IServiceCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServiceCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ServiceCategoryDto>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _unitOfWork.ServiceCategories.GetActiveAsync();

                var categoryDtos = new List<ServiceCategoryDto>();

                foreach (var category in categories)
                {
                    var services = await _unitOfWork.Services.GetActiveByCategoryIdAsync(category.CategoryId);
                    var minPrice = services.Any() ? services.Min(s => s.Price) : 0;
                    var serviceCount = services.Count();

                    categoryDtos.Add(new ServiceCategoryDto
                    {
                        CategoryId = category.CategoryId,
                        CategoryName = category.CategoryName,
                        Description = category.Description,
                        IconUrl = category.IconUrl,
                        IsActive = (bool)category.IsActive,
                        BasePrice = minPrice > 0 ? $"Từ {minPrice:N0} ₫" : "Liên hệ",
                        ServiceCount = serviceCount
                    });
                }

                return categoryDtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ServiceCategoryDto> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _unitOfWork.ServiceCategories.GetByIdAsync(id);

                if (category == null || category.IsActive != true)
                {
                    return null;
                }

                var services = await _unitOfWork.Services.GetActiveByCategoryIdAsync(category.CategoryId);
                var minPrice = services.Any() ? services.Min(s => s.Price) : 0;

                var categoryDto = new ServiceCategoryDto
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    Description = category.Description,
                    IconUrl = category.IconUrl,
                    IsActive = (bool)category.IsActive,
                    BasePrice = minPrice > 0 ? $"Từ {minPrice:N0} ₫" : "Liên hệ",
                    ServiceCount = services.Count()
                };

                return categoryDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
