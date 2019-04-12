using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services;
using System.Web;
using System.Net.WebSockets;
using System.Threading;
using mvc.Services;

namespace mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            var connStr = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MyDbContext>(options => options.UseMySql(connStr));

            //services.AddHttpContextAccessor();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.AddDistributedMemoryCache();

            services.AddMemoryCache();

            //添加redis
            //services.AddDistributedRedisCache(options =>
            //{
            //    options.Configuration = "localhost";

            //});

            services.AddSession(o => o.IdleTimeout = TimeSpan.FromHours(4));



            services.AddCors(options =>
            {
                options.AddPolicy("corsSample", p => p.WithOrigins("http://localhost:8080")
                   .AllowAnyMethod().AllowAnyHeader());
                options.AddPolicy("corsSample2", p => p.WithOrigins("http://localhost:5000")
                   .AllowAnyMethod().AllowAnyHeader());
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILiveClassService, LiveClassService>();
            services.AddScoped<IAnchorService, AnchorService>();
            services.AddScoped<IServerService, ServerService>();
            services.AddScoped<IFollowService, FollowService>();
            CoverScreenshotsService.options = new DbContextOptionsBuilder<MyDbContext>().UseMySql(connStr).Options;

            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, CoverScreenshotsService>();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseCors("corsSample2");
            app.UseStaticHttpContext();
            app.UseSession();


            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };

            app.UseWebSockets(webSocketOptions);

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        await WSHandler.Echo(context, webSocket);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }

            });


            //mvc.Util.MyHttpContext.ServiceProvider = app.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
}