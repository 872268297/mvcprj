using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Anchor//主播
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        
        public int Follower { get; set; }//关注

    }
}