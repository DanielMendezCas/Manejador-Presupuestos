using Dapper;
using ManejadorPresupuestos.Data.Repositories.Interfaces;
using ManejadorPresupuestos.Models;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace ManejadorPresupuestos.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string connectionString;
        public AccountRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Default");
        }

        public async Task Create(Account account)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                (@"INSERT INTO Accounts(AccountName, AccountTypeId, Balance, [Description]) 
                VALUES (@AccountName, @AccountTypeId, @Balance, @Description);

                SELECT SCOPE_IDENTITY();", account);
            account.Id = id;
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync
                (@"DELETE Accounts WHERE Id = @Id;", new {id});
        }

        public async Task Update(AccountCreateViewModel model)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync
                (@"UPDATE Accounts SET AccountName = @AccountName, 
                   AccountTypeId = @AccountTypeId, 
                   Balance = @Balance, 
                   [Description] = @Description
                   WHERE Id = @Id;", model
                );
        }

        public async Task<Account> GetById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Account>
                (@"SELECT ac.Id, ac.AccountName, ac.Balance, ac.Description, ac.AccountTypeId
                   FROM Accounts ac
                   INNER JOIN AccountTypes act
                   ON act.Id = ac.AccountTypeId
                   WHERE act.UserId = @UserId AND ac.Id = @Id", new {id, userId}
                );
        }

        public async Task<IEnumerable<Account>> Search(int userId)
        {
            using var connection = new SqlConnection (connectionString);
            return await connection.QueryAsync<Account>
                (@"SELECT ac.Id, ac.AccountName, ac.Balance, act.AccountName AS AccountType
                   FROM Accounts ac
                   INNER JOIN AccountTypes act
                   ON act.Id = ac.AccountTypeId
                   WHERE act.UserId = @UserId
                   ORDER BY act.[Order];", new {userId}
                );
        }
    }
}
