using mb_back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mb_back.ServicesInterface
{
    public interface IAccountService
    {
        Task<List<Account>> GetAllByUserId(int id);
        Task<long> CreateAccount(int userId);
        Task<Account> GetAccount(long Accountid, int userId);
        Task<Operation> Transfer(Operation newOperation);
        Task<Operation> Replenishment(Operation newOperation);
        Task<Operation> Payment(Operation newOperation);
        Task<List<Operation>> GetAllOperationByAccountId(long accountId);
        Task<long> GetAccountIdByUserID(int userId);
        Task<long> CloseAccount(long accountId, int userId);
        Task<int> EqualRequisites(Requisite requisite);
        Task<List<Requisite>> GetAllRequisitesByUserId(int id);
        Task<Requisite> GetRequisite(int id);
        Task<int> CreateRequisite(Requisite newRequisite, int userId);
    }
}
