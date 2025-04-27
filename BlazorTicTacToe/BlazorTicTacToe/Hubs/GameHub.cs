using BlazorTicTacToeShared;
using Microsoft.AspNetCore.SignalR;

namespace BlazorTicTacToe.Hubs
{
    public class GameHub : Hub
    {
        private IGameRoomManager _gameRoomManager;

        public GameHub(IGameRoomManager gameRoomManager)
        {
            _gameRoomManager = gameRoomManager;
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Player with Id '{Context.ConnectionId}' connected.");

            return Clients.Caller.SendAsync("Rooms", _gameRoomManager.Rooms);

        }

        public async Task<GameRoom> CreateRoom(string roomName, string playerName)
        {
            GameRoom room = _gameRoomManager.CreateRoom(roomName, playerName, Context.ConnectionId);

            await Groups.AddToGroupAsync(Context.ConnectionId, room.RoomId);

            await Clients.All.SendAsync("Rooms", _gameRoomManager.Rooms);

            return room;
        }

        public async Task<GameRoom?> JoinRoom(string roomId, string playerName)
        {
            if (_gameRoomManager.TryAddNewPlayerToRoom(roomId, Context.ConnectionId, playerName, out GameRoom? room, out Player? newPlayer))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
                await Clients.Group(roomId).SendAsync("PlayerJoined", newPlayer);

                return await Task.FromResult(room);
            }

            return null;
        }

        public async Task StartGame(string roomId)
        {
            if (_gameRoomManager.TryStartGame(roomId, out GameRoom? room))
            {
                await Clients.Group(roomId).SendAsync("UpdateGame", room);
            }
        }

        public async Task MakeMove(string roomId, int row, int col, string playerId)
        {
            if(_gameRoomManager.TryMakeMove(roomId, row, col, playerId, out GameRoom? room))
            {
                await Clients.Group(roomId).SendAsync("UpdateGame", room);
            }
        }
    }
}
