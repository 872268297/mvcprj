using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class RechargeRecord//充值记录
    {
        [Key]
        public int Id { get; set; }
        
        public decimal Amount { get; set; }//金额

        public DateTime CreateTime { get; set; }//充值时间

        public string BZ { get; set; }// 备注

        public string Type { get; set; }//购买种类（金币/银币）

        public decimal Count { get; set; }//购买数目

        public string PayWay { get; set; }//支付方式
    }
}