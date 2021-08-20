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
        protected Task ReplyToUserMessageAsync(string message)
        {
            return this.ReplyAsync(message, messageReference: new MessageReference((Context.Message.Id)));
        }
        
        /// <summary>
        /// Ответить на сообщение пользователя
        /// </summary>
        /// <param name="embed">Embed</param>
        protected Task ReplyToUserMessageAsync(Embed embed)
        {
            return this.ReplyAsync(embed: embed, messageReference: new MessageReference(Context.Message.Id));
        }
        
        /// <summary>
        /// Написать сообщение в лучку пользователю
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="embed">Embed</param>
        /// <returns></returns>
        protected Task DirectMessageToUserAsync(string message, Embed embed)
        {
            return Context.User.SendMessageAsync(message, false, embed);
        }
    }
}