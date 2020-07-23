using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TinTown.Hubs;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserLogic _userLogic;

        private readonly IHubContext<Notification> _hub;

        public UserController(IHubContext<Notification> hub)
        {
            _hub = hub;
            _userLogic = new UserLogic();
        }

        [HttpGet]
        public async Task<JsonResult> AllUser([FromQuery] string name = "", string worktype = "", int location_id = 0)
        {
            try
            {
                return await _userLogic.allUser(name, worktype, location_id).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _userLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateUser([FromBody] User create)
        {
            try
            {
                using (SHA512 sha512 = SHA512.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(create.password);
                    byte[] hash = sha512.ComputeHash(bytes);
                    create.password = Convert.ToBase64String(hash);
                }
                return await _userLogic.createUser(create).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _userLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Login([FromBody] User login)
        {
            try
            {
                using (DataTable dt = _userLogic.login_check(login, "get"))
                {
                    if (dt.Columns.Contains("password_hash"))
                    {
                        byte[] reverse_hashbytes = Convert.FromBase64String(dt.Rows[0]["password_hash"].ToString());
                        using (SHA512 sha512 = SHA512.Create())
                        {
                            byte[] bytes = Encoding.UTF8.GetBytes(login.password);
                            byte[] hash = sha512.ComputeHash(bytes);
                            int ok = 1;
                            for (int i = 0; i < 64; i++)
                            {
                                if (reverse_hashbytes[i] != hash[i])
                                {
                                    ok = 0;
                                    break;
                                }
                            }
                            if (ok == 0)
                            {
                                return await _userLogic.SendRespose("False", "Wrong Password").ConfigureAwait(false);
                            }
                            else
                            {
                                await LogoutOther(login).ConfigureAwait(false);
                                return await _userLogic.login_set(login, "set").ConfigureAwait(false);
                            }
                        }
                    }
                    else
                    {
                        return await _userLogic.SendRespose(dt.Rows[0]["condition"].ToString(), dt.Rows[0]["message"].ToString()).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ee)
            {
                return await _userLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Logout([FromBody] User lout)
        {
            try
            {
                DataTable dt = _userLogic.logout(lout);
                if (dt.Columns.Contains("connection_id"))
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
                return new JsonResult(dt);
            }
            catch (Exception ee)
            {
                return await _userLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        public async Task<JsonResult> LogoutOther(User lout)
        {
            try
            {
                DataTable dt = _userLogic.logout_signalr(lout);
                if (dt.Rows.Count > 0 && dt.Columns.Contains("connection_id"))
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
                return new JsonResult(dt);
            }
            catch (Exception ee)
            {
                return await _userLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateRole([FromBody] User roleupdate)
        {
            try
            {
                return await _userLogic.UpdateRole(roleupdate).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _userLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdatePassword([FromBody] User roleupdate)
        {
            try
            {
                using (SHA512 sha512 = SHA512.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(roleupdate.password);
                    byte[] hash = sha512.ComputeHash(bytes);
                    roleupdate.password = Convert.ToBase64String(hash);
                }
                return await _userLogic.UpdatePassword(roleupdate).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _userLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> LocationList()
        {
            try
            {
                return await _userLogic.LocationList().ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _userLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

    }
}