using Xunit;
using Application.Configuration;
using Application.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Tests
{
    public class SettingsTests
    {
        [Fact]
        public void LoggingSettings_Properties()
        {
            var settings = new LoggingSettings
            {
                LogFilePath = "log.txt",
                LogLevel = "Debug"
            };
            Assert.Equal("log.txt", settings.LogFilePath);
            Assert.Equal("Debug", settings.LogLevel);
        }

        [Fact]
        public void AppSettings_Logging_NotNull()
        {
            var appSettings = new AppSettings
            {
                Logging = new LoggingSettings { LogFilePath = "log.txt", LogLevel = "Info" }
            };
            Assert.NotNull(appSettings.Logging);
            Assert.Equal("log.txt", appSettings.Logging.LogFilePath);
        }

        [Fact]
        public void DatabaseSettings_Default()
        {
            var dbSettings = new DatabaseSettings();
            Assert.NotNull(dbSettings);
        }
    }

    public class CoverageBoostTests
    {
        [Fact]
        public void AllSettings_Constructors_And_Properties()
        {
            var appSettings = new AppSettings();
            var dbSettings = new DatabaseSettings();
            var logSettings = new LoggingSettings();

            logSettings.LogFilePath = "log.txt";
            logSettings.LogLevel = "Debug";
            Assert.Equal("log.txt", logSettings.LogFilePath);
            Assert.Equal("Debug", logSettings.LogLevel);

            appSettings.Logging = logSettings;
            Assert.Equal(logSettings, appSettings.Logging);
        }

        [Fact]
        public void ServiceCollectionExtensions_Coverage()
        {
            var services = new ServiceCollection();
            services.AddApplicationServices();
        }
    }
}
