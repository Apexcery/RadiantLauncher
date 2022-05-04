using System.Threading;
using System.Threading.Tasks;

namespace Radiant.Interfaces
{
    public interface IAuthService
    {
        Task Login(CancellationToken cancellationToken, string username, string password, bool isAddingAccount = false);
    }
}
