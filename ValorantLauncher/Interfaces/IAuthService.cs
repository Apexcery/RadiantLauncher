using System.Threading.Tasks;

namespace ValorantLauncher.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Login(string username, string password);
    }
}
