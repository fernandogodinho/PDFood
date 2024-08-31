using pdfood.api.Models.Files;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pdfood.api.Models.Products
{
    public class Product
    {
        [Key]
        public long Id { get; set; }
        public String Name { get; set; }
        public string BarCode { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("Image")]
        public long ImageId { get; set; }
        
        public ImageUpload Image { get; set; }
        public int FakeStoreId { get; set; }
    }
}
