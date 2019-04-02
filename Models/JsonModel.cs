using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.Models
{
    public class JsonModel
    {
        public bool success;
        public string message;
        public object data;
        public int count;
        public JsonModel()
        {

        }

        public JsonModel(bool success, string message, object data = null, int count = 0)
        {
            this.success = success;
            this.message = message;
            this.data = data;
            this.count = count;
        }
    }
}
