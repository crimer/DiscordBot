using System;
using System.Threading.Tasks;
using DiscordBot.Di;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot
{
    /// <summary>
    /// Главный файл
    /// </summary>
    class Program
    {
        /// <summary>
        /// Главный метод
        /// </summary>
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.ConfigureServices();
            
            var serviceProvider = services.BuildServiceProvider();

            // Точка входа
            Task.Run(async () => await serviceProvider.GetRequiredService<App>().Run(args));
            
            Console.ReadKey();
        }
    }
}