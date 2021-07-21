using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using StarrAPI.Extensions;

namespace StarrAPI.SignalR
{
    [Authorize]
    public class PresentHub : Hub
    {
        private readonly PresentTracker _PT;
        public PresentHub(PresentTracker PT)
        {
            _PT = PT;
        }

        public override async Task OnConnectedAsync()
        {
            await _PT.UserConnected(Context.User.GetUsername(),Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());

            var CurrentUsers = await _PT.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers",CurrentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception Ex)
        {
            await _PT.UserDisconnected(Context.User.GetUsername(),Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());
            var CurrentUsers = await _PT.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers",CurrentUsers);
            await base.OnDisconnectedAsync(Ex);
        }
    }
}