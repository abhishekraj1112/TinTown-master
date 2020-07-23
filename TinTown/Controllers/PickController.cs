using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinTown.Hubs;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PickController : ControllerBase
    {
        private readonly PickLogic _PickLogic;

        private readonly IHubContext<Notification> _hub;

        public PickController(IHubContext<Notification> hub)
        {
            _PickLogic = new PickLogic();
            _hub = hub;
        }

        [HttpGet]
        public async Task<JsonResult> PickZone()
        {
            try
            {
                return await _PickLogic.pickZone().ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _PickLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> PickerInfo([FromQuery] string email, int location_id)
        {
            try
            {
                List<ReturnResponse> returnResponse = new List<ReturnResponse>
                {
                    await _PickLogic.PickerInfo(email, location_id).ConfigureAwait(false)
                };
                if (returnResponse[0].barcode != null)
                {
                    returnResponse[0].images = await _PickLogic.GetImage(returnResponse[0].barcode).ConfigureAwait(false);
                }
                return new JsonResult(returnResponse);
            }
            catch (Exception ee)
            {
                return await _PickLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PickStart([FromBody] GetPick getPick)
        {
            try
            {
                List<ReturnResponse> returnResponse = new List<ReturnResponse>
                {
                    await _PickLogic.PickStart(getPick).ConfigureAwait(false)
                };
                if (returnResponse[0].barcode != null)
                {
                    returnResponse[0].images = await _PickLogic.GetImage(returnResponse[0].barcode).ConfigureAwait(false);
                }
                return new JsonResult(returnResponse);
            }
            catch (Exception ee)
            {
                return await _PickLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ScanBinBarcode([FromBody] PickScan pickScan)
        {
            try
            {
                List<ReturnResponse> returnResponse = new List<ReturnResponse>()
                {
                    await _PickLogic.ScanBinBarcode(pickScan).ConfigureAwait(false)
                };
                if (returnResponse[0].barcode != null)
                {
                    returnResponse[0].images = await _PickLogic.GetImage(returnResponse[0].barcode).ConfigureAwait(false);
                }
                return new JsonResult(returnResponse);
            }
            catch (Exception ee)
            {
                return await _PickLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ProductNotFound([FromBody] PickScan pNF)
        {
            try
            {
                List<ReturnResponse> returnResponse = new List<ReturnResponse>()
                {
                    await _PickLogic.ProductNotFound(pNF).ConfigureAwait(false)
                };
                if (returnResponse[0].barcode != null)
                {
                    returnResponse[0].images = await _PickLogic.GetImage(returnResponse[0].barcode).ConfigureAwait(false);
                }
                return new JsonResult(returnResponse);
            }
            catch (Exception ee)
            {
                return await _PickLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ProductImageMisMatch([FromBody] PickScan PIMM)
        {
            try
            {
                List<ReturnResponse> returnResponse = new List<ReturnResponse>()
                {
                    await _PickLogic.ProductImageMisMatch(PIMM).ConfigureAwait(false)
                };
                if (returnResponse[0].barcode != null)
                {
                    returnResponse[0].images = await _PickLogic.GetImage(returnResponse[0].barcode).ConfigureAwait(false);
                }
                return new JsonResult(returnResponse);
            }
            catch (Exception ee)
            {
                return await _PickLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<string> NotFoundNewLine([FromBody] NAVNF pickNF)
        {
            try
            {
                return await _PickLogic.NotFoundNewLine(pickNF.Nflines).ConfigureAwait(false);
            }
            catch (Exception)
            {
                return "UnSuccess";
            }
        }

        [HttpPost]
        public async Task<JsonResult> CloseTray([FromBody] PickScan closetray)
        {
            try
            {
                List<ReturnResponse> returnResponse = new List<ReturnResponse>()
                {
                    await _PickLogic.CloseTray(closetray).ConfigureAwait(false)
                };
                if (returnResponse[0].barcode != null)
                {
                    returnResponse[0].images = await _PickLogic.GetImage(returnResponse[0].barcode).ConfigureAwait(false);
                }
                return new JsonResult(returnResponse);
            }
            catch (Exception ee)
            {
                return await _PickLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CancelOrderComplete([FromBody] OrderCancel ordercancel)
        {
            try
            {
                return await _PickLogic.CancelOrderComplete(ordercancel).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _PickLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> PickPriority([FromQuery] string worktype, int location_id)
        {
            try
            {
                return await _PickLogic.PickPriority(worktype, location_id).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _PickLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ChangePickPriority([FromBody] Priority change)
        {
            try
            {
                return await _PickLogic.ChangePickPriority(change).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _PickLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> PriorityList()
        {
            try
            {
                return await _PickLogic.PriorityList().ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _PickLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PickInfoForForceAssigment([FromBody] PickSearch PickNo)
        {
            try
            {
                return await _PickLogic.PickInfoForForceAssigment(PickNo).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _PickLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ForceAssigment([FromBody] ForcePickAssig forcePick)
        {
            try
            {
                ReturnMessage returnMessage = await _PickLogic.ForceAssigment(forcePick).ConfigureAwait(false);

                if (returnMessage.sendResponse.Count > 0)
                {
                    foreach (SendResponse connectionId in returnMessage.sendResponse)
                    {
                        _hub.Clients.Client(connectionId.ConnectionID).SendAsync("forceassigment", connectionId);
                    }
                }
                return new JsonResult(returnMessage.data);
            }
            catch (Exception ee)
            {
                return await _PickLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PickSetupAll([FromBody] PickSetup setup)
        {
            try
            {
                return await _PickLogic.PickSetupAll(setup).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _PickLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> OrderHoldOrUnHold([FromBody] OrderCancel stage)
        {
            try
            {
                return await _PickLogic.OrderHoldOrUnHold(stage).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _PickLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}