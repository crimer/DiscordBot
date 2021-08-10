using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscordBot.Discord
{
    public class DiscordClient
    {
        private readonly DiscordSocketClient _client;
        private readonly string _botToken;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly ILogger<DiscordClient> _logger;
        private readonly string _botName;

        public DiscordClient(IServiceProvider services, 
            ILogger<DiscordClient> logger, IOptions<AppConfig> options)
        {
            _botName = options.Value.BotName;
            _botToken = options.Value.BotToken;
            _services = services;
            _logger = logger;
            
            _commands = GetCommandServiceConfig();
            _client = GetBotConfig();
            
            _client.Ready += OnReady;
            _client.MessageReceived += OnHandleCommandAsync;
            _client.Log += OnLog;
        }

        public async Task StartAsync()
        {
            try
            {
                await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
                await _client.LoginAsync(TokenType.Bot, _botToken);
                await _client.StartAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Во время старта бота произошла ошибка: {ex}");
            }
        }
        
        public Task StopAsync() => _client.LogoutAsync();
        
        private Task OnLog(LogMessage arg)
        {
            _logger.LogInformation($"{arg.Message} - {arg.Exception}");
            return Task.CompletedTask;
        }

        private Task OnReady()
        {
            _logger.LogInformation("Бот готов ^_^");
            return Task.CompletedTask;
        }
        
        private async Task OnHandleCommandAsync(SocketMessage rawMessage)
        {
            try
            {
                if (rawMessage.Author.IsBot || !(rawMessage is SocketUserMessage message) || message.Channel is IDMChannel)
                    return;

                var context = new SocketCommandContext(_client, message);

                if(string.IsNullOrWhiteSpace(rawMessage.Content))
                    return;
                
                if(rawMessage.Content.TrimStart().StartsWith(_botName, StringComparison.Ordinal))
                {
                    var argPos = _botName.Length + 1;
                    var result = await _commands.ExecuteAsync(context, argPos, _services);

                    if (!result.IsSuccess && result.Error.HasValue)
                    {
                        if (result.ErrorReason == "Unknown command.")
                        {
                            await context.Channel.SendMessageAsync($":x: Не знаю такую команду", messageReference: new MessageReference(context.Message.Id));
                            _logger.LogError($"Не знаю такую команду: {rawMessage.Content}");
                            return;
                        }
                            
                        await context.Channel.SendMessageAsync($":x: {result.ErrorReason}");
                        _logger.LogError($"Не удалось выполнить команду: {result.ErrorReason}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Во время обработки сообщения произошла ошибка: {ex}");
            }
        }
        
        private CommandService GetCommandServiceConfig()
            => new (new CommandServiceConfig()
            {
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Error
            });

        private DiscordSocketClient GetBotConfig()
            => new (new DiscordSocketConfig()
            {
                MessageCacheSize = 500,
            });
    }
}