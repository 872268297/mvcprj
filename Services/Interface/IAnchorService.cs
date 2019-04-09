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
    public interface IAnchorService
    {
        Task<bool> IsAnchor(int userid);

        Task<JsonModel> CreateBroadcastRoom(int userid, BroadcastRoom room);

        Task<BroadcastRoom> GetRoomByUserId(int userid);

        Task<BroadcastRoom> GetRoomByAnchorId(int anchorId);

        Task<BroadcastRoom> GetRoomByRoomId(int roomId);

        Task<UserAsset> GetAnchorAssetByRoomId(int roomId);

        Task<UserAsset> GetAnchorAssetByUserId(int userid);

        Task<JsonModel> SetRoomInfo(int userid, BroadcastRoom room);

        Task<JsonModel> StartBroadcast(int userid);

        Task<JsonModel> StopBroadcast(int userid);

        Task<List<BroadcastRoom>> GetRoomList(int classid = 0);

        Task<List<BroadcastRoom>> GetRoomListLiving(int classid = 0);

        Task<JsonModel> UpdateStreamCode(int userid);
    }
}