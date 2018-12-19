using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class LiveClass//直播分类
    {
        [Key]
        public int Id { get; set; }

        public int ParentId { get; set; }//父ID

        public bool IsFinal { get; set; }//是否最终节点

        public string Name { get; set; }//名称
        
    }
}