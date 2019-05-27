using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc;
using EkoApiCore.Data;
using Microsoft.Extensions.Configuration;

namespace SAPAPI.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : BaseController
    {
        SAPB1 context;
        public ItemsController(SAPB1 ctx, IConfigurationRoot configuration) : base(configuration)
        {
            context = ctx;
        }

        public override JsonResult Get()
        {
            return Json(context.Items.ToList());
        }

        [HttpGet]
        public JsonResult GetForLookUp()
        {
           
            
                var items = context.Items.ToList();

                //var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return Json(items);
            
        }
    }
}