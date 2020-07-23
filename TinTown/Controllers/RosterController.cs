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
    public class RosterController : ControllerBase
    {
        private readonly RosterLogic _rosterLogic;
        private readonly IHubContext<Notification> _hub;

        public RosterController(IHubContext<Notification> hub)
        {
            _rosterLogic = new RosterLogic();
            _hub = hub;
        }

        [HttpPost]
        public async Task<JsonResult> CreateRoster([FromBody] Roster newshift)
        {
            try
            {
                ReturnMessage returnMessage = await _rosterLogic.CreateRoster(newshift).ConfigureAwait(false);

                if (returnMessage.sendResponse.Count > 0)
                {
                    foreach (SendResponse connectionId in returnMessage.sendResponse)
                    {
                        _hub.Clients.Client(connectionId.ConnectionID).SendAsync("zonechange", connectionId);
                    }
                }

                return new JsonResult(returnMessage.data);
            }
            catch (Exception ee)
            {
                return await _rosterLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> ManagerData(int location_id)
        {
            try
            {
                return await _rosterLogic.ManagerData(location_id).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _rosterLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> AllRoster([FromQuery] string Email, int location_id)
        {
            try
            {
                return await _rosterLogic.AllRoster(Email, location_id).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _rosterLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> PickerInZone([FromQuery] string zone, int location_id)
        {
            try
            {
                return await _rosterLogic.PickerInZone(zone, location_id).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _rosterLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}