using AutoMapper;
using MyAspNetBackend.Models;
using MyAspNetBackend.DTOs;

namespace MyAspNetBackend.Mapping  // Changed from DTOs namespace
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity to DTO mapping (outgoing data)
            CreateMap<Customer, CustomerDTO>()
                .ForMember(dest => dest.full_name,
                    opt => opt.MapFrom(src => src.FullName)) // Changed from Name to FullName
                .ForMember(dest => dest.phone_number, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.customer_id, opt => opt.MapFrom(src => src.CustomerId));
            
            // DTO to Entity mapping (incoming data)
            CreateMap<CustomerDTO, Customer>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());  // Removed the extra semicolon
        }
    }
}