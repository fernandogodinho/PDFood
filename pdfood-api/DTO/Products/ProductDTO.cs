using pdfood.api.DTO.Files;

namespace pdfood.api.DTO.Products
{
    public class ProductDTO
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public string BarCode { get; set; }
        public decimal Price { get; set; }
        public ImageUploadDTO Image { get; set; }
    }
}
