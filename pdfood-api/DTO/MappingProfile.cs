using pdfood.api.Models.Products;
using AutoMapper;
using pdfood.api.DTO.Products;
using pdfood.api.DTO.Files;
using pdfood.api.Models.Files;
using pdfood.api.DTO.FakeStore;

namespace pdfood.api.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ImageUploadDTO, ImageUpload>()
                .ForMember(image => image.Id, options => options.Ignore());

            CreateMap<ProductDTO, Product>()
                .ForMember(product => product.Image, options => options.Ignore())
                .ForMember(product => product.Image, options => options.MapFrom(productDto => productDto.Image))
                .ForMember(product => product.Id, options => options.Ignore());


            CreateMap<ImageUpload, ImageUploadDTO>()
                .ForMember(dto => dto.Url, opt => opt.MapFrom(iu => string.Format("data:image/png;base64,{0}", Convert.ToBase64String(iu.Base64))));
            
            CreateMap<Product, ProductListDTO>();

            CreateMap<Product, ProductDTO>()
                .ForMember(dto => dto.Image, opt => opt.MapFrom(p => GetImageUploadDTO(p.Image)));

            CreateMap<Product, FSProductDTO>()
                .ForMember(fs => fs.Id, opt => opt.MapFrom(p => p.FakeStoreId))
                .ForMember(fs => fs.Name, opt => opt.MapFrom(p => p.Name ?? GetNameDescriptionProduct(p.Id)))
                .ForMember(fs => fs.Description, opt => opt.MapFrom(p => p.Name ?? GetNameDescriptionProduct(p.Id)))
                .ForMember(fs => fs.Price, opt => opt.MapFrom(p => p.Price))
                .ForMember(fs => fs.Image, opt => opt.MapFrom(p => GetImageUrl(p.Image)))
                .ForMember(fs => fs.Category, opt => opt.MapFrom(src => "Geral"));
        }

        private String GetNameDescriptionProduct(long productId)
        {
            return "Produto - " + productId;
        }

        private String GetImageUrl(ImageUpload imageUpload) 
        {
            return "http://localhost:5055/imageUpload/" + imageUpload.Id;
        }

        private ImageUploadDTO GetImageUploadDTO(ImageUpload imageUpload) 
        {
            if (imageUpload == null)
            {
                return null;
            }

            ImageUploadDTO dto = new ImageUploadDTO();

            string imreBase64Data = Convert.ToBase64String(imageUpload.Base64);
            string imageUrl = string.Format("data:image/jpeg;base64,{0}", imreBase64Data);

            dto.Id = imageUpload.Id;
            dto.Name = imageUpload.Name;
            dto.Base64 = imageUpload.Base64;
            dto.Url = imageUrl;

            return dto;
        }
    }
}
