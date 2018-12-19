using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class GiftReceiveRecord//礼物赠送记录
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SendUserId { get; set; }//赠送人id
        
        public int ReceiveUserId { get; set; }//收礼人id

        public int GiftId { get; set; }//礼物id

        public int Count { get; set; }//数目

        public DateTime CreateTime { get; set; }//时间

        public int Status { get; set; }//状态 
    }
}