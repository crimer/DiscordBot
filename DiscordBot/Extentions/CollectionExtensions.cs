using System;
using System.Collections;
using System.Collections.Generic;

namespace DiscordBot.Extentions
{
    /// <summary>
    /// Методы расширения для коллекций
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Коллекция пуста или null
        /// </summary>
        /// <param name="list">Коллекция</param>
        /// <returns>Bool</returns>
        public static bool IsNullOrEmpty(this IList list) => list == null || list.Count == 0;
        
        /// <summary>
        /// Получить рандомный элемент колекции
        /// </summary>
        /// <param name="items">Коллекция</param>
        /// <typeparam name="T">Тип коллекции</typeparam>
        /// <returns>Элемент коллекции</returns>
        public static T Random<T>(this IList<T> items)
        {
            var random = new Random();
            return items[random.Next(items.Count)];
        }
    }
}