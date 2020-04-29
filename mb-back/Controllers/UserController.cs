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

namespace mb_back.Controllers
{
    [Route("api/users")]
    public class UserController : Microsoft.AspNetCore.Mvc.ControllerBase
    {

        private readonly UserRequestHandler _userRequestHandler;

        public UserController(UserRequestHandler userRequestHandler)
        {
            _userRequestHandler = userRequestHandler;
        }

        [HttpGet("current")]
        public Task<User> GetUser()
        {
            return _userRequestHandler.Handle(int.Parse(User.Identity.Name));
        }
    }
 
}
