using Microsoft.AspNetCore.Mvc;
using pdfood.api.Models.Files;
using pdfood.api.Repository.Files;

namespace pdfood.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        private readonly IImageUploadRepository _repository;

        public ImageUploadController(IImageUploadRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id:int}")]
        public ActionResult<string> GetImage(int id)
        {
            try
            {
                var imageUpload = _repository.GetById(id);

                if (imageUpload == null)
                {
                    return BadRequest("Imagem não encontrada");
                }

                if (imageUpload.Base64 == null)
                {
                    return BadRequest("Não foi possível carregar a imagem");
                }

                string imreBase64Data = Convert.ToBase64String(imageUpload.Base64);
                string image = string.Format("data:image/png;base64,{0}", imreBase64Data);

                return Ok(new {DataUrl = image});

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ImageUpload>> UploadImage([FromForm] IFormFile formFile)
        {
            try
            {
                if (formFile == null || formFile.Length == 0)
                {
                    return BadRequest("Não foi possível carregar a imagem");
                }

                byte[] imageByte;

                using (var ms = new MemoryStream())
                {
                    await formFile.CopyToAsync(ms);
                    imageByte = ms.ToArray();

                }

                var imageUpload = await _repository.SaveImageAsync(new ImageUpload
                {
                    Name = formFile.Name,
                    Base64 = imageByte
                });

                return Ok(imageUpload);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
