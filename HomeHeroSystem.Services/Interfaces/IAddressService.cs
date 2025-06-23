using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Services.Models.Address;

namespace HomeHeroSystem.Services.Interfaces
{
    public interface IAddressService
    {
        Task<List<string>> GetDistrictsAsync();
        Task<List<string>> GetWardsByDistrictAsync(string district);
        Task<AddressResponse> CreateOrGetAddressAsync(CreateAddressRequest request);
        Task<AddressResponse?> GetAddressByIdAsync(int id);
        Task<AddressResponse?> FindAddressAsync(string street, string ward, string district, string city);

    }
}
