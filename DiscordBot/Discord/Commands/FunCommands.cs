using System;
using System.Threading.Tasks;
using Discord.Commands;
using DiscordBot.Extentions;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Discord.Commands
{
    /// <summary>
    /// Команды для развлечения
    /// </summary>
    [Name("Команды для фана")]
    public class FunCommands : BaseCommand
    {
        private readonly ILogger<FunCommands> _logger;
        private readonly DiscordClient _discordClient;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="logger">Логгер</param>
        /// <param name="discordClient">Клиент бота</param>
        public FunCommands(ILogger<FunCommands> logger, DiscordClient discordClient)
        {
            _logger = logger;
            _discordClient = discordClient;
        }
        
        [Command("roll", RunMode = RunMode.Async)]
        [Summary("Кинуть кубик")]
        public async Task Roll([Name("Текст на предсказание")] string? text = "")
        {
            try
            {
                var maxRandomValue = 20;
                await ReplyToUserMessageAsync($"Шанс {new Random().Next(0, maxRandomValue)} из {maxRandomValue}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Команда 'roll' ошибка: {ex}");
            }
        }
        
        [Command("decide", RunMode = RunMode.Async)]
        [Summary("Выбрать из нескольких")]
        public async Task Decide([Name("Несколько предложений на выбор")] string message = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    await ReplyToUserMessageAsync($"Пустой текст. Напиши что-то типа: **hubot decide тут или там**");
                    return;
                }
            
                var separators = new[] {"или", ","};
                var counter = 0;
                foreach (var separator in separators)
                {
                    if (!message.ToLower().Contains(separator.ToLower(), StringComparison.OrdinalIgnoreCase))
                        counter++;
                }

                if (counter == separators.Length)
                {
                    await ReplyToUserMessageAsync($"Не найдены ни один из разделителей: **или** **,** ");
                    return;
                }
            
                var options = message.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                var selection = options[new Random().Next(options.Length)];
                await ReplyToUserMessageAsync($"Определенно **{selection}**");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Команда 'decide' ошибка: {ex}");
            }
        }
        
        [Command("8ball", RunMode = RunMode.Async)]
        [Alias("8ball", "ask")]
        [Summary("Спросить у \"Шара судьбы\"")]
        public async Task AskQuestion([Name("Ваш вопрос")] string args)
        {
            var answers = new []
            {
                "Вперед!",
                "Не сейчас",
                "Не делай этого",
                "Ты шутишь?",
                "Да, но позднее",
                "Думаю, не стоит",
                "Не надейся на это",
                "Ни в коем случае",
                "Это неплохо",
                "Кто знает?",
                "Туманный ответ, попробуй еще",
                "Я не уверен",
                "Я думаю, хорошо",
                "Забудь об этом",
                "Это возможно",
                "Определенно - да",
                "Быть может",
                "Слишком рано",
                "Да",
                "Конечно, да",
                "Даже не думай",
                "Лучше Вам пока этого не знать",
                "Без понятия",
                "ХЗ"
            };
            
            var answer = answers.Random();
            await this.ReplyToUserMessageAsync($":crystal_ball: **{answer}** :crystal_ball:");
        }

        [Command("ping", RunMode = RunMode.Async)]
        [Summary("Пинг")]
        private async Task Ping()
        {
            var pingTime = $"👋 pong **{_discordClient.GetClient().Latency}**ms";
            await ReplyToUserMessageAsync(pingTime);
        }
    }
}