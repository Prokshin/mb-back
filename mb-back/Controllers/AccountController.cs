using mb_back.Models;
using mb_back.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mb_back.Controllers
{
    [Route("api/accounts")]
    public class AccountController : Microsoft.AspNetCore.Mvc.ControllerBase
    {

        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        public AccountController(IAccountService accountService, IUserService userService)
        {
            _accountService = accountService;
            _userService = userService;
        }

        [HttpGet]
        public Task<List<Account>> GetAccountsByUserId()
        {

            Task<List<Account>> res = _accountService.GetAllByUserId(int.Parse(User.FindFirst("userId").Value));
            Console.Write(res);
            return res;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount()
        {
            try
            {
                long id = await _accountService.CreateAccount(int.Parse(User.FindFirst("userId").Value));


                return Ok(id);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public Task<Account> GetAccountById(long id)
        {
            Console.WriteLine(id);
            Task<Account> res = _accountService.GetAccount(id, int.Parse(User.FindFirst("userId").Value));
            return res;
        }
        [HttpGet("{id}/operations")]
        public async Task<IActionResult> GetOperations(long id)
        {
            try
            {
                var operation = await _accountService.GetAllOperationByAccountId(id);
                return Ok(operation);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("{id}/transfer")]
        public async Task<IActionResult> AddTransfer([FromBody] Operation newOperation)
        {
            try
            {
                Account outAccount = await _accountService.GetAccount(newOperation.Account_out_id, int.Parse(User.FindFirst("userId").Value));
            
            
                if (outAccount.Balance < newOperation.Amount)
                {
                    throw new Exception("Недостаточно средств");
                }
                var operation = await _accountService.Transfer(newOperation);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("{id}/replenishment")]
        public async Task<IActionResult> AddReplenishment([FromBody] Operation newOperation)
        {
            try
            {
                var operation = await _accountService.Replenishment(newOperation);
                return Ok();
            }
            catch 
            {
                return BadRequest();
            }
        }
        [HttpPost("{id}/payment")]
        public async Task<IActionResult> AddPayment([FromBody] Payment newPayment)
        {
            try
            {
                Account outAccount = await _accountService.GetAccount(newPayment.Account_Out_Id, int.Parse(User.FindFirst("userId").Value));

                if (outAccount.Balance < newPayment.Amount) 
                {
                    throw new Exception("Недостаточно средств");
                }
                int id = await _userService.GetIdByEmail(newPayment.Requisite.Target_email);
                long AccounInId = await _accountService.GetAccountIdByUserID(id);
                Operation newOperation = new Operation("Платёж", newPayment.Amount, AccounInId, newPayment.Account_Out_Id, newPayment.Requisite, newPayment.Purpose);
                var operation = await _accountService.Payment(newOperation);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(long id)
        {
            try
            {
                await _accountService.CloseAccount(id, int.Parse(User.FindFirst("userId").Value));
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
