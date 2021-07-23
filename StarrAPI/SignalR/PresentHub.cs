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
            var isOnline = await _PT.UserConnected(Context.User.GetUsername(),Context.ConnectionId);
            if(isOnline)
                await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());

            var CurrentUsers = await _PT.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers",CurrentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception Ex)
        {
            var isOffline = await _PT.UserDisconnected(Context.User.GetUsername(),Context.ConnectionId);

            if(isOffline)
                await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());
            await base.OnDisconnectedAsync(Ex);
        }
    }
}