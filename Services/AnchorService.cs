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
            await _dbcontext.SaveChangesAsync();
            int anchorId = anchor.Id;
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
            var room = await query.FirstOrDefaultAsync();

            return room;
        }

        public async Task<BroadcastRoomDTO> GetRoomByRoomId(int roomId)
        {
            var query = from r in _dbcontext.BroadcastRooms
                        join u in _dbcontext.UserAssets on r.UserId equals u.UserId
                        join lc in _dbcontext.LiveClasses on r.ClassId equals lc.Id
                        where r.Id == roomId
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
                            LiveClass = lc
                        };
            var room = await query.FirstOrDefaultAsync();

            return room;
        }

        public async Task<BroadcastRoom> GetRoomByUserId(int userid)
        {
            var query = from r in _dbcontext.BroadcastRooms
                        where r.UserId == userid
                        select r;
            var room = await query.FirstOrDefaultAsync();

            return room;
        }

        public async Task<List<BroadcastRoomDTO>> GetRoomList(int classid = 0)
        {

            var dict = await _liveClassService.GetDict();
            var classes = await _liveClassService.AllList();
            List<int> classList = new List<int>();
            Fun(classid, dict, classList);
            IQueryable<BroadcastRoomDTO> query = from r in _dbcontext.BroadcastRooms
                                                 join u in _dbcontext.UserAssets on r.UserId equals u.UserId
                                                 join lc in classes on r.ClassId equals lc.Id
                                                 where classList.Contains(r.ClassId)
                                                 orderby r.LastLiveTime descending
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
                                                     LiveClass = lc
                                                 };
            var list = await query.ToListAsync();

            return list;
        }

        private void Fun(int root, Dictionary<int, List<LiveClass>> dict, List<int> classList)
        {
            classList.Add(root);
            if (dict.ContainsKey(root))
            {
                var list = dict[root];
                foreach (var item in list)
                {
                    Fun(item.Id, dict, classList);
                }
            }
        }

        public async Task<List<BroadcastRoomDTO>> GetRoomListLiving(int classid = 0)
        {
            var dict = await _liveClassService.GetDict();
            var classes = await _liveClassService.AllList();
            List<int> classList = new List<int>();
            Fun(classid, dict, classList);
            IQueryable<BroadcastRoomDTO> query = from r in _dbcontext.BroadcastRooms
                                                 join u in _dbcontext.UserAssets on r.UserId equals u.UserId
                                                 join lc in classes on r.ClassId equals lc.Id
                                                 where r.IsLiving == true && classList.Contains(r.ClassId)
                                                 orderby r.LastLiveTime descending
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
                                                     LiveClass = lc
                                                 };
            var list = await query.ToListAsync();

            return list;
        }

        public async Task<bool> IsAnchor(int userid)
        {
            return await _dbcontext.Anchors.AnyAsync(t => t.UserId == userid);
        }

        public async Task<JsonModel> SetRoomInfo(int userid, BroadcastRoom room)
        {
            try
            {
                BroadcastRoom r = await _dbcontext.BroadcastRooms.Where(t => t.UserId == userid).FirstOrDefaultAsync();
                if (r == null)
                {
                    return new JsonModel(false, "修改失败", null);
                }
                r.Name = room.Name;
                r.Notice = room.Notice;
                r.ClassId = room.ClassId;
                r.CoverUrl = room.CoverUrl;
                r.IsCustomCover = room.IsCustomCover;
                _dbcontext.BroadcastRooms.Update(r);
                await _dbcontext.SaveChangesAsync();

                return new JsonModel(true, "修改成功");
            }
            catch (Exception e)
            {
                return new JsonModel(false, "修改失败", e.Message);
            }
        }

        private string GenarateStreamCode(int userid)
        {
            Random random = new Random();
            string c = userid + random.Next() + "932qj@dawrr" + DateTime.Now.Ticks.ToString();
            string result = Md5Util.Encode(Md5Util.Encode(c) + "th8jusde21@HRFIh");
            return result;
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
                if (string.IsNullOrEmpty(r.StreamCode))
                {
                    r.StreamCode = this.GenarateStreamCode(userid);
                }
                r.LastLiveTime = DateTime.Now;
                _dbcontext.BroadcastRooms.Update(r);
                await _dbcontext.SaveChangesAsync();

                return new JsonModel(true, "成功", new Dictionary<string, string>() {
                    { "streamCode", r.StreamCode },
                    { "streamChannel", r.StreamChannel }
                });//返回推流码
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

        public async Task<JsonModel> UpdateStreamCode(int userid)
        {
            BroadcastRoom r = await _dbcontext.BroadcastRooms.Where(t => t.UserId == userid).FirstOrDefaultAsync();
            if (r == null)
            {
                return new JsonModel(false, "失败", "找不到直播间");
            }
            r.StreamCode = this.GenarateStreamCode(userid);
            r.LastLiveTime = DateTime.Now;
            _dbcontext.BroadcastRooms.Update(r);
            await _dbcontext.SaveChangesAsync();

            return new JsonModel(true, "成功", r.StreamCode);
        }

        public async Task<bool> CheckStreamCodeValid(string channel, string code)
        {
            var query = from r in _dbcontext.BroadcastRooms
                        where code == r.StreamCode && r.StreamChannel == channel
                        select 1
                        ;
            return await query.AnyAsync();
        }

        public async Task<BroadcastRoomDTO> GetRoomByRoomNum(int roomnum)
        {
            var query = from r in _dbcontext.BroadcastRooms
                        join u in _dbcontext.UserAssets on r.UserId equals u.UserId
                        join lc in _dbcontext.LiveClasses on r.ClassId equals lc.Id
                        where r.RoomNum == roomnum
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
                                Viewer = r.Viewer
                            },
                            UserAsset = u,
                            LiveClass = lc
                        };
            var room = await query.FirstOrDefaultAsync();

            return room;
        }

        public async Task<List<BroadcastRoomDTO>> GetRoomList(string keyword)
        {
            IQueryable<BroadcastRoomDTO> query = from r in _dbcontext.BroadcastRooms
                                                 join u in _dbcontext.UserAssets on r.UserId equals u.UserId
                                                 join lc in _dbcontext.LiveClasses on r.ClassId equals lc.Id
                                                 
                                                 orderby r.LastLiveTime descending
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
                                                     LiveClass = lc
                                                 };
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(t => t.Room.Name.Contains(keyword) || t.Room.RoomNum.ToString().Contains(keyword) || t.UserAsset.NickName.Contains(keyword));
            }
            var list = await query.ToListAsync();

            return list;
        }
    }
}