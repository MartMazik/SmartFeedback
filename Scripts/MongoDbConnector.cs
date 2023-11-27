using MongoDB.Driver;

namespace SmartFeedback.Scripts;

public class MongoDbConnector
{
    private readonly IMongoDatabase _database;

    public MongoDbConnector(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}