using mb_back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mb_back.ServicesInterface
{
    public interface IUserService
    {
        Task<User> GetById(int id);
        Task<List<User>> GetAllUsers();
        Task<User> CreateUser(User newUser);
        Task<User> UpdateUser(User updatedUser);
        Task<int> GetIdByEmail(string email);
    }
}
