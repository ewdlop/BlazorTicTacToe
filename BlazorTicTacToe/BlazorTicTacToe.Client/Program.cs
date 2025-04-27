using BlazorTicTacToeShared;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorTicTacToe.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddSingleton<IGameRoomManager, GameRoomManager>();

            await builder.Build().RunAsync();
        }
    }
}
