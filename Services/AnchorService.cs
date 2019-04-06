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

namespace Services
{
    public class AnchorService : IAnchorService
    {
        private readonly MyDbContext _dbcontext;
        private readonly IMemoryCache _memoryCache;
        private readonly ILiveClassService _liveClassService;

        public AnchorService(MyDbContext dbContext, IMemoryCache memoryCache, ILiveClassService liveClassService)
        {
            _dbcontext = dbContext;
            _memoryCache = memoryCache;
            _liveClassService = liveClassService;
        }

        public async Task<JsonModel> CreateBroadcastRoom(int userid, BroadcastRoom room)
        {
            bool isExist = await (from a in _dbcontext.Anchors where a.UserId == userid select 1).AnyAsync();
            if (isExist) return new JsonModel(false, "已经有直播间");

            Anchor anchor = new Anchor
            {
                UserId = userid,
                Follower = 0
            };

            var r = await _dbcontext.Anchors.AddAsync(anchor);
            int anchorId = r.Entity.Id;
            int roomNum = 10000;
            //直播间号
            if (await _dbcontext.BroadcastRooms.AnyAsync())
            {
                roomNum = await _dbcontext.BroadcastRooms.MaxAsync(t => t.RoomNum) + 1;
            }

            room.RoomNum = roomNum;
            room.IsBan = false;
            room.IsLiving = false;
            room.AnchorId = anchorId;
            room.UserId = userid;
            room.StreamChannel = roomNum.ToString();

            await _dbcontext.BroadcastRooms.AddAsync(room);

            await _dbcontext.SaveChangesAsync();

            return new JsonModel(true, "创建成功", room);
        }

        public async Task<UserAsset> GetAnchorAssetByRoomId(int roomId)
        {
            var query = from r in _dbcontext.BroadcastRooms
                        join u in _dbcontext.Users on r.UserId equals u.Id
                        join a in _dbcontext.UserAssets on u.Id equals a.UserId
                        where r.Id == roomId
                        select a;
            return await query.FirstOrDefaultAsync();
        }

        public async Task<UserAsset> GetAnchorAssetByUserId(int userid)
        {
            var query = from u in _dbcontext.Users
                        join a in _dbcontext.UserAssets on u.Id equals a.UserId
                        where u.Id == userid
                        select a;
            return await query.FirstOrDefaultAsync();
        }

        public async Task<BroadcastRoom> GetRoomByAnchorId(int anchorId)
        {
            var query = from r in _dbcontext.BroadcastRooms
                        where r.AnchorId == anchorId
                        select r;
            return await query.FirstOrDefaultAsync();
        }

        public async Task<BroadcastRoom> GetRoomByRoomId(int roomId)
        {
            var query = from r in _dbcontext.BroadcastRooms
                        where r.Id == roomId
                        select r;
            return await query.FirstOrDefaultAsync();
        }

        public async Task<BroadcastRoom> GetRoomByUserId(int userid)
        {
            var query = from r in _dbcontext.BroadcastRooms
                        where r.UserId == userid
                        select r;
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<BroadcastRoom>> GetRoomList(int classid = 0)
        {

            var dict = await _liveClassService.GetDict();
            IQueryable<BroadcastRoom> query = from c in _dbcontext.BroadcastRooms where 1 == 0 select c;
            List<IQueryable<BroadcastRoom>> l = new List<IQueryable<BroadcastRoom>>()
            {
                query
            };
            Fun(classid, dict, l, false);

            return await l[0].ToListAsync();
        }

        private void Fun(int root, Dictionary<int, List<LiveClass>> dict, List<IQueryable<BroadcastRoom>> query, bool live)
        {
            IQueryable<BroadcastRoom> cur_List;
            if (live)
            {
                cur_List = _dbcontext.BroadcastRooms.Where(t => t.ClassId == root && t.IsLiving == true);
            }
            else
            {
                cur_List = _dbcontext.BroadcastRooms.Where(t => t.ClassId == root);
            }
            query[0] = query[0].Union(cur_List);
            if (dict.ContainsKey(root))
            {
                var list = dict[root];
                foreach (var item in list)
                {
                    Fun(item.Id, dict, query, live);
                }
            }
        }

        public async Task<List<BroadcastRoom>> GetRoomListLiving(int classid = 0)
        {
            var dict = await _liveClassService.GetDict();
            IQueryable<BroadcastRoom> query = from c in _dbcontext.BroadcastRooms where 1 == 0 select c;
            List<IQueryable<BroadcastRoom>> l = new List<IQueryable<BroadcastRoom>>()
            {
                query
            };
            Fun(classid, dict, l, true);

            return await l[0].ToListAsync();
        }

        public async Task<bool> IsAnchor(int userid)
        {
            return await _dbcontext.Anchors.AnyAsync(t => t.UserId == userid);
        }

        public async Task<JsonModel> SetRoomInfo(BroadcastRoom room)
        {
            try
            {
                BroadcastRoom r = await _dbcontext.BroadcastRooms.Where(t => t.Id == room.Id).FirstOrDefaultAsync();
                if (r == null)
                {
                    return new JsonModel(false, "修改失败", null);
                }
                r.Name = room.Name;
                r.Notice = room.Notice;
                r.ClassId = room.ClassId;
                r.CoverUrl = room.CoverUrl;
                _dbcontext.BroadcastRooms.Update(r);
                await _dbcontext.SaveChangesAsync();

                return new JsonModel(true, "修改成功");
            }
            catch (Exception e)
            {
                return new JsonModel(false, "修改失败", e.Message);
            }
        }

        public async Task<JsonModel> StartBroadcast(int userid)
        {
            try
            {
                BroadcastRoom r = await _dbcontext.BroadcastRooms.Where(t => t.UserId == userid).FirstOrDefaultAsync();
                if (r == null)
                {
                    return new JsonModel(false, "失败", "找不到直播间");
                }
                r.IsLiving = true;

                _dbcontext.BroadcastRooms.Update(r);
                await _dbcontext.SaveChangesAsync();

                return new JsonModel(true, "成功", r.StreamCode);//返回推流码
            }
            catch (Exception e)
            {
                return new JsonModel(false, "失败", e.Message);
            }
        }

        public async Task<JsonModel> StopBroadcast(int userid)
        {
            try
            {
                BroadcastRoom r = await _dbcontext.BroadcastRooms.Where(t => t.UserId == userid).FirstOrDefaultAsync();
                if (r == null)
                {
                    return new JsonModel(false, "失败", "找不到直播间");
                }
                r.IsLiving = false;

                _dbcontext.BroadcastRooms.Update(r);
                await _dbcontext.SaveChangesAsync();

                return new JsonModel(true, "成功", null);//返回推流码
            }
            catch (Exception e)
            {
                return new JsonModel(false, "失败", e.Message);
            }
        }
    }
}