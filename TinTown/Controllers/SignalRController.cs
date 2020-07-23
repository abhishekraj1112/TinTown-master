using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using TinTown.Hubs;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SignalRController : ControllerBase
    {
        private readonly IHubContext<Notification> _hub;
        private readonly SignalRLogic _signalRLogic;
        public SignalRController(IHubContext<Notification> hub)
        {
            _hub = hub;
            _signalRLogic = new SignalRLogic();
        }

        public async Task ClientRecevie(RequestAction message)
        {
            await _hub.Clients.All.SendAsync("notification", message).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<JsonResult> AllNotification([FromQuery] string email)
        {
            try
            {
                return await _signalRLogic.AllNotification(email).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _signalRLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}