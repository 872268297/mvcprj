using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace mvc.Entities
{
    public class UserAsset//用户资源
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int Level { get; set; }//等级

        public int Exp { get; set; }//经验

        public decimal Gold { get; set; }//金币

        public decimal Silver { get; set; }//银币

        public decimal RechargeAmount { get; set; }//历史充值金额


    }
}
