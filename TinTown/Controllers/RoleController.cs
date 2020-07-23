using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleLogic _roleLogic;
        public RoleController()
        {
            _roleLogic = new RoleLogic();
        }

        [HttpPost]
        public async Task<JsonResult> RoleProcess([FromBody] Role role)
        {
            try
            {
                return await _roleLogic.Role_process(role).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _roleLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }


        [HttpGet("{role_id}")]
        public async Task<JsonResult> RolePermissionDetail(int role_id)
        {
            try
            {
                return await _roleLogic.RolePermissionDetail(role_id).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _roleLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> RolePermissionUpdate([FromBody] NewRole newRole)
        {
            try
            {
                return await _roleLogic.RolePermissionUpdate(newRole).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _roleLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}