using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Server//服务器
    {
        [Key]
        public int Id { get; set; }

        public string IpAddress { get; set; }//Ip地址

        public int Port { get; set; }//端口

        public string Type { get; set; }//种类

    }
}