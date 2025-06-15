using Xunit;
using Application.Services;
using Application.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Tests
{
    public class LoggingServiceTests : IDisposable
    {
        private readonly string _tempLogFile;
        private readonly LoggingService _service;
        private readonly AppSettings _settings;

        public LoggingServiceTests()
        {
            _tempLogFile = Path.GetTempFileName();
            _settings = new AppSettings
            {
                Logging = new LoggingSettings { LogFilePath = _tempLogFile }
            };
            _service = new LoggingService(_settings);
        }

        [Fact]
        public async Task LogInfoAsync_WritesInfoLog()
        {
            await _service.LogInfoAsync("Test info");
            var log = await File.ReadAllTextAsync(_tempLogFile);
            Assert.Contains("INFO", log);
            Assert.Contains("Test info", log);
        }

        [Fact]
        public async Task LogErrorAsync_WritesErrorLog()
        {
            await _service.LogErrorAsync("Test error");
            var log = await File.ReadAllTextAsync(_tempLogFile);
            Assert.Contains("ERROR", log);
            Assert.Contains("Test error", log);
        }

        [Fact]
        public async Task LogErrorAsync_WithException_WritesException()
        {
            await _service.LogErrorAsync("Test error", new Exception("fail"));
            var log = await File.ReadAllTextAsync(_tempLogFile);
            Assert.Contains("ERROR", log);
            Assert.Contains("fail", log);
        }

        public void Dispose()
        {
            if (File.Exists(_tempLogFile))
                File.Delete(_tempLogFile);
        }
    }
} 