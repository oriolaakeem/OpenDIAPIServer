using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ChapelHill.Services
{
    public class XDocumentInputFormatter : InputFormatter, IInputFormatter, IApiRequestFormatMetadataProvider
    {
        public XDocumentInputFormatter()
        {
            SupportedMediaTypes.Add("application/xml");
        }

        protected override bool CanReadType(Type type)
        {
            if (type.IsAssignableFrom(typeof(XDocument))) return true;
            return base.CanReadType(type);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            // Use StreamReader to convert any encoding to UTF-16 (default C# and sql Server).
            using (var streamReader = new StreamReader(context.HttpContext.Request.Body))
            {
                var xmlDoc = XDocument.Load(streamReader, LoadOptions.None);
               return await Task.Run(() => InputFormatterResult.Success(xmlDoc));
            }
        }
    }
}
