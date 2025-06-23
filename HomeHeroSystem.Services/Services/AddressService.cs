using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;
using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Models.Address;

namespace HomeHeroSystem.Services.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AddressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<string>> GetDistrictsAsync()
        {
            try
            {
                // Lấy danh sách các quận/huyện duy nhất từ database
                var districts = await _unitOfWork.Addresses.GetDistinctDistrictsAsync();

                

                return districts.OrderBy(d => d).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting districts: {ex.Message}");
            }
        }
        public async Task<List<string>> GetWardsByDistrictAsync(string district)
        {
            try
            {
                if (string.IsNullOrEmpty(district))
                {
                    throw new ArgumentException("District cannot be null or empty");
                }

                var wards = await _unitOfWork.Addresses.GetWardsByDistrictAsync(district);


                return wards.OrderBy(w => w).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting wards for district {district}: {ex.Message}");
            }
        }
        public async Task<AddressResponse> CreateOrGetAddressAsync(CreateAddressRequest request)
        {
            try
            {
                // Validate input
                if (string.IsNullOrEmpty(request.Street?.Trim()) ||
                    string.IsNullOrEmpty(request.Ward?.Trim()) ||
                    string.IsNullOrEmpty(request.District?.Trim()))
                {
                    throw new ArgumentException("Street, Ward, and District are required");
                }

                // Tìm địa chỉ đã tồn tại
                var existingAddress = await _unitOfWork.Addresses.FindAddressAsync(
                    request.Street.Trim(),
                    request.Ward.Trim(),
                    request.District.Trim(),
                    request.City?.Trim() ?? "Ho Chi Minh City"
                );

                if (existingAddress != null)
                {
                    return _mapper.Map<AddressResponse>(existingAddress);
                }

                // Tạo địa chỉ mới
                var newAddress = new Address
                {
                    Street = request.Street.Trim(),
                    Ward = request.Ward.Trim(),
                    District = request.District.Trim(),
                    City = request.City?.Trim() ?? "Ho Chi Minh City",
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };

                _unitOfWork.Addresses.AddEntity(newAddress);
                await _unitOfWork.CompleteAsync();

                return _mapper.Map<AddressResponse>(newAddress);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating/getting address: {ex.Message}");
            }
        }
        public async Task<AddressResponse?> GetAddressByIdAsync(int id)
        {
            try
            {
                var address = await _unitOfWork.Addresses.GetByIdAsync(id);
                if (address == null || address.IsDeleted == true)
                {
                    return null;
                }

                return _mapper.Map<AddressResponse>(address);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting address by ID {id}: {ex.Message}");
            }
        }

        public async Task<AddressResponse?> FindAddressAsync(string street, string ward, string district, string city)
        {
            try
            {
                var address = await _unitOfWork.Addresses.FindAddressAsync(
                    street?.Trim() ?? "",
                    ward?.Trim() ?? "",
                    district?.Trim() ?? "",
                    city?.Trim() ?? "Ho Chi Minh City"
                );

                if (address == null || address.IsDeleted == true)
                {
                    return null;
                }

                return _mapper.Map<AddressResponse>(address);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error finding address: {ex.Message}");
            }
        }

    }
}
