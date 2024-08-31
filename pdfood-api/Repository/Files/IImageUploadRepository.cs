using pdfood.api.DTO.Files;
using pdfood.api.Models.Files;

namespace pdfood.api.Repository.Files
{
    public interface IImageUploadRepository
    {
        Task<ImageUploadDTO> SaveImageAsync(ImageUpload imageUpload);
        public ImageUpload? GetById(long id);
    }
}
