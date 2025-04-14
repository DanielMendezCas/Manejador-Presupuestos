using ManejadorPresupuestos.Models;

namespace ManejadorPresupuestos.Data.Repositories.Interfaces
{
    public interface IAccountTypeRepository
    {
        Task CreateAccount(AccountType accountType);
        Task<bool> Exists(string accountName, int userId);
        Task<IEnumerable<AccountType>> GetAccounts(int userId);
        Task UpdateAccount(AccountType accountType);
        Task<AccountType> GetAccountById(int id, int userId);
        Task Delete(int id);
        Task Order(IEnumerable<AccountType> accounts);
    }
}
