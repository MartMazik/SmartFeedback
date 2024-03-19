namespace SmartFeedback.Scripts.ExternalApi;

public class ApiClient
{
    private static HttpClient? _client;

    private const string BaseUrl = "http://127.0.0.1:8000/";

    public static HttpClient? GetClient()
    {
        if (_client != null) return _client;
        _client = new HttpClient();
        _client.BaseAddress = new Uri(BaseUrl);
        return _client;
    }
}
