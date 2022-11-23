using MongoDB.Bson;

namespace net_api.Data
{
    public class Habit
    {

        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Rating { get; set; } = 100;

        public int CurrentStreak { get; set; } = 0;

        public int HighestStreak { get; set; } = 0;
    }
}
