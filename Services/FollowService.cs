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
    public class FollowService : IFollowService
    {
        private readonly MyDbContext _dbContext;

        public FollowService(MyDbContext myDbContext)
        {
            _dbContext = myDbContext;
        }

        public async Task<JsonModel> AddFollowByAnchorId(int userid, int anchorId)
        {
            try
            {
                var anchor = await _dbContext.Anchors.FirstOrDefaultAsync(t => t.Id == anchorId);
                if (anchor == null)
                {
                    return new JsonModel(false, "主播不存在", null);
                }

                UserFollow u = new UserFollow();
                u.UserId = userid;
                u.AnchorId = anchorId;

                await _dbContext.UserFollows.AddAsync(u);

                anchor.Follower += 1;

                _dbContext.Anchors.Update(anchor);

                await _dbContext.SaveChangesAsync();

                return new JsonModel(true, "关注成功", null);

            }
            catch
            {
                return new JsonModel(false, "已经关注", null);
            }

        }

        public async Task<JsonModel> AddFollowByRoomId(int userid, int roomId)
        {
            try
            {
                var anchor = await (from c in _dbContext.BroadcastRooms
                                    join a in _dbContext.Anchors on c.AnchorId equals a.Id
                                    where c.Id == roomId
                                    select a
                              ).FirstOrDefaultAsync();

                if (anchor == null)
                {
                    return new JsonModel(false, "主播不存在", null);
                }

                UserFollow u = new UserFollow();
                u.UserId = userid;
                u.AnchorId = anchor.Id;

                await _dbContext.UserFollows.AddAsync(u);

                anchor.Follower += 1;

                _dbContext.Anchors.Update(anchor);

                await _dbContext.SaveChangesAsync();

                return new JsonModel(true, "关注成功", null);

            }
            catch
            {
                return new JsonModel(false, "已经关注", null);
            }
        }

        public async Task<JsonModel> CancelFollowByAnchorId(int userid, int anchorId)
        {
            try
            {
                var anchor = await _dbContext.Anchors.FirstOrDefaultAsync(t => t.Id == anchorId);
                if (anchor == null)
                {
                    return new JsonModel(false, "主播不存在", null);
                }

                UserFollow u = await _dbContext.UserFollows.FirstOrDefaultAsync(t => t.UserId == userid && t.AnchorId == anchorId);
                if (u == null)
                {
                    return new JsonModel(false, "没有关注", null);
                }
                _dbContext.Entry(u).State = EntityState.Deleted;


                anchor.Follower -= 1;

                _dbContext.Anchors.Update(anchor);

                await _dbContext.SaveChangesAsync();

                return new JsonModel(true, "取关成功", null);

            }
            catch
            {
                return new JsonModel(false, "失败", null);
            }
        }

        public async Task<JsonModel> CancelFollowByRoomId(int userid, int roomId)
        {
            try
            {
                var anchor = await (from c in _dbContext.BroadcastRooms
                                    join a in _dbContext.Anchors on c.AnchorId equals a.Id
                                    where c.Id == roomId
                                    select a
                             ).FirstOrDefaultAsync();
                if (anchor == null)
                {
                    return new JsonModel(false, "主播不存在", null);
                }

                UserFollow u = await _dbContext.UserFollows.FirstOrDefaultAsync(t => t.UserId == userid && t.AnchorId == anchor.Id);

                if (u == null)
                {
                    return new JsonModel(false, "没有关注", null);
                }
                _dbContext.Entry(u).State = EntityState.Deleted;


                anchor.Follower -= 1;

                _dbContext.Anchors.Update(anchor);

                await _dbContext.SaveChangesAsync();

                return new JsonModel(true, "取关成功", null);

            }
            catch
            {
                return new JsonModel(false, "失败", null);
            }
        }

        public async Task<List<BroadcastRoomDTO>> GetFollowedRoom(int userid)
        {
            var query = from f in _dbContext.UserFollows
                        join r in _dbContext.BroadcastRooms on f.AnchorId equals r.AnchorId
                        join u in _dbContext.UserAssets on r.UserId equals u.UserId
                        join lc in _dbContext.LiveClasses on r.ClassId equals lc.Id
                        join a in _dbContext.Anchors on r.AnchorId equals a.Id
                        where f.UserId == userid
                        select new BroadcastRoomDTO()
                        {
                            Room = new BroadcastRoom()
                            {
                                AnchorId = r.AnchorId,
                                ClassId = r.ClassId,
                                Id = r.Id,
                                CoverUrl = r.CoverUrl,
                                IsBan = r.IsBan,
                                IsLiving = r.IsLiving,
                                LastLiveTime = r.LastLiveTime,
                                Name = r.Name,
                                Notice = r.Notice,
                                RoomNum = r.RoomNum,
                                StreamChannel = r.StreamChannel,
                                UserId = r.UserId,
                                Viewer = r.Viewer,
                                IsCustomCover = r.IsCustomCover
                            },
                            UserAsset = u,
                            LiveClass = lc,
                            Follower = a.Follower
                        };
            return await query.ToListAsync();
        }

        public async Task<bool> IsFollowed(int userid, int roomid)
        {
            return await (from c in _dbContext.UserFollows
                          join r in _dbContext.BroadcastRooms on c.AnchorId equals r.AnchorId
                          where c.UserId == userid && r.Id == roomid
                          select 1).AnyAsync();
        }
    }
}
