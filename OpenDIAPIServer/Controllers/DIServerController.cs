using Microsoft.AspNetCore.Mvc;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using OpenDIAPIServer.models;
using System.Text;
using OpenDIAPIServer;
using System.Net.Http;
using System.Threading.Tasks;

namespace SAPAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/xml")]
    [Authorize]
    public class DIServerController : ControllerBase
    {

        private readonly SAP _sapOptions;
        private readonly ApplicationClient _client;
        private readonly IServerTokenComponent _tokenServer;
        public DIServerController(SAP sapOptions, ApplicationClient client, IServerTokenComponent tokenServer)
        {
            _sapOptions = sapOptions;
            _tokenServer = tokenServer;
            _client = client;
        }

        [HttpGet]
        [Route("GetSessionId")]
        public async Task<IActionResult> GetSessionId(string dbName)
        {
            string xmlParam = $@"<?xml version='1.0'?>
                                                <env:Envelope xmlns:env='http://schemas.xmlsoap.org/soap/envelope/'>
                                                    <env:Body>
                                                        <dis:Login xmlns:dis='http://www.sap.com/SBO/DIS'>
                                                            <DatabaseServer>{_sapOptions.DBServer}</DatabaseServer>
                                                            <DatabaseName>{dbName}</DatabaseName>
                                                            <DatabaseType>dst_MSSQL2012</DatabaseType>
                                                            <DatabaseUsername>{_sapOptions.DBUserName}</DatabaseUsername>
                                                            <DatabasePassword>{_sapOptions.DBPassword}</DatabasePassword>
                                                            <CompanyUsername>{_sapOptions.SAPUserName}</CompanyUsername>
                                                            <CompanyPassword>{_sapOptions.SAPPassword}</CompanyPassword>
                                                            <Language>ln_English</Language>
                                                            <LicenseServer>{_sapOptions.LicenseServer}</LicenseServer>
                                                         </dis:Login>
                                                    </env:Body>
                                                </env:Envelope>";
            XElement xml = GetElement(xmlParam);
            var token = await _tokenServer.GetTokenAsync();
            _client.Client.SetBearerToken(token);
            var response = await _client.Client.PostAsJsonAsync("api/DIServer/Interact", xml);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStreamAsync();
                return Ok(json);
            }
            return BadRequest();
        }

        private static XElement GetElement(string xml)
        {
            XElement xmlTree = XElement.Parse(xml);
            return xmlTree;
        }

        [HttpPost]
        [Route("Interact")]
        public IActionResult Interact([FromBody]XElement xmldocument)
        {
            StringBuilder sb = new StringBuilder();
            XmlDocument sRetVal = null;
            XmlDocument xmlDoc = new XmlDocument();
            SBODI_Server.Node pDISnode = null;
            string sSOAPans = null;

            pDISnode = new SBODI_Server.Node();
            sb.Append("<?xml version='1.0'?>");
            string text = xmldocument.ToString();
            sb.Append(text);

            sSOAPans = pDISnode.Interact(sb.ToString());

            // get parsed result
            sRetVal = GetParseResultFromSoapAnswer(sSOAPans);

            // return result
            return Ok(sRetVal);
        }

        private XmlDocument GetParseResultFromSoapAnswer(string sSOAPans)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(sSOAPans);
            
            return xml;
        }
    }
}