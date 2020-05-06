using Dapper;
using mb_back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using mb_back.Services;
using Microsoft.AspNetCore.Authorization;

namespace mb_back.Controllers
{

    public class AuthController : ControllerBase
    {


        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("api/auth")]
        public IActionResult Token([FromBody] User user)
        {
            var identity = _authService.GetIdentity(user.Email, user.Password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expired_to = jwt.ValidTo
            };

            return Ok(response);
        }
        [Authorize]
        [HttpGet("api/auth/checktoken")]
        public IActionResult CheckToken()
        {
            return Ok("Token is Valid");
        }
      
    }
}
