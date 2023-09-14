using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

[Route("api/images")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IGridFSBucket _gridFSBucket;

    public ImageController(IGridFSBucket gridFSBucket)
    {
        _gridFSBucket = gridFSBucket;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetImage(string id)
    {
        if (ObjectId.TryParse(id, out ObjectId objectId))
        {
            var fileStream = await _gridFSBucket.OpenDownloadStreamAsync(objectId);
            return File(fileStream, fileStream.FileInfo.ContentType);
        }
        else
        {
            return BadRequest("Invalid ObjectId");
        }
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            using (var stream = file.OpenReadStream())
            {
                var options = new GridFSUploadOptions
                {
                    Metadata = new BsonDocument("filename", file.FileName)
                };

                ObjectId objectId = await _gridFSBucket.UploadFromStreamAsync(file.FileName, stream, options);
                return Ok($"File uploaded with ObjectId: {objectId}");
            }
        }
        else
        {
            return BadRequest("No file in the request");
        }
    }
}
