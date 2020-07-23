using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportLogic _reportLogic;
        public ReportController()
        {
            _reportLogic = new ReportLogic();
        }

        [HttpGet]
        public async Task<JsonResult> PickInfo([FromQuery] string pickno)
        {
            try
            {
                return await _reportLogic.PickInfo(pickno).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _reportLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> BinZoneSearch([FromQuery] string filter, int page)
        {
            try
            {
                return await _reportLogic.BinZoneSearch(filter, page).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _reportLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> PickDistribution()
        {
            try
            {
                return await _reportLogic.PickDistribution().ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _reportLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> SaleOrder([FromQuery] string source_no)
        {
            try
            {
                List<Report> repotlist = new List<Report>();
                Report repot = new Report();
                source_no = "'" + source_no.Replace("|", "','") + "'";
                DataSet ds = await _reportLogic.SaleOrder(source_no).ConfigureAwait(false);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    repot.condition = "True";
                    repot.message = "";
                    repot.sale_headers = ds.Tables[0];
                    repot.sale_lines = ds.Tables[1];

                    repotlist.Add(repot);
                    return new JsonResult(repotlist);
                }
                else
                {
                    repot.condition = "False";
                    repot.message = "";
                    repotlist.Add(repot);
                    return new JsonResult(repotlist);
                }
            }
            catch (Exception ee)
            {
                return await _reportLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> OQCRangeReport()
        {
            try
            {
                return await _reportLogic.OQCRangeReport().ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _reportLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> ConsolidationRangeReport()
        {
            try
            {
                return await _reportLogic.ConsolidationRangeReport().ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _reportLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SlotInfo([FromBody] SlotReport slotReport)
        {
            try
            {
                return await _reportLogic.SlotInfo(slotReport).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return await _reportLogic.SendRespose("False", e.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> SlotSingleReport()
        {
            try
            {
                return await _reportLogic.SlotSingleReport().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return await _reportLogic.SendRespose("False", e.Message).ConfigureAwait(false);
            }
        }
    }
}