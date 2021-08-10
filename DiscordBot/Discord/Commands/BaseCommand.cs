using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Discord.Commands
{
    public class BaseCommand : ModuleBase<SocketCommandContext>
    {
        protected async Task ReplyToUserMessage(string message, ulong messageId)
        {
            await this.ReplyAsync(message, messageReference: new MessageReference(messageId));
        }
    }
}