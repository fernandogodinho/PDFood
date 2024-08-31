using pdfood.api.Models.Products;

namespace pdfood.api.DTO
{
    public class Pageable<T>
    {
        public List<T> Content { get; set; }
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 20;
        public bool HasNext { get; set; } = false;
    }
}
