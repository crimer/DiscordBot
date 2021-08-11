using System.IO;
using DiscordBot.Configuration;
using DiscordBot.Discord;
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

            // Сервисы
            services.AddSingleton<DiscordClient>();

            // Точка входа
            services.AddSingleton<App>();
        }
    }
}