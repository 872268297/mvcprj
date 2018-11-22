using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities {
    public class UserRole {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }
    }
}