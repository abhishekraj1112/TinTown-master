using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PickCreateController : ControllerBase
    {
        private readonly PickCreateLogic _pickCreateLogic;

        public PickCreateController()
        {
            _pickCreateLogic = new PickCreateLogic();
        }

        [HttpPost]
        public async Task<JsonResult> FindOrder([FromBody] PickCreate find)
        {
            try
            {
                find.OrderNolist = find.OrderNo.Split('|').ToList();
                find.Pincodelist = find.Pincode.Split('|').ToList();
                find.OrderNolist.Remove("");
                find.OrderNolist.Remove("");
                DataTable dt = _pickCreateLogic.FindOrder(DocumentTypeChange(ref find));
                if (dt.Rows.Count == 0)
                {
                    return await _pickCreateLogic.SendRespose("False", "No order for pick").ConfigureAwait(false);
                }
                return new JsonResult(dt);
            }
            catch (Exception ee)
            {
                return await _pickCreateLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PickCreation([FromBody] PickCreate find)
        {
            try
            {
                if (find.IsManual && find.Picker.Count > find.document_no.Count)
                {
                    return await _pickCreateLogic.SendRespose("False", "Can't assign more picker in less orders").ConfigureAwait(false);
                }

                DocumentTypeChange(ref find);

                if (find.IsManual)
                {

                    int orderPerPicker = find.document_no.Count / find.Picker.Count;
                    int extraToPick = find.document_no.Count % find.Picker.Count;
                    int orderListIndex = 0;

                    foreach (string picker in find.Picker)
                    {
                        for (int i = 1; i <= orderPerPicker + extraToPick; i++)
                        {
                            find.document_no[orderListIndex].picker = picker;
                            orderListIndex++;
                        }
                        extraToPick = 0;
                    }
                }

                return await _pickCreateLogic.PickCreation(find).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _pickCreateLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        public PickCreate DocumentTypeChange(ref PickCreate find)
        {
            if (find.DocumentType == "Sales Order" && find.OrderType == "Company Single Order")
            {
                find.DocumentType = "SINGLE ORDER";
                find.Marketplace = 0;
            }
            else if (find.DocumentType == "Sales Order" && find.OrderType == "Company Multi Order")
            {
                find.Marketplace = 0;
            }
            else if (find.DocumentType == "Sales Order" && find.OrderType == "Marketplace Single Order")
            {
                find.DocumentType = "SINGLE ORDER";
                find.Marketplace = 1;
            }
            else if (find.DocumentType == "Sales Order" && find.OrderType == "Marketplace Multi Order")
            {
                find.Marketplace = 1;
            }
            else
            {
                find.Marketplace = 0;
            }

            return find;
        }
    }
}