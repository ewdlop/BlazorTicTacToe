namespace BlazorTicTacToeShared
{
    public interface IGameRoomManager
    {
        IEnumerable<GameRoom> Rooms { get; }
        void AddRooms(IEnumerable<GameRoom> rooms);
        GameRoom CreateRoom(string roomName, string playerName, string connecitonId);
        GameRoom CreateAIRoom(string connecitonId, string playerName);
        bool TryAddNewPlayerToRoom(string roomId, string connectionId, string playerName, out GameRoom? room, out Player? newPlayer);
        bool TryStartGame(string roomId, out GameRoom? room);
        bool TryMakeMove(string roomId, int row, int col, string playerId, out GameRoom? room);
    }
}
