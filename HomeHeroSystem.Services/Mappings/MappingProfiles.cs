using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Services.Models.Address;
using HomeHeroSystem.Services.Models.Booking;

namespace HomeHeroSystem.Services.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region
            CreateMap<Booking, GetBookingByIdResponse>()
    .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.CustomerName) ? src.CustomerName : src.User.FullName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone))
                .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.CustomerPhone))

                // Service Information
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.ServiceName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Service.Price))

                // Technician Information - handle null technician
                .ForMember(dest => dest.TechnicianName, opt => opt.MapFrom(src =>
                    src.Technician != null ? src.Technician.FullName : "Chưa phân công"))

                // Booking Information - direct mapping
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.BookingDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.PreferredTimeSlot, opt => opt.MapFrom(src => src.PreferredTimeSlot))
                .ForMember(dest => dest.ProblemDescription, opt => opt.MapFrom(src => src.ProblemDescription))
                .ForMember(dest => dest.UrgencyLevel, opt => opt.MapFrom(src => src.UrgencyLevel))
                .ForMember(dest => dest.UrgencyFee, opt => opt.MapFrom(src => src.UrgencyFee))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))

                // Address Information - handle null address
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address != null ?
                    $"{src.Address.Street}, {src.Address.Ward}, {src.Address.District}, {src.Address.City}" : "N/A"))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address != null ? src.Address.Street : ""))
                .ForMember(dest => dest.Ward, opt => opt.MapFrom(src => src.Address != null ? src.Address.Ward : ""))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.Address != null ? src.Address.District : ""))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address != null ? src.Address.City : ""));

            CreateMap<Booking, CreateBookingRequest>().ReverseMap();
            CreateMap<Booking, UpdateBookingRequest>().ReverseMap();
            #endregion

            #region Address Mappings
            CreateMap<Address, AddressResponse>()
                .ForMember(dest => dest.FullAddress, opt => opt.MapFrom(src =>
                    $"{src.Street}, {src.Ward}, {src.District}, {src.City}"));

            CreateMap<CreateAddressRequest, Address>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));
            #endregion
        }

    }
}
