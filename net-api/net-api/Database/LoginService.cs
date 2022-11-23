using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using net_api.Data;

namespace net_api.Database
{
    public class LoginService
    {

        private readonly IMongoCollection<UserLogin> _loginCollection;

        public LoginService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _loginCollection = mongoDatabase.GetCollection<UserLogin>(databaseSettings.Value.LoginCollectionName);
        }

        public async Task<UserLogin?> FindAsync(ObjectId id) => await _loginCollection.Find(login => login.Id.Equals(id)).FirstOrDefaultAsync();

        public async Task<UserLogin?> FindByUsernameAsync(string username) => await _loginCollection.Find(login => username == login.Username).FirstOrDefaultAsync();

        public async Task<UserLogin?> FindByRefreshTokenAsync(string token) => await _loginCollection.Find(login => login.RefreshToken.Token == token).FirstOrDefaultAsync();

        public async Task InsertAsync(UserLogin user) => await _loginCollection.InsertOneAsync(user);

        public async Task<UpdateResult> UpdateRefreshTokenAsync(ObjectId id, RefreshToken token) =>
            await _loginCollection.UpdateOneAsync(login => login.Id.Equals(id),
                Builders<UserLogin>.Update.Set(login => login.RefreshToken, token));
    }
}
