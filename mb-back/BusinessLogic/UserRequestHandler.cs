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
        private readonly IUserService _userService;
        
        public UserRequestHandler(IUserService userServices)
        {
            _userService = userServices;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _userService.GetById(id);
        }
        public async Task<User> CreateUser(User newUser)
        {
            return await _userService.CreateUser(newUser);
        }
        public async Task<User> UpdateUser(User updatedUser, int userId)
        {
            updatedUser.Id = userId;
            return await _userService.UpdateUser(updatedUser);
        }
        public async Task<int> DeleteUser(int userId)
        {
            return await _userService.DeleteUser(userId);
        }
    }

}
