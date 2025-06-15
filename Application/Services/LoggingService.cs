using System;
using System.IO;
using System.Threading.Tasks;
using Application.Configuration;
using Application.Interfaces;

namespace Application.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly LoggingSettings _settings;
        private readonly string _logFilePath;

        public LoggingService(AppSettings settings)
        {
            _settings = settings.Logging;
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _settings.LogFilePath);
        }

        public async Task LogInfoAsync(string message)
        {
            await LogAsync("INFO", message);
        }

        public async Task LogErrorAsync(string message, Exception ex = null)
        {
            var errorMessage = ex != null 
                ? $"{message}. Exception: {ex.Message}" 
                : message;
            await LogAsync("ERROR", errorMessage);
        }

        private async Task LogAsync(string level, string message)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
            await File.AppendAllTextAsync(_logFilePath, logEntry + Environment.NewLine);
        }
    }
} 