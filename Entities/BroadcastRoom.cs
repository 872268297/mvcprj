using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvc.Entities
{
    public class BroadcastRoom//直播间
    {
        [Key]
        public int Id { get; set; }//房间ID

        [Required]
        public int UserId { get; set; }//用户ID

        [Required]
        public int AnchorId { get; set; }//主播ID

        [MaxLength(32)]
        public string Name { get; set; }//房间名

        public int RoomNum { get; set; }//房间号码

        public bool IsLiving { get; set; }//是否正在直播

        public DateTime? LastLiveTime { get; set; }//最后直播时间

        public int Viewer { get; set; }//观众

        public string StreamChannel { get; set; }//推流频道


    }
}
