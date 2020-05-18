using mb_back.Models;
using mb_back.Services;
using mb_back.ServicesInterface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<UserInfo> GetUserById(int id)
        {
            return await _userService.GetById(id);
        }
        public async Task<UserRegistration> CreateUser(UserRegistration newUser)
        {
            return await _userService.CreateUser(newUser);
        }
        public async Task<UserInfo> UpdateUser(UserInfo updatedUser, int userId)
        {
            return await _userService.UpdateUser(updatedUser, userId);
        }
        public async Task<int> DeleteUser(int userId)
        {
            return await _userService.DeleteUser(userId);
        }

        public async Task<string> UpdateUserImage(IFormFile uploadedFile, int userId)
        {

            if (uploadedFile.Length > 2097152) throw new Exception("file size exceeded");
            string extension =  System.IO.Path.GetExtension(uploadedFile.FileName).Substring(1);
            if (extension == "jpg" || extension == "png")
            {
                string filePath = DateTime.Now.Ticks + uploadedFile.FileName;

                using (var stream = System.IO.File.Create("./wwwroot/images/" + filePath))
                {
                    await uploadedFile.CopyToAsync(stream);
                }

                string imageName = await _userService.UpdateUserImg("http://localhost:51870/images/" + filePath, userId);

                return imageName;
            }
            else throw new Exception("invalid file extension ");
        }
    }

}
