﻿namespace BlazorTicTacToeShared
{
    public class GameRoom(string roomId, string roomName)
    {
        public string RoomId { get; set; } = roomId;
        public string RoomName { get; set; } = roomName;
        public bool IsAIRoom { get; set; } = false;

        public List<Player> Players { get; set; } = [];
        public Game Game { get; set; } = new();

        public bool TryAddPlayer(Player newPlayer)
        {
            if(Players.Count < 2 && !Players.Any(p => p.ConnectionId == newPlayer.ConnectionId))
            {
                Players.Add(newPlayer);

                if(Players.Count == 1)
                {
                    Game.PlayerXId = newPlayer.ConnectionId;
                } else
                {
                    Game.PlayerOId = newPlayer.ConnectionId;
                }
                return true;
            }

            return false;
        }
    }
}
