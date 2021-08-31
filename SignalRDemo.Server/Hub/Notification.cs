using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalRDemo.Server.Helpers;
using SignalRDemo.Server.Services;

namespace SignalRDemo.Server.Hub
{

    [AppAuthorize]
    public class Notification : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly ICurrentUserService<string> _currentUserService;

        public Notification(ICurrentUserService<string> currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }


        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

    }
}
