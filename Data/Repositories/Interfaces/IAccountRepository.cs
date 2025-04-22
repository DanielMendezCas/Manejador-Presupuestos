using ManejadorPresupuestos.Models;

namespace ManejadorPresupuestos.Data.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task Create(Account account);
        Task Delete(int id);
        Task Update(AccountCreateViewModel model);
        Task<Account> GetById(int id, int userId);
        Task<IEnumerable<Account>> Search(int userId);
    }
}
