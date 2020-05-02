using Dapper;
using mb_back.Models;
using mb_back.ServicesInterface;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mb_back.Services
{
    public class AccountService:IAccountService
    {
        private const string ConnectionString =
           "server=localhost;database=mb;userid=postgres;password=password;Pooling=false";
        public async Task<List<Account>> GetAllByUserId(int id)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var res =  await connection.QueryMultipleAsync(
                    "SELECT * FROM Accounts WHERE user_id = @id", new { id });
                var accounts = res.Read<Account>().ToList();
                Console.WriteLine("\n----------------- \n");
                Console.Write(accounts.Count());
                Console.WriteLine("\n----------------- \n");
                return accounts;
            }
        }

        public async Task<long> CreateAccount(int UserId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var res = await connection.QueryMultipleAsync("INSERT INTO Accounts (user_id) VALUES (@Userid) Returning (id)", new { UserId });
                var id = res.ReadSingle<long>();
                return id;
            }
        }
        public async Task<Account> GetAccount(long accountId, int userId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var res = await connection.QueryMultipleAsync("SELECT * FROM Accounts WHERE id=@accountID AND user_id=@userId", new { accountId, userId });
                var account = res.ReadSingle<Account>();
                return account;
            }
        }
    }
}
