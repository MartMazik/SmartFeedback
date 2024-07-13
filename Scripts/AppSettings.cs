using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SmartFeedback.Scripts;

public static class AppSettings
{
    public static AuthorizationSettings? Authorization { get; private set; }
    public static PythonModuleSettings? PythonModule { get; private set; }
    public static MongoDbSettings? MongoDb { get; private set; }

    public static void Initialize(IConfiguration configuration)
    {
        Authorization = configuration.GetSection("AuthorizationSettings").Get<AuthorizationSettings>();
        PythonModule = configuration.GetSection("PythonModuleSettings").Get<PythonModuleSettings>();
        MongoDb = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
    }
}

public class AuthorizationSettings(string issuer, string audience, string key)
{
    public string Issuer { get; init; } = issuer;
    public string Audience { get; init; } = audience;
    public string Key { get; init; } = key;

    public SymmetricSecurityKey GetKey => new(Encoding.UTF8.GetBytes(Key));
}

public class PythonModuleSettings(string baseUrl)
{
    public string BaseUrl { get; init; } = baseUrl;
}

public class MongoDbSettings(string connectionString, string databaseName)
{
    public string ConnectionString { get; init; } = connectionString;
    public string DatabaseName { get; init; } = databaseName;
}