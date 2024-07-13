namespace SmartFeedback.Scripts;

public static class Logger
{
    private static readonly string LogFilePath;

    static Logger()
    {
        var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

#if DEBUG
        LogFilePath = Path.Combine(logDirectory, $"log_debug.txt");
#else
        LogFilePath = Path.Combine(logDirectory, $"log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
#endif

        using (var stream = File.Create(LogFilePath))
        {
        }
    }

    public static void Log(string fileName, string message)
    {
        var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {fileName} | {message}";
        try
        {
            using var writer = new StreamWriter(LogFilePath, true);
            writer.WriteLine(logMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка записи лога: {ex.Message}");
        }
    }
}