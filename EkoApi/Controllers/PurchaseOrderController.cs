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
    public class PurchaseOrderController : BaseController
    {
        SAPB1 context;
        public PurchaseOrderController(SAPB1 ctx, IConfigurationRoot configuration) : base(configuration)
        {
            context = ctx;
        }

        public override JsonResult  Get()
        {
                var ocrds = context.PurchaseOrders.ToList();
                return Json(ocrds);
        }

        [HttpPost]
        [Route("Add")]
        public JsonResult Add([FromBody]PurchaseOrder poModel)
        {
            Connect();
            try
            {
                int oGRNLineCount = poModel.LineItems.Count;

                Documents documents = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
                documents.CardCode = poModel.CardCode;
                documents.HandWritten = SAPbobsCOM.BoYesNoEnum.tNO;
                documents.DocDate = poModel.DocDate;
                documents.DocDueDate = poModel.DocDueDate;

                if (oGRNLineCount > 0)
                {
                    foreach (POR1 row1 in poModel.LineItems)
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