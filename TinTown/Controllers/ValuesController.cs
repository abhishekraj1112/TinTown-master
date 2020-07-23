using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinTown.EntryCode;
using TinTown.Hubs;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHubContext<Notification> _hub;
        public ValuesController(IHubContext<Notification> hub)
        {
            _hub = hub;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAsync()
        {
            try
            {
                List<string> email = new List<string>() { "arjun@pristinebs.com" };
                List<string> connectionID = await new ConnectionMapping(_hub).GetConnections(email).ConfigureAwait(false);

                SendResponse sendResponse = new SendResponse()
                {
                    Action = "SOS",
                    Message = "MEDDDSE"
                };
                foreach (string connectionId in connectionID)
                {
                    _hub.Clients.Client(connectionId).SendAsync("notification", sendResponse);
                }
                return new string[] { "value1", "value2" };
            }
            catch (Exception ee)
            {
                return new string[] { ee.Message };
            }

        }

        // GET: api/Values/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
