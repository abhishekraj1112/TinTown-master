using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TinTown.Hubs;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.EntryCode
{
    public class ConnectionMapping
    {
        private readonly SignalRLogic _SRconnectionBL;
        private readonly IHubContext<Notification> _hub;

        public ConnectionMapping(IHubContext<Notification> hub)
        {
            _hub = hub;
            _SRconnectionBL = new SignalRLogic();
        }

        public async void Add(string key, string connectionId)
        {
            DataTable dt = await _SRconnectionBL.Signalr_access("insert", key, connectionId).ConfigureAwait(false);
            if (dt.Rows.Count > 0)
            {
                SendResponse sendResponse = new SendResponse()
                {
                    Action = "Logout",
                    Message = "Logout Your device"
                };
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _hub.Clients.Client(dt.Rows[i]["connection_id"].ToString()).SendAsync("logoutAllDevices", sendResponse);
                }
            }
        }

        public async Task<List<string>> GetConnections(List<string> key)
        {
            return await _SRconnectionBL.Signalr_access(key).ConfigureAwait(false);
        }

        public async void Remove(string key, string connectionId)
        {
            DataTable dt = await _SRconnectionBL.Signalr_access("delete", key, connectionId).ConfigureAwait(false);
            if (dt.Rows.Count > 0)
            {
                SendResponse sendResponse = new SendResponse()
                {
                    Action = "Logout",
                    Message = "Logout Your device"
                };
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _hub.Clients.Client(dt.Rows[i]["connection_id"].ToString()).SendAsync("logoutAllDevices", sendResponse);
                }
            }
        }
    }
}
