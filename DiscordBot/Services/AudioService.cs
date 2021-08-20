using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Services
{
    public class AudioService
    {
        private readonly ILogger<AudioService> _logger;

        public AudioService(ILogger<AudioService> logger)
        {
            _logger = logger;
        }
        
        public async Task JoinVoiceChannelAsync(SocketVoiceChannel channel)
        {
            try
            {
                await channel.ConnectAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Произошла ошибка в подключении к голосовому каналу: {ex}");
            }
        }
        
        public async Task LeaveVoiceChannelAsync(SocketVoiceChannel channel)
        {
            try
            {
                await channel.DisconnectAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Произошла ошибка в отключении от голосового канала: {ex}");
            }
        }

        public async Task SendAsync(IAudioClient client, string path)
        {
            try
            {
                var ffmpeg = CreateStream(path);
                var output = ffmpeg.StandardOutput.BaseStream;
                var discord = client.CreatePCMStream(AudioApplication.Mixed, 96000);
                await output.CopyToAsync(discord);
                await discord.FlushAsync();
                
                //if (DependencyMap.Get<VoiceService>().inUse()) BROKEN??????
                //    DependencyMap.Get<VoiceService>().stopContext();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Произошла ошибка в в отправке {ex}");
            }
        }

        public async Task SendTextAsync(SocketCommandContext context, string text)
        {
            await context.Channel.SendMessageAsync(text);
        }

        public async Task SendTextWithoutContextAsync(ISocketMessageChannel channel, string text)
        {
            await channel.SendMessageAsync(text);
        }

        public async Task SendFileAsync(SocketCommandContext context, string path)
        {
            await context.Channel.SendFileAsync(path);
        }

        public async Task Stream(IAudioClient client, string url)
        {
            try
            {
                var ffmpeg = CreateYoutubeStream(url);
                var output = ffmpeg.StandardOutput.BaseStream;
                var discord = client.CreatePCMStream(AudioApplication.Mixed, 96000);
                await output.CopyToAsync(discord);
                await discord.FlushAsync();
                // if (DependencyMap.Get<VoiceService>().inUse()) BROKEN????????
                //    DependencyMap.Get<VoiceService>().stopContext();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Произошла ошибка в в отправке {ex}");
            }
        }

        public async Task StreamRadio(IAudioClient client, string url)
        {
            /*
            WebResponse res = await WebRequest.Create(@"http://uk5.internet-radio.com:8278/live").GetResponseAsync();
            Console.WriteLine(res.ContentLength);
            Stream web = res.GetResponseStream();
            var ffmpeg = CreateRadioStream();
            var input = ffmpeg.StandardInput.BaseStream;
            var output = ffmpeg.StandardOutput.BaseStream;
            var discord = client.CreatePCMStream(AudioApplication.Mixed, 1920);
            web.CopyTo(input);
            await output.CopyToAsync(discord);
            await discord.FlushAsync();
            if (DependencyMap.Get<VoiceService>().inUse())
                DependencyMap.Get<VoiceService>().stopContext(); */
        }

        private Process CreateStream(string path)
        {
            var ffmpeg = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            return Process.Start(ffmpeg);
        }

        private Process CreateYoutubeStream(string url)
        {
            var ffmpeg = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $@"/C C:\youtube-dl.exe --no-check-certificate -f bestaudio -o - {url} | ffmpeg -i pipe:0 -f s16le -ar 48000 -ac 2 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            return Process.Start(ffmpeg);
        }
    }
}