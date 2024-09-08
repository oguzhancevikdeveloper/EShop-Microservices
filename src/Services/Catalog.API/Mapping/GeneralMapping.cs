using AutoMapper;

namespace Catalog.API.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Models.Product,DTOs.Product.ProductCreatedDto>().ReverseMap();
            CreateMap<Models.Product,DTOs.Product.ProductUpdatedDto>().ReverseMap();
            CreateMap<Models.Product,DTOs.Product.ProductDto>().ReverseMap();

            CreateMap<Models.Category,DTOs.Category.CategoryCreatedDto>().ReverseMap();
            CreateMap<Models.Category,DTOs.Category.CategoryDto>().ReverseMap();
            CreateMap<Models.Category,DTOs.Category.CategoryUpdatedDto>().ReverseMap();
        }
    }
}
