using BlazorTicTacToeShared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorTicTacToe.Client.Components
{
    public partial class Room : ComponentBase
    {
        [Parameter]
        public GameRoom? CurrentRoom { get; set; }
        [CascadingParameter]
        public HubConnection? HubConnection { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (CurrentRoom is null || HubConnection is null || HubConnection.ConnectionId is null) return;

            HubConnection.On<Player>("PlayerJoined", player =>
            {
                CurrentRoom.Players.Add(player);
                StateHasChanged();
            });
        }
    }
}