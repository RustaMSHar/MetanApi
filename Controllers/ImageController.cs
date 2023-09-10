using Microsoft.AspNetCore.Mvc;
using MetanApi.Services;
using System.IO;
using System.Threading.Tasks;

namespace MetanApi.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ImageController : ControllerBase
    {
        private readonly ImageService _imageService;

        public ImageController(ImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetImage(string id)
        {
            var imageBytes = await _imageService.GetImageAsync(id);
            if (imageBytes == null)
            {
                return NotFound();
            }

            return File(imageBytes, "image/jpeg"); // Измените тип содержимого на соответствующий вашим изображениям
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var id = await _imageService.UploadImageAsync(file.FileName, memoryStream, file.ContentType);
                return CreatedAtAction(nameof(GetImage), new { id });
            }
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteImage(string id)
        {
            await _imageService.DeleteImageAsync(id);
            return NoContent();
        }
    }
}
