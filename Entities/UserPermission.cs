using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities {
    public class UserPermission {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PermissionId { get; set; }
    }
}