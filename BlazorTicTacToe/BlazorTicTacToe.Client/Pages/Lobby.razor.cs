using BlazorTicTacToeShared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorTicTacToe.Client.Pages
{
    public partial class Lobby : ComponentBase
    {
        private HubConnection? _hubConnection;
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        private string _playerName = string.Empty;
        private string _currentRoomName = string.Empty;
        private GameRoom? _currentRoom;
        private List<GameRoom> _rooms = new();

        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder().WithUrl(NavigationManager
                .ToAbsoluteUri("/gamehub"))
                .Build();

            _hubConnection.On<List<GameRoom>>("Rooms", (roomList) =>
            {
                Console.WriteLine($"We got rooms! Count = {roomList.Count}");
            });

            await _hubConnection.StartAsync();
        }

        private async Task CreateRoom()
        {
            if (_hubConnection is null) return;

            _currentRoom = await _hubConnection.InvokeAsync<GameRoom>("CreateRoom", _currentRoomName, _playerName);
        }
    }
}