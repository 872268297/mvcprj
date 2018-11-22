using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities {
    public class User {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength (32)]
        public string UserName { get; set; }

        [Required]
        [MaxLength (32)]
        public string Password { get; set; }

    }
}