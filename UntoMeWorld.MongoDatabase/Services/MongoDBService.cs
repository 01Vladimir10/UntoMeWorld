using MongoDB.Driver;

namespace UntoMeWorld.MongoDatabase.Services
{
    public class MongoDbService
    {
        private MongoClient _client;
        private IMongoDatabase _database;

        public MongoDbService(string connectionString)
        {
            InitializeDatabase(connectionString);
        }
        public MongoDbService(string username, string password, string server, string database = "default")
        {
            InitializeDatabase($"mongodb+srv://{username}:{password}@{server}/{database}?retryWrites=true&w=majority", database);
        }
        private void InitializeDatabase(string connection, string database = "default")
        {
            _client = new MongoClient(connection);
            _database = _client.GetDatabase(database);
        }
        public IMongoCollection<T> GetCollection<T>(string collection)
            => _database.GetCollection<T>(collection);
    }
}