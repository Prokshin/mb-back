using mb_back.Models;
using Npgsql;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mb_back.ServicesInterface;

namespace mb_back.Services
{
    public class UserService : IUserService
    {
        private const string ConnectionString =
            "server=localhost;database=mb;userid=postgres;password=password;Pooling=false";

        public async Task<UserInfo> GetById(int id)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var user =  await connection.QuerySingleAsync<UserInfo>(
                    "SELECT email, name, img FROM Users WHERE Id=@id",
                    new { id });
                return user;
            }
        }
        public async Task<List<User>> GetAllUsers()
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var result = await connection.QueryAsync<User>("SELECT Id, email, password FROM Users");
                return result.ToList();
            }
        }
        public async Task<UserRegistration> CreateUser(UserRegistration newUser)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
                newUser.Password = hashedPassword;
                await connection.ExecuteAsync("INSERT INTO Users (Name, Email, Password) VALUES ( @name, @email, @password)", 
                    new { 
                        newUser.Name,
                        newUser.Email, 
                        newUser.Password 
                    });

                return newUser;
            }
        }
        public async Task<UserInfo> UpdateUser(UserInfo updatedUser, int userId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {

                await connection.ExecuteAsync("UPDATE Users SET  name = @name, email=@email, img=@img WHERE id=@userId ", 
                    new { 
                        updatedUser.Name, 
                        updatedUser.Email, 
                        updatedUser.Img, 
                        userId
                    });
                return updatedUser;
            }
        }

        public async Task<string> UpdateUserImg(string fileName, int userId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.ExecuteAsync("UPDATE Users SET img=@fileName WHERE id=@userId",
                   new
                   {
                       fileName,
                       userId
                   });
                return fileName;
            }
        }

        public async Task<int> GetIdByEmail(string email)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {

                var res = await connection.QueryFirstOrDefaultAsync<int>("SELECT id FROM Users WHERE email=@email", 
                    new { email });
                return res;
            }
        }
        public async Task<int> DeleteUser(int Id)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {  
                await connection.ExecuteAsync("UPDATE Users SET is_deleted=true WHERE id=@Id ", 
                    new { Id });
                return Id;
            }
        }
    }
}
