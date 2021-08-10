using System;
using System.Threading.Tasks;
using DiscordBot.Di;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.ConfigureServices();
            
            var serviceProvider = services.BuildServiceProvider();

            // точка входа
            Task.Run(async () => await serviceProvider.GetRequiredService<App>().Run(args));
            
            Console.ReadKey();
        }
    }
}