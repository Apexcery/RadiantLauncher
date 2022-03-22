using System.Threading;
using System.Threading.Tasks;
using Radiant.Models;
using Radiant.Models.AppConfigs;

namespace Radiant.Interfaces
{
    public interface IAuthService
    {
        Task<Account> Login(CancellationToken cancellationToken, string username, string password, bool isAddingAccount = false);
    }
}
