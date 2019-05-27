using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using SupplyChain.Models;
using Microsoft.AspNetCore.Mvc;
using EkoApiCore.Data;
using AutoMapper;
using EkoApi.Services;
using Microsoft.Extensions.Configuration;

namespace SAPAPI.Controllers
{
    [Route("api/[controller]")]
    public class OCRDController : BaseController
    {
        SAPB1 context;
        public OCRDController(SAPB1 ctx, IConfigurationRoot configuration) : base(configuration)
        {
            context = ctx;
        }


        public override JsonResult  Get()
        {
                var ocrds = context.BusinessPatners.ToList();
                return Json(ocrds);
        }

        //[HttpGet]
        //public JsonResult GetForLookUp()
        //{
        //    using (var context = new SAPB1())
        //    {
        //        var ocrds = _employeeService.BPLoookups.SqlQuery("SELECT CardCode,CardName, CardType FROM OCRD  ").ToList();

        //        var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        //        return Json(ocrds, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //[HttpGet]
        //public JsonResult GetForLookUp(string searchString)
        //{
        //    using (var context = new SAPB1())
        //    {
        //        var ocrds = context.BusinessPatners.SqlQuery(string.Format("SELECT CardCode,CardName FROM OCRD where CardCode = '{0}' or CardName = '{0}' or CardType = '{0}'", searchString)).ToList();

        //        var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        //        return Json(ocrds, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //[HttpPost]
        //public JsonResult AddBP( BusinessPatner bp)
        //{
        //    var cmpName = b1.B1Company.CompanyName;
        //    return Json("testing completer", JsonRequestBehavior.AllowGet);
        //}

    }
}