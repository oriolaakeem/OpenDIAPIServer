using System.Net.Http;

namespace ChapelHill
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
