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
                    "SELECT * FROM Accounts WHERE user_id = @id And is_close=false", 
                    new { id });
                var accounts = res.Read<Account>().ToList();

                return accounts;
            }
        }

        public async Task<long> CreateAccount(int UserId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var res = await connection.QueryMultipleAsync("INSERT INTO Accounts (user_id) VALUES (@Userid) Returning (id)", 
                    new { UserId });
                var id = res.ReadSingle<long>();
                return id;
            }
        }
        public async Task<Account> GetAccount(long accountId, int userId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var res = await connection.QueryMultipleAsync("SELECT * FROM Accounts WHERE id=@accountID AND user_id=@userId AND is_close=false", 
                    new { 
                        accountId, 
                        userId 
                    });
                var account = res.ReadSingle<Account>();
                return account;
            }
        }

        public async Task<long> GetAccountIdByUserID(int userId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var res = await connection.QueryFirstOrDefaultAsync<long>("SELECT id FROM Accounts WHERE user_id=@userId and is_close=false", 
                    new { userId });
                return res;
            }
        }
        public async Task<Operation> Transfer(Operation newOperation)
        {
            newOperation.Date = DateTime.Now;
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.QueryAsync("INSERT INTO OPERATIONS (amount, date, account_in_id ,account_out_id, operation_type ) values (@amount, @date, @account_in_id, @account_out_id, 'Перевод')",
                    new { 
                        newOperation.Amount, 
                        newOperation.Date, 
                        newOperation.Account_in_id, 
                        newOperation.Account_out_id 
                    });
                await this.ChangeBalance( newOperation.Account_in_id, newOperation.Amount);
                await this.ChangeBalance(newOperation.Account_out_id, -newOperation.Amount);
                return newOperation;
            }
        }
        public async Task<Operation> Replenishment(Operation newOperation)
        {

            newOperation.Date = DateTime.Now;
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.QueryAsync("INSERT INTO OPERATIONS (amount, date, account_in_id,operation_type ) values (@amount, @date, @account_in_id, 'пополнение')", 
                    new { 
                        newOperation.Amount,
                        newOperation.Date,
                        newOperation.Account_in_id,
                        newOperation.Operation_type 
                    });
                await this.ChangeBalance(newOperation.Account_in_id, newOperation.Amount);
                return newOperation;
            }
        }
        public async Task<Operation> Payment(Operation newOperation)
        {

            newOperation.Date = DateTime.Now;
            Requisite newRequisite = newOperation.Requisite;
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                int requisiteId = await EqualRequisites(newRequisite);
                if (requisiteId < 1)
                {
                    requisiteId = await connection.QueryFirstOrDefaultAsync<int>("INSERT INTO requisites (payment_name, target_name, target_email) values (@payment_name, @target_name, @target_email) Returning (id)",
                        new
                        {
                            newRequisite.Payment_name,
                            newRequisite.Target_name,
                            newRequisite.Target_email
                        });
                }

                await connection.QueryAsync("INSERT INTO OPERATIONS (amount, date, account_in_id ,account_out_id, operation_type, requisite_id ) values (@amount, @date, @account_in_id, @account_out_id, 'Платёж', @requisiteId)",
                    new { 
                        newOperation.Amount, 
                        newOperation.Date, 
                        newOperation.Account_in_id, 
                        newOperation.Account_out_id,
                        requisiteId 
                    });
                await this.ChangeBalance(newOperation.Account_in_id, newOperation.Amount);
                await this.ChangeBalance(newOperation.Account_in_id, -newOperation.Amount);
           
        
               
                return newOperation;

            }
        }
        public async Task<decimal> ChangeBalance(long accountId, decimal amount)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))

            {
                await connection.QueryAsync("UPDATE Accounts SET  balance = balance + @amount WHERE id = @accountId", 
                    new { 
                        amount, 
                        accountId });
                return amount;
            }
        }

        public async Task<List<Operation>> GetAllOperationByAccountId(long accountId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {

                var res = await connection.QueryMultipleAsync("SELECT * FROM Operations WHERE account_out_id = @accountId OR account_in_id = @accountId", 
                    new {  accountId });
                var operations = res.Read<Operation>().ToList();
                return operations;
            }
        }

        public async Task<long> CloseAccount(long accountId, int userId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.QueryAsync("UPDATE Accounts SET is_close=true WHERE id = @accountId", 
                    new {accountId });
                return accountId;
            }
        }

        public async Task<int> EqualRequisites(Requisite requisite)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                try
                {
                    int id = await connection.QueryFirstOrDefaultAsync<int>("SELECT Id FROM Requisites WHERE payment_name=@payment_name AND target_email=@target_email AND target_name=@target_name",
                        new { 
                            requisite.Payment_name,
                            requisite.Target_email,
                            requisite.Target_name
                        });
                    return id;
                }
                catch
                {
                    return -1;
                }
               
            }
        }

        public async Task<List<Requisite>> GetAllRequisitesByUserId(int id)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                    var res = await connection.QueryMultipleAsync(
                    "SELECT * FROM Requisites WHERE user_id = 13", 
                    new { id });
                    var requisites = res.Read<Requisite>().ToList();
                    return requisites;
            }
        }

        public async Task<Requisite> GetRequisite(int id)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var res = await connection.QueryMultipleAsync("SELECT * FROM Requisites WHERE id=@id ", 
                    new { id });
                var requisite = res.ReadSingle<Requisite>();
                return requisite;
            }
        }

        public async Task<int> CreateRequisite(Requisite newRequisite, int userId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                int requisiteId = await connection.QueryFirstOrDefaultAsync<int>("INSERT INTO requisites (payment_name, target_name, target_email, user_id) values (@payment_name, @target_name, @target_email, @userId) Returning (id)",
                    new {
                         newRequisite.Payment_name,
                         newRequisite.Target_name,
                         newRequisite.Target_email,
                         userId
                    });
                return requisiteId;
            }     
        }
    }
}
