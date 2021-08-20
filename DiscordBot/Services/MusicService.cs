using Microsoft.Extensions.Logging;

namespace DiscordBot.Services
{
    /// <summary>
    /// Музыкальный сервис
    /// </summary>
    public class MusicService
    {
        private readonly ILogger<MusicService> _logger;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="logger">Логгер</param>
        public MusicService(ILogger<MusicService> logger)
        {
            _logger = logger;
        }
    }
}