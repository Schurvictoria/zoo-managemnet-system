namespace Application.Configuration
{
    public class AppSettings
    {
        public DatabaseSettings Database { get; set; }
        public LoggingSettings Logging { get; set; }
    }

    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
    }

    public class LoggingSettings
    {
        public string LogLevel { get; set; }
        public string LogFilePath { get; set; }
    }
} 