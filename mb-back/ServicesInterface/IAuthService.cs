using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
namespace mb_back.ServicesInterface
{
    interface IAuthService
    {
        ClaimsIdentity GetIdentity(string email, string password);
    }
}
