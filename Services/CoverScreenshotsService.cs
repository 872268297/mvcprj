using Entities;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace mvc.Services
{
    public class CoverScreenshotsService : BackgroundService
    {
        //private readonly MyDbContext _dbContext;
        //private IServiceProvider _service;
        public static DbContextOptions<MyDbContext> options;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public CoverScreenshotsService(Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment)
        {
            //this.options = options;
            this._hostingEnvironment = _hostingEnvironment;
            //_service.GetService()
            //this._dbContext = _dbContext;
            //this._dbContext = _dbContext;
            //this._service = _service;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                //MyDbContext _context = _service.GetService(typeof(MyDbContext)) as MyDbContext;
                Console.WriteLine("开启封面截取定时器");
                string webRootPath = _hostingEnvironment.WebRootPath;
                while (true)
                {
                    var _dbcontext = new MyDbContext(options);
                    //var query = from c in _dbContext.BroadcastRooms
                    //            where c.IsCustomCover == false && c.IsLiving == true
                    //            select c;
                    //var list =  query.ToList();
                    var list = _dbcontext.BroadcastRooms.Where(c => c.IsCustomCover == false && c.IsLiving == true).ToList();

                    foreach (var item in list)
                    {
                        //Console.WriteLine(item.RoomNum);
                        ThreadPool.QueueUserWorkItem(t =>
                        {
                            string img_name = webRootPath + "/upload/cover/" + item.StreamChannel + ".jpg";
                            Process p = new Process();//建立外部调用线程
                            p.StartInfo.FileName = "ffmpeg";//要调用外部程序的绝对路径
                            p.StartInfo.Arguments = " -i " + "rtmp://" + ServerService.GetRtmpAddr() + item.StreamChannel
                                    + " -y -f image2 -ss 1"
                                    + " -t 0.001 -s 332*195"
                                    + " " + img_name;  //設定程式執行參數
                            p.StartInfo.UseShellExecute = false;//不使用操作系统外壳程序启动线程(一定为FALSE,详细的请看MSDN)
                            p.StartInfo.RedirectStandardError = true;//把外部程序错误输出写到StandardError流中(这个一定要注意,FFMPEG的所有输出信息,都为错误输出流,用StandardOutput是捕获不到任何消息的...这是我耗费了2个多月得出来的经验...mencoder就是用standardOutput来捕获的)
                            p.StartInfo.CreateNoWindow = false;//不创建进程窗口
                                                               //p.ErrorDataReceived += new DataReceivedEventHandler(Output);//外部程序(这里是FFMPEG)输出流时候产生的事件,这里是把流的处理过程转移到下面的方法中,详细请查阅MSDN
                            p.Start();//启动线程
                                      //p.BeginErrorReadLine();//开始异步读取
                            Thread.Sleep(5000);
                            if (!p.HasExited)
                            {
                                p.Kill();
                            }
                            //p.WaitForExit();//阻塞等待进程结束
                            p.Close();//关闭进程
                            p.Dispose();//释放资源
                        });


                    }
                    //Console.WriteLine(DateTime.Now.ToString() + " 执行逻辑");
                    await Task.Delay(30000, stoppingToken); //每30秒执行一次
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
