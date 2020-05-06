using mb_back.BusinessLogic;
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
        private readonly AccountRequestHandler _accountRequestHandler;
        public AccountController(IAccountService accountService, IUserService userService, AccountRequestHandler accountRequestHandler)
        {
            _accountService = accountService;
            _userService = userService;
            _accountRequestHandler = accountRequestHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountsByUserId()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("userId").Value);
                var res = await _accountRequestHandler.GetAccountsByUserId(userId);
                return Ok(res);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("userId").Value);
                long id = await _accountRequestHandler.CreateAccount(userId);
                return Ok(id);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(long id)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("userId").Value);
                Account res = await _accountRequestHandler.GetAccountById(id, userId);
                return Ok(res);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(long id)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("userId").Value);
                await _accountRequestHandler.DeleteAccount(id, userId);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("{id}/operations")]
        public async Task<IActionResult> GetOperations(long id)
        {
            try
            {
                var operation = await _accountRequestHandler.GetOperations(id);
                return Ok(operation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("{id}/transfer")]
        public async Task<IActionResult> AddTransfer([FromBody] Operation newOperation)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("userId").Value);
                var operation = await _accountRequestHandler.AddTransfer(newOperation, userId);
                return Ok(operation);
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
                var operation = await _accountRequestHandler.AddReplenishment(newOperation);
                return Ok(operation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Не очень хорошо понял что из себя должен представлять платёж, пусть пока будет в таком состоянии
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
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
       

    }
}
