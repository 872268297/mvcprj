using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace mvc.Entities
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }

        public int ParentId { get; set; }

        public string Name { get; set; }

        public int RequirePermissionId { get; set; }

        public string Url { get; set; }

        public int Index { get; set; }
    }
}
