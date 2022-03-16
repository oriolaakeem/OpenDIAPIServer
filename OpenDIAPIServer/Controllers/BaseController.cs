using OpenDIAPIServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SAPbobsCOM;
using System;
using System.Text;

namespace SAPAPI.Controllers
{
    public abstract  class BaseController : Controller
    {
        protected Company oCompany = new Company();
        protected int lErrCode;
        protected string sErrMsg = "";

        private readonly IConfigurationRoot _optCron;

        public BaseController(IConfigurationRoot optCron)
        {
            _optCron = optCron;
        }

        [HttpGet]
        public abstract JsonResult Get();

        protected void Connect()
        {
            try
            {
                oCompany.Server = _optCron["SAP:LicenseServer"];
                oCompany.CompanyDB = _optCron["SAP:CompanyDB"];
                oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2008;

                //db user name
                oCompany.DbUserName = _optCron["SAP:DBUserName"];
                oCompany.DbPassword = _optCron["SAP:DBPassword"];
                //user name
                oCompany.UserName = _optCron["SAP:SAPUserName"];
                //user password
                oCompany.Password = _optCron["SAP:SAPPassword"];
                oCompany.language = SAPbobsCOM.BoSuppLangs.ln_English;

                // Use Windows authentication for database server.
                // True for NT server authentication,
                // False for database server authentication.
                oCompany.UseTrusted = false;

                //insert license server and port
                oCompany.LicenseServer = _optCron["SAP:LicenseServer"] + ":30000";

                //Connecting to a company DB
                int lRetCode = oCompany.Connect();

                if (lRetCode != 0)
                {
                    oCompany.GetLastError(out lErrCode, out sErrMsg);
                    oCompany.Disconnect();
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //protected virtual JsonResult Json(object data, string contentType,
        //   Encoding contentEncoding, JsonRequestBehavior behavior)
        //{
        //    return new System.Web.Mvc.JsonResult()
        //    {
        //        Data = data,
        //        ContentType = contentType,
        //        ContentEncoding = contentEncoding,
        //        JsonRequestBehavior = behavior,
        //        MaxJsonLength = int.MaxValue
        //    };
        //}

        //protected virtual System.Web.Mvc.JsonResult Json(object data, System.Web.Mvc.JsonRequestBehavior behavior)
        //{
        //    return new System.Web.Mvc.JsonResult()
        //    {
        //        Data = data,
        //        JsonRequestBehavior = behavior,
        //        MaxJsonLength = int.MaxValue
        //    };
        //}
    }
}
