using Dapper;
using mb_back.Models;
using mb_back.ServicesInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace mb_back.Services
{
    public class AuthService
    {


        private readonly IUserService _userService;

        public AuthService(IUserService userService)
        {
            _userService = userService;
        }

        public ClaimsIdentity GetIdentity(string email, string password)
        {
            User user = _userService.GetAllUsers().Result.FirstOrDefault(x => x.Email == email && BCrypt.Net.BCrypt.Verify(password, x.Password));
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                    //new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                    new Claim("userId", user.Id.ToString())
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
