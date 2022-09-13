using MongoDB.Driver;

namespace UntoMeWorld.Infrastructure.Services
{
    public class MongoDbService
    {
        private MongoClient _client = null!;
        private IMongoDatabase _database = null!;

        public MongoDbService(string connectionString, string? database = default)
        {
            InitializeDatabase(connectionString, database ?? "default");
        }
        private void InitializeDatabase(string connection, string database)
        {
            try
            {
                _client = new MongoClient(connection);
                _database = _client.GetDatabase(database);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public IMongoCollection<T> GetCollection<T>(string collection)
            => _database.GetCollection<T>(collection);
    }
}