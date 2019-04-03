using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Http;
using mvc.Entities;

namespace Services
{
    public interface IUserService
    {

        Task<string> Register(User user);

        Task<List<User>> GetList();

        Task<string> Login(string username, string password);

        bool LogOut();

        Task<UserAsset> GetUserAsset(int userId);

        Task UpdateUserAsset(UserAsset asset);
    }
}