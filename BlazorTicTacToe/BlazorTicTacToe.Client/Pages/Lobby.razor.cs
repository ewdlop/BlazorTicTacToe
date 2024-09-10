using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorTicTacToe.Client.Pages
{
    public partial class Lobby : ComponentBase
    {
        private HubConnection? _hubConnection;
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder().WithUrl(NavigationManager
                .ToAbsoluteUri("/gamehub"))
                .Build();

            await _hubConnection.StartAsync();
        }
    }
}