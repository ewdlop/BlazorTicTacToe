using BlazorTicTacToeShared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorTicTacToe.Client.Components
{
    public partial class Room : ComponentBase
    {
        private string? myPlayerId;

        [Parameter]
        public GameRoom? CurrentRoom { get; set; }
        [CascadingParameter]
        public HubConnection? HubConnection { get; set; }

        protected override Task OnInitializedAsync()
        {
            if (CurrentRoom is null || HubConnection is null || HubConnection.ConnectionId is null) return Task.CompletedTask;

            myPlayerId = HubConnection.ConnectionId; 
            HubConnection.On<Player>("PlayerJoined", player =>
            {
                CurrentRoom.Players.Add(player);
                StateHasChanged();
            });

            HubConnection.On<GameRoom>("UpdateGame", serverRoom =>
            {
                CurrentRoom = serverRoom;
                StateHasChanged();
            });

            return Task.CompletedTask;
        }

        private Task StartGame()
        {
            if (HubConnection is null || CurrentRoom is null) return Task.CompletedTask;

            return HubConnection.InvokeAsync("StartGame", CurrentRoom.RoomId);
        }

        private async Task MakeMove(int row, int col)
        {

            if(IsMyTurn() 
                && CurrentRoom is not null 
                && CurrentRoom.Game.GameStarted
                && !CurrentRoom.Game.GameOver
                && HubConnection is not null)
            {
                await HubConnection.InvokeAsync("MakeMove", CurrentRoom.RoomId, row, col, myPlayerId);

            }

        }

        private bool IsMyTurn()
        {
            if(CurrentRoom is not null)
            {
                return myPlayerId == CurrentRoom.Game.CurrentPlayerId; ;
            }

            return false;
        }
    }
}