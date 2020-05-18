using Dapper;
using mb_back.Models;
using mb_back.ServicesInterface;
using Microsoft.AspNetCore.Http.Connections;
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
                var accounts =  await connection.QueryAsync<Account>(
                    "SELECT * FROM Accounts WHERE user_id = @id And is_close=false", 
                    new { id });

                var accountList = accounts.ToList();
                return accountList;
            }
        }

        public async Task<long> CreateAccount(int UserId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var id = await connection.QueryFirstOrDefaultAsync<long>("INSERT INTO Accounts (user_id) VALUES (@Userid) Returning (id)", 
                    new { UserId });
                return id;
            }
        }
        public async Task<Account> GetAccount(long accountId, int userId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var account = await connection.QueryFirstOrDefaultAsync<Account>("SELECT * FROM Accounts WHERE id=@accountID AND user_id=@userId AND is_close=false", 
                    new { 
                        accountId, 
                        userId 
                    });
                return account;
            }
        }

        public async Task<long> GetAccountIdByUserID(int userId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var accountId = await connection.QueryFirstOrDefaultAsync<long>("SELECT id FROM Accounts WHERE user_id=@userId and is_close=false", 
                    new { userId });
                return accountId;
            }
        }
        public async Task<Operation> Transfer(Operation newOperation)
        {
            newOperation.Date = DateTime.Now;
            newOperation.Operation_type = "перевод";
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync("INSERT INTO OPERATIONS (amount, date, account_in_id ,account_out_id, operation_type ) values (@amount, @date, @account_in_id, @account_out_id, @operation_type)",
                                  new
                                  {
                                      newOperation.Amount,
                                      newOperation.Date,
                                      newOperation.Account_in_id,
                                      newOperation.Account_out_id,
                                      newOperation.Operation_type
                                  });

                        await this.ChangeBalance(newOperation.Account_in_id, newOperation.Amount, transaction);
                        await this.ChangeBalance(newOperation.Account_out_id, -newOperation.Amount, transaction);
                        transaction.Commit();
                    }
                    catch
                    {                        
                        transaction.Rollback();
                        throw new Exception("Ошибка при переводе");
                    }
                }
                return newOperation;
            }
        }
        public async Task<Operation> Replenishment(Operation newOperation)
        {
            newOperation.Date = DateTime.Now;
            newOperation.Operation_type = "пополнение";
            using (var connection = new NpgsqlConnection(ConnectionString))
            {

                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync("INSERT INTO OPERATIONS (amount, date, account_in_id,operation_type ) values (@amount, @date, @account_in_id, @operation_type)",
                        new
                        {
                            newOperation.Amount,
                            newOperation.Date,
                            newOperation.Account_in_id,
                            newOperation.Operation_type
                        }, transaction: transaction);
                        await this.ChangeBalance(newOperation.Account_in_id, newOperation.Amount, transaction);
                        transaction.Commit();
                        
                    }
                    catch {
                        transaction.Rollback();
                        throw new Exception("Ошибка при пополнении счёта");
                    }
                    return newOperation;
                }

            }
        }
        public async Task<Operation> Payment(Operation newOperation)
        {

            newOperation.Date = DateTime.Now;
            newOperation.Operation_type = "Платёж";
            Requisite newRequisite = newOperation.Requisite;
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
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
                                },transaction);
                        }

                        await connection.ExecuteAsync("INSERT INTO OPERATIONS (amount, date, account_in_id ,account_out_id, operation_type,  requisite_id ) values (@amount, @date, @account_in_id, @account_out_id, @operation_type, @requisiteId)",
                            new { 
                                newOperation.Amount, 
                                newOperation.Date, 
                                newOperation.Account_in_id, 
                                newOperation.Account_out_id,
                                newOperation.Operation_type,
                                newOperation.Purpose,
                                requisiteId
                            },transaction);

                        await this.ChangeBalance(newOperation.Account_in_id, newOperation.Amount,transaction);
                        await this.ChangeBalance(newOperation.Account_out_id, -newOperation.Amount, transaction);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw new Exception("Ошибка при выполнении платежа");
                    }
                    return newOperation;
                }
            }
        }
        public async Task<decimal> ChangeBalance(long accountId, decimal amount, NpgsqlTransaction transaction)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))

            {
                await connection.QueryAsync("UPDATE Accounts SET  balance = balance + @amount WHERE id = @accountId", 
                    new { 
                        amount, 
                        accountId 
                    },transaction: transaction);
                return amount;
            }
        }

        public async Task<List<Operation>> GetAllOperationByAccountId(long accountId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {

                var operations = await connection.QueryAsync<Operation>("SELECT * FROM Operations WHERE account_out_id = @accountId OR account_in_id = @accountId", 
                    new { accountId });
                var operationsList = operations.ToList();
                return operationsList;
            }
        }

        public async Task<long> CloseAccount(long accountId, int userId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.ExecuteAsync("UPDATE Accounts SET is_close=true WHERE id = @accountId", 
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
                    var requisites = await connection.QueryAsync<Requisite>(
                    "SELECT * FROM Requisites WHERE user_id = @id", 
                    new { id });
                    var requisitesList = requisites.ToList();
                    return requisitesList;
            }
        }

        public async Task<Requisite> GetRequisite(int id)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var requisite = await connection.QueryFirstOrDefaultAsync<Requisite>("SELECT * FROM Requisites WHERE id=@id ", 
                    new { id });
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
