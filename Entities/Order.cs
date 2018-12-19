using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Order//订单
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public string GoodGuid { get; set; }//商品唯一ID

        public string Type { get; set; }//种类

        public decimal UnitPrice { get; set; }//单价

        public string PayWay { get; set; }//支付方式

        public int Count { get; set; }//数量

        public decimal ToTalPrice { get; set; }//总价

        public decimal RecevieMoney { get; set; }//实收

        public DateTime CreateTime { get; set; }//创建时间

        public DateTime? CompleteTime { get; set; }//完成时间

        public int Status { get; set; }//状态

    }
}