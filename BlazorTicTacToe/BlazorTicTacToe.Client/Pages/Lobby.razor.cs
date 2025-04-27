using BlazorTicTacToeShared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorTicTacToe.Client.Pages
{
    public partial class Lobby : ComponentBase
    {
        private HubConnection? _hubConnection;
        [Inject]
        public required NavigationManager NavigationManager { get; init; }
        [Inject]
        public required IGameRoomManager GameRoomManager { get; init; }
        private string _playerName = string.Empty;
        private string _currentRoomName = string.Empty;
        private GameRoom? _currentRoom;
        private List<GameRoom> _rooms = [];

        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder().WithUrl(NavigationManager
                .ToAbsoluteUri("/gamehub"))
                .Build();

            _hubConnection.On<List<GameRoom>>("Rooms", (roomList) =>
            {
                Console.WriteLine($"We got rooms! Count = {roomList.Count}");
                _rooms = roomList;
                StateHasChanged();
            });

            await _hubConnection.StartAsync();
        }

        private async Task CreateRoom()
        {
            if (_hubConnection is null) return;

            _currentRoom = await _hubConnection.InvokeAsync<GameRoom>("CreateRoom", _currentRoomName, _playerName);
            _rooms.Add( _currentRoom );
            await JoinRoom(_currentRoom.RoomId);
        }

        private void CreateAIRoom()
        {
            _currentRoom = GameRoomManager.CreateAIRoom(_playerName);
            _rooms.Add(_currentRoom);
            NavigationManager.NavigateTo($"/game/{_currentRoom.RoomId}");
        }

        private async Task JoinRoom(string roomId)
        {
            if(_hubConnection is null) return;

            var joinedRoom = await _hubConnection.InvokeAsync<GameRoom>("JoinRoom", roomId, _playerName);

            if(joinedRoom is not null)
            {
                _currentRoom = joinedRoom;
            } 
            else
            {
                Console.WriteLine("Room is full or does not exists");
            }
        }
    }
}