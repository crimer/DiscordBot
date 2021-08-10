using System;
using System.Threading.Tasks;
using DiscordBot.Discord;
using Microsoft.Extensions.Logging;

namespace DiscordBot
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly DiscordClient _discordClient;
        
        public App(ILogger<App> logger, DiscordClient discordClient)
        {
            _logger = logger;
            _discordClient = discordClient;
        }
        
        public async Task Run(string[] args)
        {
            try
            {
                _logger.LogInformation("Starting...");
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