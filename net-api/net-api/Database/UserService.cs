using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using net_api.Data;

namespace net_api.Database
{
    public class UserService
    {

        private readonly IMongoCollection<User> _userCollection;

        public UserService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _userCollection = mongoDatabase.GetCollection<User>(databaseSettings.Value.UserCollectionName);
        }

        public async Task<User?> FindAsync(ObjectId id) => await _userCollection.Find(user => user.Id.Equals(id)).FirstOrDefaultAsync();

        public async Task InsertAsync(User user) => await _userCollection.InsertOneAsync(user);

        public async Task<UpdateResult> PushHabitAsync(ObjectId id, Habit habit) =>
            await _userCollection.UpdateOneAsync(user => user.Id.Equals(id),
                Builders<User>.Update.Push(user => user.Habits, habit));
    }
}
