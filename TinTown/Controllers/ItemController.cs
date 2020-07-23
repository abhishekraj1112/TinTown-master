using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ItemLogic _itemLogic;
        public ItemController()
        {
            _itemLogic = new ItemLogic();
        }

        [HttpGet]
        public async Task<JsonResult> FindItem([FromQuery] string name_or_no)
        {
            try
            {
                return await _itemLogic.FindItem(name_or_no).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _itemLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> ItemList([FromQuery] int location_id)
        {
            try
            {
                return await _itemLogic.ItemList(location_id).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _itemLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ItemFullInfo([FromBody] Item icm)
        {
            try
            {
                return await _itemLogic.ItemFullInfo(icm).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _itemLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> ItemCategoryList()
        {
            try
            {
                return await _itemLogic.ItemCategoryList().ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _itemLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ItemCategoryCreate([FromBody] ItemCategoryModel icm)
        {
            try
            {
                return await _itemLogic.ItemCategoryCreate(icm).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _itemLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> ItemSubCategoryList([FromQuery] long id)
        {
            try
            {
                return await _itemLogic.ItemSubCategoryList(id).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _itemLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ItemCategoryDelete([FromBody] itemCategorydeleteModel icdm)
        {
            try
            {
                return await _itemLogic.ItemCategoryDelete(icdm).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _itemLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> ItemAttributeTypeList()
        {
            try
            {
                return await _itemLogic.ItemAttributeTypeList().ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _itemLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }


        [HttpPost]
        public async Task<JsonResult> ItemAttributeTypeCreate([FromBody] itemattributetypeModel iatm)
        {
            try
            {
                return await _itemLogic.ItemAttributeTypeCreate(iatm).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _itemLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> ItemAttributeValueList(long attribute_type_no)
        {
            try
            {
                return await _itemLogic.ItemAttributeValueList(attribute_type_no).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _itemLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ItemAttributeValueCreate([FromBody] itemattributevalueModel iavm)
        {
            try
            {
                return await _itemLogic.ItemAttributeValueCreate(iavm).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _itemLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ItemAttributeDelete([FromBody] itemAttributedeleteModel icdm)
        {
            try
            {
                return await _itemLogic.ItemAttributeDelete(icdm).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _itemLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GstGroupIdValue()
        {
            try
            {
                return await _itemLogic.GstGroupIdValue().ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _itemLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GstHsnCode([FromQuery] int GstGroupId)
        {
            try
            {
                return await _itemLogic.GstHsnCode(GstGroupId).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _itemLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> BaseUomValue()
        {
            try
            {
                return await _itemLogic.BaseUomValue().ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _itemLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ItemCreate([FromBody] Item itm)
        {
            try
            {
                return await _itemLogic.ItemCreate(itm).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _itemLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

    }
}