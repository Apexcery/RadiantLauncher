using System.Threading.Tasks;

namespace Radiant.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Login(string username, string password);
    }
}
