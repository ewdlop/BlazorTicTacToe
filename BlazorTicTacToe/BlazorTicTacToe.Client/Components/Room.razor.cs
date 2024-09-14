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

        protected override async Task OnInitializedAsync()
        {
            if (CurrentRoom is null || HubConnection is null || HubConnection.ConnectionId is null) return;

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
        }

        private async Task StartGame()
        {
            if (HubConnection is null || CurrentRoom is null) return;

            await HubConnection.InvokeAsync("StartGame", CurrentRoom.RoomId);
        }
    }
}