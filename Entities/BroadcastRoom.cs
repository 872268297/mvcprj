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

        public int ClassId { get; set; }//分类ID

        public int RoomNum { get; set; }//房间号码

        public bool IsLiving { get; set; }//是否正在直播

        public bool IsBan { get; set; }//是否封禁

        public DateTime? LastLiveTime { get; set; }//最后直播时间

        public int Viewer { get; set; }//观众

        public string StreamChannel { get; set; }//推流频道

        public string Notice { get; set; }//直播公告

        public string CoverUrl { get; set; }//房间封面

        public string StreamCode { get; set; }//推流码

        public bool IsCustomCover { get; set; }//个性化封面
    }
}
