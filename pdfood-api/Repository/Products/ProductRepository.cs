using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pdfood.api.DTO;
using pdfood.api.DTO.Products;
using pdfood.api.Models.Products;

namespace pdfood.api.Repository.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        
        public ProductRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public void Delete(long id)
        {
            var product = GetById(id) ?? throw new Exception("Produto não encontrado");

            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public Pageable<ProductDTO> GetAll(int? limit, int? offset, string? name, string? barCode, string? sortByPrice)
        {
            if (limit == null || limit == 0)
            {
                limit = 20;
            }

            var internalOffset = offset ?? 0;
            int internalLimit = limit ?? 0;
            var hasNext = false;
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(product => product.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(barCode))
            {
                query = query.Where(product => product.BarCode.Contains(barCode));
            }

            query = query.OrderBy(p => p.Id);
            query = query.Include(p => p.Image);

            if (!string.IsNullOrEmpty(sortByPrice))
            {
                query = sortByPrice.ToUpper() == "DESC" 
                    ? query.OrderByDescending(p => p.Price)
                    : query.OrderBy(p => p.Price);
            }
            
            var content = query.Skip(internalOffset)
                .Take(internalLimit + 1)
                .ToList();

            if (content.Count > internalLimit)
            {
                hasNext = true;
                content.RemoveAt(content.Count - 1);
            }

            var contentToList = new List<ProductDTO>();

            content.ForEach(product => contentToList.Add(_mapper.Map<ProductDTO>(product)));

            return new Pageable<ProductDTO>
            {
                Content = contentToList,
                Offset = internalOffset,
                HasNext = hasNext,
                Limit = internalLimit
            };
        }

        public Product? GetById(long id)
        {
            return _context.Products
                .Where(product => product.Id == id)
                .FirstOrDefault();
        }

        public async Task UpdateAsync(Product product)
        {
            var existingProduct = GetById(product.Id) ?? throw new Exception("Produto não encontrado");

            existingProduct.Name = product.Name;
            existingProduct.BarCode = product.BarCode;
            existingProduct.Price = product.Price;
            existingProduct.Image = product.Image;
            existingProduct.FakeStoreId = product.FakeStoreId;

            await _context.SaveChangesAsync();
        }
    }
}
