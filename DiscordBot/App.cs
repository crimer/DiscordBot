using System;
using System.Threading.Tasks;
using DiscordBot.Discord;
using Microsoft.Extensions.Logging;

namespace DiscordBot
{
    /// <summary>
    /// Точка входа
    /// </summary>
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly DiscordClient _discordClient;
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="logger">Логгер</param>
        /// <param name="discordClient">Клиент Дискорда Бота</param>
        public App(ILogger<App> logger, DiscordClient discordClient)
        {
            _logger = logger;
            _discordClient = discordClient;
        }
        
        /// <summary>
        /// Запуск
        /// </summary>
        /// <param name="args"></param>
        public async Task Run(string[] args)
        {
            try
            {
                _logger.LogInformation("Стартуем...");
                await _discordClient.StartAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            
            await Task.CompletedTask;
        }
    }
}