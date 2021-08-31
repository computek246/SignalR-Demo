using Microsoft.AspNetCore.SignalR;

namespace SignalRDemo.Server.Hub
{


    public class BasedUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst("id")?.Value;
        }
    }

}
