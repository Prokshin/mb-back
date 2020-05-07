using mb_back.Models;
using mb_back.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mb_back.BusinessLogic
{
    public class AccountRequestHandler
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        public AccountRequestHandler(IAccountService accountService, IUserService userService)
        {
            _accountService = accountService;
            _userService = userService;
        }

        public async Task<long> CreateAccount(int userId)
        {
            return await _accountService.CreateAccount(userId);
        }
        public async Task<List<Account>> GetAccountsByUserId(int userId)
        {
            List<Account> res = await _accountService.GetAllByUserId(userId);
            return res;
        }
        public async Task<Account> GetAccountById(long accountId, int userId)
        {
            Account res = await _accountService.GetAccount(accountId, userId);
            return res;
        }

        public async Task<long> DeleteAccount(long accountId, int userId)
        {
            return await _accountService.CloseAccount(accountId, userId);
        }

        public async Task<List<Operation>> GetOperations(long id)
        {
                List<Operation> operation = await _accountService.GetAllOperationByAccountId(id);
                return operation;
        }
        public async Task<Operation> AddTransfer( Operation newOperation, int userId)
        {
                Account outAccount = await _accountService.GetAccount(newOperation.Account_out_id, userId);
                if (outAccount.Balance < newOperation.Amount)
                {
                    throw new Exception("not enough money in the account");
                }
                var operation = await _accountService.Transfer(newOperation);
                return operation;
        }
        public async Task<Operation> AddReplenishment(Operation newOperation)
        {
                var operation = await _accountService.Replenishment(newOperation);
                return operation;
        }
        public async Task<Operation> AddPayment(Payment newPayment, int userOutId)
        {
                Account outAccount = await _accountService.GetAccount(newPayment.Account_Out_Id, userOutId);
                if (outAccount.Balance < newPayment.Amount)
                {
                    throw new Exception("not enough money in the account");
                }
                int userInId = await _userService.GetIdByEmail(newPayment.Requisite.Target_email);
                  long AccounInId = await _accountService.GetAccountIdByUserID(userInId);
                Operation newOperation = new Operation("Платёж", newPayment.Amount, AccounInId, newPayment.Account_Out_Id, newPayment.Requisite, newPayment.Purpose);
                var operation = await _accountService.Payment(newOperation);
                return operation;         
        }
        public async Task<List<Requisite>> GetTemplates(int userId)
        {
            List<Requisite> requisites = await _accountService.GetAllRequisitesByUserId(userId);
            return requisites;
        }
        public async Task<Requisite> GetTemplate(int id, int userId)
        {
            Requisite requisite = await _accountService.GetRequisite(id);
            return requisite;
        }
        public async Task<int> CreateTemplate(Requisite newRequisite, int userId)
        {
            int requisiteId = await _accountService.CreateRequisite(newRequisite, userId);
            return requisiteId;
        }



    }
}
