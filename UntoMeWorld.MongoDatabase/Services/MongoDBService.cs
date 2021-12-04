using MongoDB.Driver;

namespace UntoMeWorld.MongoDatabase.Services
{
    public class MongoDbService
    {
        private MongoClient _client;

        public MongoDbService(string connectionString)
        {
            InitializeDatabase(connectionString);
        }

        public MongoDbService(string server, string username, string password)
        {
            
        }

        private void InitializeDatabase(string connection)
        {
            _client = new MongoClient(connection);
        }
    }
}