using System.Threading;
using System.Threading.Tasks;
using Radiant.Models;

namespace Radiant.Interfaces
{
    public interface IAuthService
    {
        Task<Account> Login(CancellationToken cancellationToken, string username, string password, bool isAddingAccount = false);
    }
}
