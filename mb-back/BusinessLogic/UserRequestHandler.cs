using mb_back.Models;
using mb_back.Services;
using mb_back.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mb_back.BusinessLogic
{
    public class UserRequestHandler
    {
        private readonly IUserService _userServices;
        
        public UserRequestHandler(IUserService userServices)
        {
            _userServices = userServices;
        }

        public Task<User> Handle(int id)
        {
            return _userServices.GetById(id);
        }
    }
}
