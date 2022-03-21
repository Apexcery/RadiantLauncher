using System.Threading.Tasks;
using Radiant.Models;

namespace Radiant.Interfaces
{
    public interface IAuthService
    {
        Task<Account> Login(string username, string password);
    }
}
