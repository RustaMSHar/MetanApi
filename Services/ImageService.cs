using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MetanApi.Models;
using MongoDB.Bson;

namespace MetanApi.Services
{
    public class ImageService
    {
        private readonly IMongoDatabase _database;
        private readonly IGridFSBucket _gridFSBucket;

        public ImageService(IOptions<StoreDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.ImageDatabaseName);
            _gridFSBucket = new GridFSBucket(_database);
        }

        public async Task<byte[]> GetImageBytesAsync(string id)
        {
            if (ObjectId.TryParse(id, out ObjectId objectId))
            {
                var fileStream = await _gridFSBucket.OpenDownloadStreamAsync(objectId);
                using (var memoryStream = new MemoryStream())
                {
                    await fileStream.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            return null;
        }

        public async Task<ObjectId> UploadImageAsync(string fileName, Stream stream)
        {
            var options = new GridFSUploadOptions
            {
                Metadata = new BsonDocument("filename", fileName)
            };
            return await _gridFSBucket.UploadFromStreamAsync(fileName, stream, options);
        }

        public async Task<(bool Success, string ErrorMessage)> DeleteImageAsync(string id)
        {
            if (ObjectId.TryParse(id, out ObjectId objectId))
            {
                try
                {
                    await _gridFSBucket.DeleteAsync(objectId);
                    return (true, null); 
                }
                catch (Exception ex)
                {
                    return (false, ex.Message); 
                }
            }
            return (false, "Invalid ObjectId"); 
        }


    }
}
