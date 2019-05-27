using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using SupplyChain.Models;
using System;
using SAPbobsCOM;
using Microsoft.AspNetCore.Mvc;
using EkoApiCore.Data;
using Microsoft.Extensions.Configuration;

namespace SAPAPI.Controllers
{
    [Route("api/[controller]")]
    public class PurchaseRequestController : BaseController
    {
        SAPB1 context;
        public PurchaseRequestController(SAPB1 ctx, IConfigurationRoot configuration) : base(configuration)
        {
            context = ctx;
        }

        public override JsonResult Get()
        {
            var ocrds = context.PurchaseRequests.ToList();
            return Json(ocrds);
        }

        [HttpPost]
        [Route("Add")]
        public JsonResult Add([FromBody]PurchaseRequest prModel)
        {
            Connect();
            try
            {
                int oGRNLineCount = prModel.LineItems.Count;

                Documents documents = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseRequest);
                documents.CardCode = prModel.CardCode;
                documents.HandWritten = SAPbobsCOM.BoYesNoEnum.tNO;
                documents.DocDate = prModel.DocDate;
                documents.DocDueDate = prModel.DocDueDate;

                if (oGRNLineCount > 0)
                {
                    foreach (PRQ1 row1 in prModel.LineItems)
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
                return Json(new string[] { "Purchase Order Added Successfully!" });
            }
            catch (Exception ex)
            {
                oCompany.Disconnect();
                return Json(new Error() { ErrorCode = ex.Source, Description = ex.Message });
            }
        }
    }
}