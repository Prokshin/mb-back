using mb_back.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using mb_back.Services;
using mb_back.BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using mb_back.ServicesInterface;

namespace mb_back.Controllers
{
    [Route("api/users")]
    public class UserController : Microsoft.AspNetCore.Mvc.ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;

        }

        [Authorize]
        [HttpGet("current")]
        public Task<User> GetUser()
        {
            Console.WriteLine();
            return _userService.GetById(int.Parse(User.FindFirst("userId").Value));
        }
        [HttpPost]
        public IActionResult CreateUser([FromBody] User newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
             _userService.CreateUser(newUser);
            return StatusCode(201);
        }
        [HttpPut]
        public IActionResult UpdateUser([FromBody] User updatedUser)
        {
            if(!ModelState.IsValid )
            {
                return BadRequest();
            }
            updatedUser.Id = int.Parse(User.FindFirst("userId").Value);
            var res = _userService.UpdateUser(updatedUser);
            return Ok();
        }
    }   

 
}
