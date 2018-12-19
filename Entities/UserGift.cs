using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class UserGift//用户拥有礼物
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int GiftId { get; set; }

        public int Count { get; set; }//数量
    }
}