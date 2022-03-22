using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using Radiant.Models;

namespace Radiant.Interfaces
{
    public interface IAuthService
    {
        Task<Account> Login(string username, string password, bool isAddingAccount);
    }
}
