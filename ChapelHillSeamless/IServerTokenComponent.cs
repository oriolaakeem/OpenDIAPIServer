using System.Threading.Tasks;

namespace ChapelHill
{
    public interface IServerTokenComponent
    {
        Task<string> GetTokenAsync();
        Task<string> RequestTokenAsync();
    }
}