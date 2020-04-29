using mb_back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mb_back.ServicesInterface
{
    public interface IUserServices
    {
        Task<User> GetById(int id);
    }
}
