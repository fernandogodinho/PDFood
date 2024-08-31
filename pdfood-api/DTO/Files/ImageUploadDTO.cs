namespace pdfood.api.DTO.Files
{
    public class ImageUploadDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public byte[]? Base64 { get; set; }

        public string Url { get; set; }
    }
}
