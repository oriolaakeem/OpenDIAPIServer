using System.Net.Http;

namespace OpenDIAPIServer
{
    public class ApplicationClient
    {
        public ApplicationClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }
    }
}
