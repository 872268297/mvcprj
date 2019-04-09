using Entities;
using mvc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.Models
{
    public class BroadcastRoomDTO
    {
        public BroadcastRoom Room { get; set; }
        public UserAsset UserAsset { get; set; }
        public LiveClass LiveClass { get; set; }
    }
}
