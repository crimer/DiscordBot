using System;
using Discord;

namespace DiscordBot.Utils
{
    /// <summary>
    /// Класс работы с Embed
    /// </summary>
    public static class EmbedHelper
    {
        /// <summary>
        /// Создать базовый Embed
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="description">Описание</param>
        /// <param name="color">Цвет</param>
        /// <returns>Embed</returns>
        public static Embed CreateBasicEmbed(string title, string description, Color color)
        {
            return new EmbedBuilder()
                .WithTitle(title)
                .WithDescription(description)
                .WithColor(color)
                .WithCurrentTimestamp().Build();
        }

        /// <summary>
        /// Создать Embed ошибки
        /// </summary>
        /// <param name="ex">Ошибка</param>
        /// <returns>Embed</returns>
        public static Embed CreateErrorEmbed(Exception ex)
        {
            return new EmbedBuilder()
                .WithTitle($"Ошибка в - {ex.Source}")
                .WithDescription($"**Описание**: \n{ex.Message}")
                .WithColor(Color.DarkRed)
                .WithCurrentTimestamp().Build();
        }
    }
}