using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class UserFollow//关注
    {
       

        [Key]
        public int UserId { get; set; }

        [Key]
        public int AnchorId { get; set; }

    }
}