using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Http;

namespace Services
{
    public interface IUserService
    {

        Task<string> Register(User user);

        Task<List<User>> GetList();

        Task<string> Login(string username, string password);

        bool LogOut();
    }
}