using AutoMapper;
using SkillStackCSharp.DTOs.ProductDTOs;
using SkillStackCSharp.Models;

namespace SkillStackCSharp.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ProductMapping();
        }

        private void ProductMapping()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<CreateProductDTO, Product>()
                .AfterMap((src, dest) => dest.Id = Guid.NewGuid().ToString());
            CreateMap<UpdateProductDTO, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
