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
    public class UserServices : IUserServices
    {
        private const string ConnectionString =
            "server=localhost;database=mb;userid=postgres;password=password;Pooling=false";

        public async Task<User> GetById(int id)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                return await connection.QuerySingleAsync<User>(
                    "SELECT * FROM Users WHERE Id = @id", new {id });
            }
        }
    }
}
