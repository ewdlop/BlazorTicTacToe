using System.Collections.Concurrent;

namespace BlazorTicTacToeShared
{
    public class GameRoomManager : IGameRoomManager
    {
        protected readonly ConcurrentBag<GameRoom> _rooms = [];

        public IEnumerable<GameRoom> Rooms => _rooms.OrderBy(r => r.RoomName);

        public GameRoom CreateRoom(string roomName, string playerName, string connecitonId)
        {
            string roomId = Guid.NewGuid().ToString();
            GameRoom room = new GameRoom(roomId, roomName);


            var newPlayer = new Player(connecitonId, playerName);
            room.TryAddPlayer(newPlayer);

            _rooms.Add(room);

            return room;
        }

        public GameRoom CreateAIRoom(string playerName)
        {
            GameRoom _currentRoom = new GameRoom(Guid.NewGuid().ToString(), "AI Room");
            _currentRoom.Game.StartGame();
            _currentRoom.Game.PlayerXId = "AI";
            _currentRoom.Game.PlayerOId = playerName;
            _currentRoom.Game.CurrentPlayerId = "AI";
            return _currentRoom;
        }

        public bool TryAddNewPlayerToRoom(string roomId, string connectionId, string playerName, out GameRoom? room, out Player? newPlayer)
        {
            room = _rooms.FirstOrDefault(r => r.RoomId == roomId);
            if (room is not null)
            {
                newPlayer = new Player(connectionId, playerName);
                return room.TryAddPlayer(newPlayer);
            }
            newPlayer = null;
            return false;
        }

        public bool TryStartGame(string roomId, out GameRoom? room)
        {
            room = _rooms.FirstOrDefault(r => r.RoomId == roomId);

            if (room is not null)
            {
                room.Game.StartGame();
                return true;
            }
            return false;
        }

        private static bool IsPlayerTurn(GameRoom room, string playerId)
        {
            if(room is not null)
            {
                return room.Game.CurrentPlayerId == playerId;
            }
            return false;
        }

        public bool TryMakeMove(string roomId, int row, int col, string playerId, out GameRoom? room)
        {
            room = _rooms.FirstOrDefault(r => r.RoomId == roomId);
            if (room is not null && IsPlayerTurn(room, playerId) && room.Game.MakeMove(row, col, playerId))
            {
                room.Game.Winner = room.Game.CheckWinner();
                room.Game.IsDraw = room.Game.CheckDraw() && string.IsNullOrEmpty(room.Game.Winner);
                if (!string.IsNullOrEmpty(room.Game.Winner) || room.Game.IsDraw)
                {
                    room.Game.GameOver = true;
                }

                return true;
            }
            return false;
        }
    }
}
