namespace OpenDIAPIServer.models
{
    public class SAP
    {
        public string DBServer { get; set; }
        public string LicenseServer { get; set; }
        public string CompanyDB { get; set; }
        public string DBUserName { get; set; }
        public string DBPassword { get; set; }
        public string SAPUserName { get; set; }
        public string SAPPassword { get; set; }
        public string webpagesVersion { get; set; }
        public string webpagesEnabled { get; set; }
        public string ClientValidationEnabled { get; set; }
        public string UnobtrusiveJavaScriptEnabled { get; set; }
    }
}
