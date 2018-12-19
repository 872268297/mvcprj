using Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace mvc.Models
{
    [Serializable]
    public class UserData
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Level { get; set; }//等级
        public int Exp { get; set; }//经验
        public List<Role> Roles { get; set; }
        public DateTime LoginTime { get; set; }
        public List<Permission> Permissions { get; set; }

        public UserData()
        {
            Roles = new List<Role>();
            Permissions = new List<Permission>();
        }

        public bool CheckPermission(string per)
        {
            return (from c in Permissions where c.PermissionName == per select 1).Any();
        }
        public bool CheckPermission(int perId)
        {
            return (from c in Permissions where c.Id == perId select 1).Any();
        }

        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public static UserData Current
        {
            get { return MyHttpContext.Current.Session.Get<UserData>("UserData"); }
        }
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public static string CurrentId
        {
            get { return MyHttpContext.Current.Session.GetString("UserId"); }
        }
    }
}
