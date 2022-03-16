using System.Threading.Tasks;

namespace OpenDIAPIServer
{
    public interface IServerTokenComponent
    {
        Task<string> GetTokenAsync();
        Task<string> RequestTokenAsync();
    }
}