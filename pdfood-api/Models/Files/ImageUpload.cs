using System.ComponentModel.DataAnnotations;

namespace pdfood.api.Models.Files
{
    public class ImageUpload
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public byte[]? Base64 { get; set; }
    }
}
