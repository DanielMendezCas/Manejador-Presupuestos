using ManejadorPresupuestos.Models;

namespace ManejadorPresupuestos.Data.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task Create(Account account);
        Task Delete(int id);
        Task<IEnumerable<Account>> Search(int userId);
    }
}
