using Microsoft.AspNetCore.Http;
using mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mvc.Services
{

    public class RoomPool
    {
        //public Dictionary<string, WebSocket> _pool = new Dictionary<string, WebSocket>();
        public Dictionary<WebSocket, string> _wTos = new Dictionary<WebSocket, string>();
    }

    public class WSHandler
    {
        private static Dictionary<string, RoomPool> _pool = new Dictionary<string, RoomPool>();
        public static async Task Echo(HttpContext context, WebSocket webSocket)
        {
            string user = context.Request.Query["user"];
            string roomid = context.Request.Query["roomid"];

            if (user != "")//非游客连接
            {
                UserData u = context.Session.Get<UserData>("UserData");
                if (u == null || u.UserName != user)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "非法登录", CancellationToken.None);
                    return;
                }
            }

            lock (_pool)
            {
                if (!_pool.ContainsKey(roomid))
                {
                    _pool.Add(roomid, new RoomPool());
                }
            }
            RoomPool p = _pool[roomid];
            //lock (p._pool)
            //{
            //    if (!string.IsNullOrWhiteSpace(user))
            //    {
            //        if (_pool.ContainsKey(user))
            //        {
            //            WebSocket s = p._pool[user];
            //            _pool.Remove(user);
            //            if (p._wTos.ContainsKey(s))
            //            {
            //                lock (p._wTos)
            //                {
            //                    p._wTos.Remove(s);
            //                }
            //            }
            //            s.CloseAsync(WebSocketCloseStatus.NormalClosure, "在其他地方登录", CancellationToken.None);
            //        }
            //        p._pool.Add(user, webSocket);
            //    }
            //    p._wTos.Add(webSocket, user);
            //}
            lock (p._wTos)
            {
                p._wTos.Add(webSocket, user);
            }
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                //byte[] b = Encoding.UTF8.GetBytes(user + "騛兯" + Encoding.UTF8.GetString(buffer, 0, result.Count));
                foreach (var item in p._wTos.Keys)
                {
                    await item.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                }
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }


            lock (p._wTos)
            {
                if (p._wTos.ContainsKey(webSocket))
                {
                    p._wTos.Remove(webSocket);
                }
            }

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
