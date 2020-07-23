using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly VendorLogic _vendorLogic;
        public VendorController()
        {
            _vendorLogic = new VendorLogic();
        }


        [HttpGet]
        public async Task<JsonResult> GetVendorDetail([FromQuery] string no_or_name = "")
        {
            try
            {
                if (string.IsNullOrEmpty(no_or_name))
                {
                    no_or_name = "";
                }
                return await _vendorLogic.GetVendorDetail(no_or_name).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _vendorLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }


        [HttpGet]
        public async Task<JsonResult> AllVendorList()
        {
            try
            {
                return await _vendorLogic.AllVendorList().ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _vendorLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }


        [HttpPost, DisableRequestSizeLimit]
        public async Task<JsonResult> CreateVendor([FromForm] VendorModel vm)
        {
            try
            {
                List<FileModel> file = new List<FileModel>();
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "vendor_attachment")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "vendor_attachment"));
                }

                if (vm.file != null)
                {
                    if (vm.file.Count > 0)
                    {
                        for (int i = 0; i < vm.file.Count; i++)
                        {
                            using (FileStream targetStream = System.IO.File.Create(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "vendor_attachment", vm.file[i].FileName)))
                            {
                                vm.file[i].CopyTo(targetStream);
                            }

                            FileModel fileUrl = new FileModel
                            {
                                id = 0,
                                file = Path.Combine("vendor_attachment", vm.file[i].FileName)
                            };
                            file.Add(fileUrl);
                        }
                    }
                }
                return await _vendorLogic.CreateVendor(vm, file).ConfigureAwait(false);

            }
            catch (Exception ee)
            {
                return await _vendorLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> VendorCatalogueList()
        {
            try
            {
                return await _vendorLogic.VendorCatalogueList().ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _vendorLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateUpdateVendorCatalogue(VendorCatalogueModel vcm)
        {
            try
            {
                return await _vendorLogic.CreateUpdateVendorCatalogue(vcm).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _vendorLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteVendorCatalogue(VendorCatalogueModel vcm)
        {
            try
            {
                return await _vendorLogic.DeleteVendorCatalogue(vcm).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _vendorLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }


    }
}