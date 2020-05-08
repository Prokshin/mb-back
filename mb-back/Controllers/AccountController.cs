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

        private readonly AccountRequestHandler _accountRequestHandler;
        public AccountController(AccountRequestHandler accountRequestHandler)
        {

            _accountRequestHandler = accountRequestHandler;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAccountsByUserId()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("userId").Value);
                var res = await _accountRequestHandler.GetAccountsByUserId(userId);
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAccount()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("userId").Value);
                long id = await _accountRequestHandler.CreateAccount(userId);
                return Ok(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(long id)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("userId").Value);
                Account res = await _accountRequestHandler.GetAccountById(id, userId);
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        [HttpPost("{id}/transfer")]
        public async Task<IActionResult> AddTransfer([FromBody] Operation newOperation)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("userId").Value);
                var operation = await _accountRequestHandler.AddTransfer(newOperation, userId);
                return Ok(operation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpPost("{id}/replenishment")]
        public async Task<IActionResult> AddReplenishment([FromBody] Operation newOperation, long id)
        {
            try
            {
                newOperation.Account_in_id = id;
                var operation = await _accountRequestHandler.AddReplenishment(newOperation);
                return Ok(operation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpPost("{id}/payment")]
        public async Task<IActionResult> AddPayment([FromBody] Payment newPayment)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("userId").Value);
                var operation = await _accountRequestHandler.AddPayment(newPayment, userId);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpGet("templates")]
        public async Task<IActionResult> GetTemplates()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("userId").Value);
                var templates = await _accountRequestHandler.GetTemplates(userId);
                return Ok(templates);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpPost("templates")]
        public async Task<IActionResult> CreateTemplate([FromBody] Requisite newRequisite)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("userId").Value);
                var templateId = await _accountRequestHandler.CreateTemplate(newRequisite,userId);
                return Ok(templateId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpGet("templates/{id}")]
        public async Task<IActionResult> GetTemplate(int id)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("userId").Value);
                var template = await _accountRequestHandler.GetTemplate(id, userId);
                return Ok(template);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
