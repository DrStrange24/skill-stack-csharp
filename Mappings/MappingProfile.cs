using AutoMapper;
using SkillStackCSharp.DTOs.ProductDTOs;
using SkillStackCSharp.DTOs.UserDTOs;
using SkillStackCSharp.Models;

namespace SkillStackCSharp.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ProductMapping();
            UserMapping();

            CreateMap<UpdateProfileDTO, UpdateUserDTO>();
        }

        private void ProductMapping()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<CreateProductDTO, Product>()
                .AfterMap((src, dest) => dest.Id = Guid.NewGuid().ToString());
            CreateMap<UpdateProductDTO, Product>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }

        private void UserMapping()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<CreateUserDTO, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            CreateMap<UpdateUserDTO, User>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
