using System.IO;
using DiscordBot.Configuration;
using DiscordBot.Discord;
using DiscordBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Di
{
    /// <summary>
    /// Регистрация зависимостей приложения
    /// </summary>
    public static class RegisterDependencies
    {
        private const string AppSettingsFileName = "appsettings.json";
        
        /// <summary>
        /// Конфигурация зависимостей приложения
        /// </summary>
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppSettingsFileName, optional: false)
                .Build();

            services.Configure<AppConfig>(configuration.GetSection("Config"));

            services.AddSingleton<App>();

            // Сервисы
            services.AddSingleton<DiscordClient>()
                .AddSingleton<MusicService>()
                .AddSingleton<AudioService>();
        }
    }
}