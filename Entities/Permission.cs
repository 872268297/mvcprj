using System;
using System.ComponentModel.DataAnnotations;

namespace Entities {
    public class Permission {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength (32)]
        public string PermissionName { get; set; }

        public int ParentPermissionId { get; set; }
    }
}