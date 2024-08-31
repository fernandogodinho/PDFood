using pdfood.api.DTO;
using pdfood.api.DTO.Products;
using pdfood.api.Models.Products;

namespace pdfood.api.Services.Products
{
    public interface IProductService
    {
        Pageable<ProductDTO> GetAll(int? limit, int? offset, string? name, string? barCode, string? sort);
        Product? GetById(long id);
        Task UpdateAsync(long id, ProductDTO productDTO);
        Task<Product> CreateAsync(ProductDTO product);
        void Delete(long id);
    }
}
