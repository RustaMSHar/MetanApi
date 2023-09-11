using MetanApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.IO;
using System.Threading.Tasks;

namespace MetanApi.Services
{
    public class ImageService
    {
        private readonly IMongoDatabase _database;
        private readonly IGridFSBucket _gridFSBucket;

        public ImageService(IOptions<StoreDatabaseSettings> storeDatabaseSettings)
        {
            var mongoClient = new MongoClient(storeDatabaseSettings.Value.ConnectionString);
            _database = mongoClient.GetDatabase(storeDatabaseSettings.Value.ImageDatabaseName);
            _gridFSBucket = new GridFSBucket(_database);

        }

        public async Task<byte[]> GetImageAsync(string id)
        {
            var imageStream = new MemoryStream();
            await _gridFSBucket.DownloadToStreamAsync(new ObjectId(id), imageStream);
            return imageStream.ToArray();
        }

        public async Task<string> UploadImageAsync(string fileName, Stream imageStream, string contentType)
        {
            var objectId = await _gridFSBucket.UploadFromStreamAsync(fileName, imageStream, new GridFSUploadOptions
            {
                Metadata = new BsonDocument("contentType", contentType)
            });
            return objectId.ToString();
        }

        public async Task DeleteImageAsync(string id)
        {
            await _gridFSBucket.DeleteAsync(new ObjectId(id));
        }
    }
}
