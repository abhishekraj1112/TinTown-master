using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerLogic _customerLogic;

        public CustomerController()
        {
            _customerLogic = new CustomerLogic();
        }

        [HttpGet]
        public async Task<JsonResult> FindCustomer([FromQuery] string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = "";
                }
                return await _customerLogic.FindCustomer(name).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _customerLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateCustomer([FromBody] Customer customer)
        {
            try
            {
                return await _customerLogic.CreateCustomer(customer).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _customerLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> CustomerInfo([FromQuery] string customerid)
        {
            try
            {
                DataTable dt = await _customerLogic.CustomerInfo(customerid).ConfigureAwait(false);
                if (dt.Rows.Count > 0)
                {
                    return new JsonResult(dt);
                }
                else
                {
                    return await _customerLogic.SendRespose("False", "Wrong Customer Id").ConfigureAwait(false);
                }
            }
            catch (Exception ee)
            {
                return await _customerLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> CustomerList()
        {
            try
            {
                DataTable dt = await _customerLogic.CustomerList().ConfigureAwait(false);
                if (dt.Rows.Count > 0)
                {
                    return new JsonResult(dt);
                }
                else
                {
                    return await _customerLogic.SendRespose("False", "Data Not found").ConfigureAwait(false);
                }
            }
            catch (Exception ee)
            {
                return await _customerLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}
