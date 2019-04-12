using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Http;
using mvc.Entities;
using mvc.Models;

namespace Services
{
    public interface IFollowService
    {

        Task<JsonModel> AddFollowByAnchorId(int userid, int anchorId);

        Task<JsonModel> AddFollowByRoomId(int userid, int roomId);

        Task<JsonModel> CancelFollowByAnchorId(int userid, int anchorId);

        Task<JsonModel> CancelFollowByRoomId(int userid, int roomId);

        Task<List<BroadcastRoomDTO>> GetFollowedRoom(int userid);

        Task<bool> IsFollowed(int userid, int roomid);

    }
}