using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mvc.Entities;
using mvc.Models;

namespace Services
{
    public interface IServerService
    {
        string GetRtmpAddress();
    }
}