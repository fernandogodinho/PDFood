using pdfood.api.DTO;
using pdfood.api.DTO.Products;
using pdfood.api.Models.Products;

namespace pdfood.api.Repository.Products
{
    public interface IProductRepository
    {
        Pageable<ProductDTO> GetAll(int? limit, int? offset, string? name, string? barCode, string? sortByPrice);
        Product? GetById(long id);
        Task UpdateAsync(Product product);
        Task<Product> CreateAsync(Product product);
        void Delete(long id);
    }
}
