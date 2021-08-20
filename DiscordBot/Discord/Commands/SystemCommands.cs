using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Discord.Commands
{
    /// <summary>
    /// Системные команды бота
    /// </summary>
    [Name("Системные команды")]
    public class SystemCommands : BaseCommand
    {
        private readonly CommandService _commandService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="commandService">Серсив команд бота</param>
        public SystemCommands(CommandService commandService)
        {
            _commandService = commandService;
        }

        [Command("help")]
        [Remarks("Список доступных команд")]
        [Summary("Помощь")]
        public async Task HelpAsync()
        {
            var embedBuilder = new EmbedBuilder
            {
                Color = new Color(114, 137, 218),
                Description = "Команды которые вы можете использовать"
            };

            foreach (var module in _commandService.Modules)
            {
                var builder = new StringBuilder();
                foreach (var commandInfo in module.Commands)
                {
                    var result = await commandInfo.CheckPreconditionsAsync(Context).ConfigureAwait(false);
                    if (result.IsSuccess)
                    {
                        var desc = $"({commandInfo.Summary}) {commandInfo.Name}";
                        var parameters = string.Empty;

                        if (commandInfo.Parameters != null && commandInfo.Parameters.Count > 0)
                        {
                            var commandParams = string.Join(" ", commandInfo.Parameters.Select(x => x.Name)); 
                            parameters = $"*{commandParams}*";
                        }
                        
                        builder.AppendLine($"| {desc} {parameters}");
                    }
                }
                var description = builder.ToString();

                if (!string.IsNullOrWhiteSpace(description))
                {
                    embedBuilder.AddField(x =>
                    {
                        x.Name = module.Name;
                        x.Value = description;
                        x.IsInline = false;
                    });
                }
            }

            await this.ReplyToUserMessageAsync(embedBuilder.Build());
        }
    }
}