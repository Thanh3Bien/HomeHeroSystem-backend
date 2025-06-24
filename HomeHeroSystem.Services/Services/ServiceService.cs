using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Infrastructures;
using HomeHeroSystem.Repositories.Interfaces;
using HomeHeroSystem.Repositories.Repositories;
using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Models.Service;
using HomeHeroSystem.Services.Models.ServiceCategory;

namespace HomeHeroSystem.Services.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private const decimal URGENCY_FEE = 50000;
        public ServiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<string>> GetServiceNamesAsync()
        {
            try
            {
                var serviceNames = await _unitOfWork.Services.GetServiceNamesAsync();
                return serviceNames.Where(name => !string.IsNullOrEmpty(name));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<string>> SearchServiceNamesAsync(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return await GetServiceNamesAsync();
                }

                var filteredNames = await _unitOfWork.Services.SearchServiceNamesAsync(keyword.Trim());
                return filteredNames.Where(name => !string.IsNullOrEmpty(name));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ServiceDto>> GetServicesByCategoryAsync(int categoryId)
        {
            try
            {
                var services = await _unitOfWork.Services.GetActiveByCategoryIdAsync(categoryId);

                if (!services.Any())
                {
                    return null;
                }

                var serviceDtos = services.Select(s => new ServiceDto
                {
                    ServiceId = s.ServiceId,
                    CategoryId = s.CategoryId,
                    ServiceName = s.ServiceName,
                    Description = s.Description,
                    Price = s.Price,
                    FormattedPrice = $"{s.Price:N0} ₫",
                    EstimatedTime = s.EstimatedTime ?? 60,
                    EstimatedTimeText = FormatEstimatedTime(s.EstimatedTime ?? 60),
                    IsActive = (bool)s.IsActive
                }).ToList();

                return serviceDtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ServicePriceDto> GetServicePriceAsync(int serviceId, string urgencyLevel = "normal")
        {
            try
            {
                var service = await _unitOfWork.Services.GetByIdAsync(serviceId);

                if (service == null || service.IsActive != true)
                {
                    return null;
                }

                var basePrice = service.Price;
                var urgencyFee = urgencyLevel.ToLower() == "urgent" ? URGENCY_FEE : 0;
                var totalPrice = basePrice + urgencyFee;

                var priceDto = new ServicePriceDto
                {
                    ServiceId = service.ServiceId,
                    ServiceName = service.ServiceName,
                    BasePrice = basePrice,
                    UrgencyFee = urgencyFee,
                    TotalPrice = totalPrice,
                    FormattedBasePrice = $"{basePrice:N0} ₫",
                    FormattedUrgencyFee = urgencyFee > 0 ? $"{urgencyFee:N0} ₫" : "0 ₫",
                    FormattedTotalPrice = $"{totalPrice:N0} ₫",
                    UrgencyLevel = urgencyLevel
                };

                return priceDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ServiceDto> GetServiceByIdAsync(int serviceId)
        {
            try
            {
                var service = await _unitOfWork.Services.GetByIdAsync(serviceId);

                if (service == null)
                {
                    return null;
                }

                var serviceDto = new ServiceDto
                {
                    ServiceId = service.ServiceId,
                    CategoryId = service.CategoryId,
                    ServiceName = service.ServiceName,
                    Description = service.Description,
                    Price = service.Price,
                    FormattedPrice = $"{service.Price:N0} ₫",
                    EstimatedTime = service.EstimatedTime ?? 60,
                    EstimatedTimeText = FormatEstimatedTime(service.EstimatedTime ?? 60),
                    IsActive = (bool)service.IsActive
                };

                return serviceDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string FormatEstimatedTime(int minutes)
        {
            if (minutes < 60)
                return $"{minutes} phút";

            var hours = minutes / 60;
            var remainingMinutes = minutes % 60;

            if (remainingMinutes == 0)
                return $"{hours} giờ";

            return $"{hours} giờ {remainingMinutes} phút";
        }
    }
}
