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

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;

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

    }
}
