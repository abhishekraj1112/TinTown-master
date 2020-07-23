using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SortingController : ControllerBase
    {
        private readonly SortingLogic _sortingLogic;
        public SortingController()
        {
            _sortingLogic = new SortingLogic();
        }

        [HttpPost]
        public async Task<JsonResult> ScanTray([FromBody] Sorting sorting)
        {
            try
            {
                return await _sortingLogic.ScanTray(sorting).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _sortingLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ScanCage([FromBody] Sorting sorting)
        {
            try
            {
                return await _sortingLogic.ScanCage(sorting).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _sortingLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}