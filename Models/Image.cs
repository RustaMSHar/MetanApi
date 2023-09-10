using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MetanApi.Models
{
    public class Image
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string FileName { get; set; } = null!;
        public byte[] Data { get; set; } = null!;
        public string ContentType { get; set; } = null!;
    }
}
