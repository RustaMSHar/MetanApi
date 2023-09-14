using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using MetanApi.Services;

[Route("api/images")]
public class ImageController : ControllerBase
{
    private readonly ImageService _imageService;

    public ImageController(ImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetImage(string id)
    {
        var imageBytes = await _imageService.GetImageBytesAsync(id);
        if (imageBytes != null)
        {
            return File(imageBytes, "image/jpeg"); 
        }
        return NotFound();
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            using (var stream = file.OpenReadStream())
            {
                var objectId = await _imageService.UploadImageAsync(file.FileName, stream);
                return Ok($"File uploaded with ObjectId: {objectId}");
            }
        }
        return BadRequest("No file in the request");
    }
}
