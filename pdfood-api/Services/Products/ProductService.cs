using AutoMapper;
using pdfood.api.DTO;
using pdfood.api.DTO.FakeStore;
using pdfood.api.DTO.Products;
using pdfood.api.Models.Products;
using pdfood.api.Repository.Products;

namespace pdfood.api.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly FakeStoreService _fakeStoreService;

        public ProductService(IMapper mapper, IProductRepository productRepository, FakeStoreService fakeStoreService)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _fakeStoreService = fakeStoreService;
        }

        public async Task<Product> CreateAsync(ProductDTO productDTO)
        {
            var product = _mapper.Map<Product>(productDTO);
            var fSProduct = _mapper.Map<FSProductDTO>(product);

            var fSProductCreated = await _fakeStoreService.CreateProductAsync(fSProduct);

            product.FakeStoreId = fSProductCreated.Id;

            await _productRepository.CreateAsync(product);

            return product;
        }

        public void Delete(long id)
        {
            _productRepository.Delete(id);
        }

        public Pageable<ProductDTO> GetAll(int? limit, int? offset, string? name, string? barCode, string? sort)
        {
            return _productRepository.GetAll(limit, offset, name, barCode, sort);
        }

        public Product? GetById(long id)
        {
            return _productRepository.GetById(id);
        }

        public async Task UpdateAsync(long id, ProductDTO productDTO)
        {
            var productSaved = _productRepository.GetById(id) ?? throw new Exception("Produto não encontrado");
            var productToSave = _mapper.Map(productDTO, productSaved);

            productToSave.Id = id;

            var fSProduct = _mapper.Map<FSProductDTO>(productToSave);

            await _fakeStoreService.CreateProductAsync(fSProduct);
            await _productRepository.UpdateAsync(productToSave);
        }
    }
}
