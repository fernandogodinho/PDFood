using AutoMapper;
using pdfood.api.DTO.Files;
using pdfood.api.Models.Files;
using pdfood.api.Models.Products;

namespace pdfood.api.Repository.Files
{
    public class ImageUploadRepository : IImageUploadRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ImageUploadRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ImageUploadDTO> SaveImageAsync(ImageUpload imageUpload)
        {
            await _context.ImageUploads.AddAsync(imageUpload);
            await _context.SaveChangesAsync();

            return _mapper.Map<ImageUploadDTO>(imageUpload);
        }

        public ImageUpload? GetById(long id)
        {
            return _context.ImageUploads
                .Where(upload => upload.Id == id)
                .FirstOrDefault();
        }
    }
}
