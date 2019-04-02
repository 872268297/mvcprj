using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;

namespace Services
{
    public interface ILiveClassService
    {

        Task<List<LiveClass>> AllList();

        Task<JsonModel> List(string keyword, int page, int rows);

        Task Edit(LiveClass model);

        Task Delete(string idList);

        Task<Dictionary<int, List<LiveClass>>> GetDict();
    }
}