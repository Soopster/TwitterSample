using System.Threading.Tasks;

namespace TwitterSample.Services
{
    public interface ITwitterAuthService
    {
        Task<string> GetAccessTokenAsync();
    }
}