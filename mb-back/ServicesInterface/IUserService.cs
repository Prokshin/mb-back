using mb_back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mb_back.ServicesInterface
{
    public interface IUserService
    {
        Task<UserInfo> GetById(int id);
        Task<List<User>> GetAllUsers();
        Task<UserRegistration> CreateUser(UserRegistration newUser);
        Task<UserInfo> UpdateUser(UserInfo updatedUser, int userId);
        Task<int> GetIdByEmail(string email);
        Task<int> DeleteUser(int userId);

        Task<string> UpdateUserImg(string fileName, int userId);
    }
}
