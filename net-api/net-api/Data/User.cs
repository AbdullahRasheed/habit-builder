using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace net_api.Data
{
    public class User
    {

        [BsonId]
        public ObjectId Id { get; set; }

        public string Username { get; set; } = null!;

        public List<Habit> Habits { get; set; } = null!;
    }
}
