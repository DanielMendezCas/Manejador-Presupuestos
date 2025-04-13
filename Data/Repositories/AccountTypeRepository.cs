using Dapper;
using ManejadorPresupuestos.Data.Repositories.Interfaces;
using ManejadorPresupuestos.Models;
using Microsoft.Data.SqlClient;

namespace ManejadorPresupuestos.Data.Repositories
{
    public class AccountTypeRepository : IAccountTypeRepository
    {
        private readonly string connectionString;

        public AccountTypeRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Default");
        }

        public async Task CreateAccount(AccountType accountType)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                (@"
                INSERT INTO AccountTypes (AccountName, UserId, [Order])
                VALUES (@AccountName, @UserId, 0);
                SELECT SCOPE_IDENTITY();", accountType);
            accountType.Id = id;
        }

        public async Task<bool> Exists(string accountName, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            var exists = await connection.QueryFirstOrDefaultAsync<int>
                (@"
                SELECT 1
                FROM AccountTypes
                WHERE AccountName = @AccountName AND UserId = @UserId",
                new { accountName, userId });

            return exists == 1;
        }

        public async Task<IEnumerable<AccountType>> GetAccounts(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            var get = await connection.QueryAsync<AccountType>
                (@"
                SELECT Id, AccountName, [Order] 
                FROM AccountTypes 
                WHERE UserId = @UserId", new { userId });

            return get;
        }

        public async Task UpdateAccount(AccountType accountType)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync
                (@"
                UPDATE AccountTypes
                SET AccountName = @AccountName
                WHERE Id = @Id
                ", accountType);
        }

        public async Task<AccountType> GetAccountById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<AccountType>
                (@"
                SELECT Id, AccountName, [Order]
                FROM AccountTypes
                WHERE Id = @Id AND UserId = @UserId
                ", new {id, userId});
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync
                (@"
                DELETE AccountTypes WHERE Id = @Id
                ", new {id});
        }
    }
}