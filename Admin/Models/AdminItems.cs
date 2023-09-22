using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MetanApi.Admin.Models
{
    public class AdminItems
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }

    }
}
