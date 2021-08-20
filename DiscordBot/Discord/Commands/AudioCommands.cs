using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Services;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Discord.Commands
{
    /// <summary>
    /// Аудио команды
    /// </summary>
    [Name("Аудио команды")]
    public class AudioCommands : BaseCommand
    {
        private readonly AudioService _audioService;
        private readonly ILogger<AudioCommands> _logger;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="audioService">Аудио сервис</param>
        /// <param name="logger">Логгер</param>
        public AudioCommands(AudioService audioService, ILogger<AudioCommands> logger)
        {
            _audioService = audioService;
            _logger = logger;
        }
        
        [Command("join", RunMode = RunMode.Async)]
        [Summary("Вступить в голосовой канал")]
        public async Task JoinVoiceChannelAsync([Name("Точное название канала")] string channelName)
        {
            try
            {
                var targetChannel = GetVoiceChannelByName(channelName);
                if (targetChannel == null)
                {
                    await ReplyToUserMessageAsync($"Не найден канал: \"{channelName}\"");
                    return;
                }

                await _audioService.JoinVoiceChannelAsync(targetChannel as SocketVoiceChannel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Произошла ошибка в подключении к голосовому каналу: {ex}");
            }
        }
        
        [Command("leave", RunMode = RunMode.Async)]
        [Summary("Покинуть голосовой канал")]
        public async Task LeaveVoiceChannelAsync([Name("Точное название канала")] string channelName)
        {
            try
            {
                var targetChannel = GetVoiceChannelByName(channelName);
                if (targetChannel == null)
                {
                    await ReplyToUserMessageAsync($"Не найден канал: \"{channelName}\"");
                    return;
                }

                await _audioService.LeaveVoiceChannelAsync(targetChannel as SocketVoiceChannel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Произошла ошибка при отключении от голосового канала: {ex}");
            }
        }

        /// <summary>
        /// Получить канал сервера по имени
        /// </summary>
        /// <param name="channelName">Имя канала</param>
        /// <returns>Канал</returns>
        private IChannel GetVoiceChannelByName(string channelName)
        {
            var guildChannels = Context.Guild.Channels.Where(c => 
                c.GetType() == typeof(SocketVoiceChannel));

            return guildChannels.FirstOrDefault(c => c.Name == channelName);
        }
        
        [Command("youtube", RunMode = RunMode.Async)]
        [Summary("Запустить видео с Youtube")]
        [RequireContext(ContextType.Guild)]
        public async Task PlayYouTubeVideoAsync(string url)
        {
            // await _audioService.Stream(audioClient, url.Split('&')[0]);
        }
    }
}