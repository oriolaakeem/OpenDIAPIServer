using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using SupplyChain.Models;
using System;
using SAPbobsCOM;
using Microsoft.AspNetCore.Mvc;
using EkoApiCore.Data;
using AutoMapper;
using EkoApi.Services;
using Microsoft.Extensions.Configuration;

namespace SAPAPI.Controllers
{
    [Route("api/[controller]")]
    public class GRNController : BaseController
    {
        private readonly GRNServices _employeeService;
        private readonly IMapper _mapper;

        public GRNController(GRNServices employeeService, IConfigurationRoot configuration, IMapper mapper):base(configuration)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }


        [Route("Get")]
        public override JsonResult  Get()
        {
                var ocrds = _employeeService.Get();
                return Json(ocrds);
        }


        //[HttpPost ValidateModelState]
        [HttpPost]
        [Route("Add")]
        public JsonResult Add([FromBody]GRN grnModel)
        {
            Connect();
            try
            {
                int oGRNLineCount = grnModel.LineItems.Count;

                Documents documents = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);
                documents.CardCode = grnModel.CardCode;
                documents.HandWritten = SAPbobsCOM.BoYesNoEnum.tNO;
                documents.DocDate = grnModel.DocDate;
                documents.DocDueDate = grnModel.DocDueDate;

                if (oGRNLineCount > 0)
                {
                    foreach (IGN1 row1 in grnModel.LineItems)
                    {
                        documents.Lines.ItemCode = row1.ItemCode;
                        //documents.Lines.ItemDescription = row1.des;
                        documents.Lines.Price = row1.Price;
                        documents.Lines.ShipDate = DateTime.Today;
                        documents.Lines.Quantity = row1.Quantity;
                        documents.Lines.Add();
                    }
                }
                int resp = documents.Add();

                if (resp != 0)
                {
                    return Json(new Error() { ErrorCode = oCompany.GetLastErrorCode().ToString(), Description = oCompany.GetLastErrorDescription() });
                }
                return Json(new string[] { "GRN Added Successfully!" });
            }
            catch (Exception ex)
            {
                oCompany.Disconnect();
                return Json(new Error() { ErrorCode = ex.Source, Description = ex.Message });
            }
        }

    }
}