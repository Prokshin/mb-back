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

namespace mb_back.Controllers
{

    public class AuthController : ControllerBase
    {
        private List<User> users = new List<User>
        {
            new User("a@a.a", "12345"),
            new User("b@b.b", "12345")
        };

        private List<User> GetAllusers()
        {
            using (var connection = new NpgsqlConnection($"server=localhost;database=mb;userid=postgres;password=password;Pooling=false"))
            {
                connection.Open();

                var result = connection.Query<User>("SELECT Id, email, password FROM Users").ToList();
                return result;
            }
        }

        [HttpPost("api/auth")]
        public IActionResult Token([FromBody] User user)
        {
            var identity = GetIdentity(user.Email, user.Password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
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
                email = identity.Name,
            };

            return Ok(response);
        }
        private ClaimsIdentity GetIdentity(string email, string password)
        {
            User user = GetAllusers().FirstOrDefault(x => x.Email == email && x.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    //new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            Console.WriteLine("не найден юзер");
            return null;
        }

      
    }
}
