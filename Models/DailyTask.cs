using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.Models
{
    public class DailyTask
    {
        public string Discribe { get; set; }
        public int TotalCount { get; set; }
        public int CurrentCount { get; set; }
        public int Exp { get; set; }
        public bool IsComplete { get; set; }
        public int Id { get; set; }
    }
}
