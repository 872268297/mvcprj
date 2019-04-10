using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using mvc.Entities;
using mvc.Models;
using Services;

namespace mvc.Services
{
    public class ServerService : IServerService
    {
        public string GetRtmpAddress()
        {
            return "127.0.0.1:1935/live/";
        }

        public static string GetRtmpAddr()
        {
            return "127.0.0.1:1935/live/";
        }
    }
}
