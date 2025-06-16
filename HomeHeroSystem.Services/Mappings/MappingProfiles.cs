using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Services.Models.Booking;

namespace HomeHeroSystem.Services.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region
            CreateMap<Booking, GetBookingByIdResponse>()
    .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.User.FullName)) 
    .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone))
    .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.ServiceName))
    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Service.Price))
    .ForMember(dest => dest.TechnicianName, opt => opt.MapFrom(src => src.Technician.FullName)) 
    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address.Street));

            CreateMap<Booking, CreateBookingRequest>().ReverseMap();
            CreateMap<Booking, UpdateBookingRequest>().ReverseMap();    
            #endregion
        }

    }
}
