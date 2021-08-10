using System;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Discord.Commands
{
    public class FunCommands : BaseCommand
    {
        private readonly ILogger<FunCommands> _logger;

        public FunCommands(ILogger<FunCommands> logger)
        {
            _logger = logger;
        }
        
        [Command("roll")]
        [Alias("dice")]
        [Summary("Кинуть кубик")]
        public async Task Roll([Remainder] string text = "")
        {
            try
            {
                var maxRandomValue = 20;
                await ReplyToUserMessage($"Шанс {new Random().Next(0, maxRandomValue)} из {maxRandomValue}", Context.Message.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Команда 'roll' ошибка: {ex}");
            }
        }
        
        [Command("decide")]
        [Alias("choose")]
        [Summary("Выбрать из нескольких")]
        public async Task Decide([Remainder] string message = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    await ReplyToUserMessage($"Пустой текст. Напиши что-то типа: **hubot decide тут или там**", Context.Message.Id);
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
                    await ReplyToUserMessage($"Не найдены ни один из разделителей: **или** **,** ", Context.Message.Id);
                    return;
                }
            
                var options = message.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                var selection = options[new Random().Next(options.Length)];
                await ReplyToUserMessage($"Определенно **{selection}**", Context.Message.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Команда 'decide' ошибка: {ex}");
            }
        }
    }
}