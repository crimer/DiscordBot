using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Discord.Commands
{
    /// <summary>
    /// Базовая команда для бота
    /// </summary>
    public class BaseCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Ответить на сообщение пользователя
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        /// <param name="messageId">Идентификатор сообщения пользователя</param>
        protected async Task ReplyToUserMessage(string message, ulong messageId)
        {
            await this.ReplyAsync(message, messageReference: new MessageReference(messageId));
        }
    }
}