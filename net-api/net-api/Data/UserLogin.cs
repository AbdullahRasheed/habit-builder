using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace net_api.Data
{
    public class UserLogin
    {

        [BsonId]
        public ObjectId Id { get; set; }

        public string Username { get; set; } = null!;

        public byte[] PasswordHash { get; set; } = new byte[0];

        public byte[] PasswordSalt { get; set; } = new byte[0];

        public RefreshToken RefreshToken { get; set; } = null!;
    }
}
