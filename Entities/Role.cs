using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities {
    public class Role {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength (32)]
        public string RoleName { get; set; }
    }
}