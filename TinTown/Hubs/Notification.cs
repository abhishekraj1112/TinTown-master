using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using TinTown.EntryCode;

namespace TinTown.Hubs
{
    public class Notification : Hub
    {
        private readonly ConnectionMapping _connectionMapping;
        public Notification(IHubContext<Notification> hub)
        {
            _connectionMapping = new ConnectionMapping(hub);
        }

        public override Task OnConnectedAsync()
        {
            string email = Context.GetHttpContext().Request.Query["email"];
            _connectionMapping.Add(email, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string email = Context.GetHttpContext().Request.Query["email"];
            _connectionMapping.Remove(email, Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
