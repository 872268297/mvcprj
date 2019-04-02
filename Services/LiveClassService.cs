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
using mvc.Models;

namespace Services
{
    public class LiveClassService : ILiveClassService
    {
        private readonly MyDbContext _dbcontext;
        private readonly IMemoryCache _memoryCache;
        public LiveClassService(MyDbContext dbContext, IMemoryCache memoryCache)
        {
            _dbcontext = dbContext;
            _memoryCache = memoryCache;
        }

        public async Task<List<LiveClass>> AllList()
        {
            if (!_memoryCache.TryGetValue("live_class_list", out List<LiveClass> list))
            {
                list = await (from c in _dbcontext.LiveClasses orderby c.Order select c).ToListAsync();
                _memoryCache.Set("live_class_list", list);
            }
            return list;
        }

        private async Task ReflashCache()
        {
            List<LiveClass> list = await (from cc in _dbcontext.LiveClasses orderby cc.Order select cc).ToListAsync();
            Dictionary<int, List<LiveClass>> dict = new Dictionary<int, List<LiveClass>>();
            foreach (var item in list)
            {
                if (!dict.ContainsKey(item.ParentId)) dict.Add(item.ParentId, new List<LiveClass>());
                dict[item.ParentId].Add(item);
            }
            _memoryCache.Set("live_class_list", list);
            _memoryCache.Set("live_class_dict", dict);
        }

        public async Task Delete(string idList)
        {
            string[] strs = idList.Split(',');
            List<int> lst = new List<int>();
            foreach (string item in strs)
            {
                if (int.TryParse(item, out int i))
                {
                    lst.Add(i);
                }
            }
            var query = from c in _dbcontext.LiveClasses where lst.Contains(c.Id) select c;

            _dbcontext.LiveClasses.RemoveRange(query);

            await _dbcontext.SaveChangesAsync();

            await ReflashCache();
        }

        public async Task Edit(LiveClass model)
        {
            if (model.Id == 0)
            {
                await _dbcontext.LiveClasses.AddAsync(model);
                await _dbcontext.SaveChangesAsync();
            }
            else
            {
                int id = model.Id;
                LiveClass c = await _dbcontext.LiveClasses.FirstOrDefaultAsync(t => t.Id == id);
                if (c != null)
                {
                    c.Name = model.Name;
                    c.Order = model.Order;
                    c.ParentId = model.ParentId;
                    _dbcontext.LiveClasses.Update(c);
                    await _dbcontext.SaveChangesAsync();
                }
            }
            await ReflashCache();
        }

        public async Task<JsonModel> List(string keyword, int page, int rows)
        {
            var query = from c in _dbcontext.LiveClasses select c;
            if (keyword != "") query = query.Where(t => t.Name.Contains(keyword));
            int count = await (from c in _dbcontext.LiveClasses select 1).CountAsync();
            List<LiveClass> list = await query.OrderBy(t => t.Order).Skip((page - 1) * rows).Take(rows).ToListAsync();
            return new JsonModel(true, "成功", list, count);
        }

        public async Task<Dictionary<int, List<LiveClass>>> GetDict()
        {
            if (!_memoryCache.TryGetValue("live_class_dict", out Dictionary<int, List<LiveClass>> dict))
            {
                var list = await (from c in _dbcontext.LiveClasses orderby c.Order select c).ToListAsync();

                dict = new Dictionary<int, List<LiveClass>>();

                foreach (var item in list)
                {
                    if (!dict.ContainsKey(item.ParentId)) dict.Add(item.ParentId, new List<LiveClass>());
                    dict[item.ParentId].Add(item);
                }

                _memoryCache.Set("live_class_list", list);
                _memoryCache.Set("live_class_dict", dict);
            }
            return dict;
        }
    }
}