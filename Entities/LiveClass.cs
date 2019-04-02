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

        public string Name { get; set; }//名称

        public int Order { get; set; }//排序
        

    }
}