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
using Microsoft.AspNetCore.Http;
using System.IO;

namespace mb_back.Controllers
{
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserRequestHandler _userRequestHandler;

        public UserController( UserRequestHandler userRequestHandler )
        {
            _userRequestHandler = userRequestHandler;
        }

        [Authorize]
        [HttpGet("current")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var user = await _userRequestHandler.GetUserById(int.Parse(User.FindFirst("userId").Value));
                return Ok(user);
            }
            catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User newUser)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Invalid data");
                await _userRequestHandler.CreateUser(newUser);
                return StatusCode(201);
            }
            catch(Exception e)
            {
                return NotFound(e.Message);
            }
           
        }
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User updatedUser)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Invalid data");
                await _userRequestHandler.UpdateUser(updatedUser, int.Parse(User.FindFirst("userId").Value));
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

 
        [HttpPatch("current/img")]
        public async Task<IActionResult> UpdateUserImage(IFormFile uploadedFile)
        {
            try
            {              
                int userId = int.Parse(User.FindFirst("userId").Value);
                string url = await _userRequestHandler.UpdateUserImage(uploadedFile, userId);
                return Ok(url);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpDelete("current")]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                await _userRequestHandler.DeleteUser(int.Parse(User.FindFirst("userId").Value));
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }   

 
}
