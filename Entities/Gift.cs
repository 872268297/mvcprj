using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Gift//礼物
    {
        [Key]
        public int Id { get; set; }

        public string Guid { get; set; }//全局唯一ID

        [MaxLength(32)]
        public string Type { get; set; }//种类（金币/银币）

        public string Name { get; set; }//名称

        public decimal Price { get; set; }//价格

        public string ImgUrl { get; set; }//图片地址

        public bool IsShow { get; set; }//是否上线
    }
}