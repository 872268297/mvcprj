using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class ComsumeRecord//消费记录
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        
        public int GoodGuid { get; set; }//商品唯一ID

        public decimal UnitPrice { get; set; }//单价

        public string Type { get; set; }//类型

        public int Count { get; set; }//数目

        public decimal TotalPrice { get; set; }//总价

        public DateTime CreateTime { get; set; }//时间


    }
}