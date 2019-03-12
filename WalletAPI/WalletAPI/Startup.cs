using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Autofac;
using Microsoft.AspNetCore.Mvc.Controllers;
using WalletAPI.IOCModules;
using Autofac.Extensions.DependencyInjection;
using WalletComponent.Repositorys.EF;
using Microsoft.EntityFrameworkCore;
using WalletAPI.Chat;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using AutoMapper;
using WalletComponent.Common.StorageOptions;
using WalletComponent.Common.JwtOptions;

namespace WalletAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Scoped<IControllerActivator, ServiceBasedControllerActivator>());

            #region 配置Jwt

            //将appsettings.json中的JwtSettings部分文件读取到JwtSettings中，这是给其他地方用的
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            //将配置绑定到JwtSettings实例中
            var jwtSettings = new JwtSettings();
            Configuration.Bind("JwtSettings", jwtSettings);

            services.AddAuthentication(options =>
            {
                //认证中间件配置
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(o =>{

                //主要是jwt的token参数设置
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    //Token颁发机构
                    ValidIssuer = jwtSettings.Issuer,

                    //颁发给谁
                    ValidAudience = jwtSettings.Audience,

                    //这里的key要进行加密，需要引用Microsoft.IdentityModel.Tokens
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

                    //ValidateIssuerSigningKey=true,
                    
                    //是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                    ValidateLifetime=true,
                    //允许的服务器时间偏移量
                    //ClockSkew=TimeSpan.Zero
                };
            });

            //添加Claim授权
            services.AddAuthorization(options => {
               options.AddPolicy("SuperAdminOnly", policy => { policy.RequireClaim("SuperAdminOnly"); });
            });

            #endregion
            
            #region 配置SignalR

            services.AddSignalR();

            //配置跨域策略
            services.AddCors(options => options.AddPolicy("SignalrCore",
            policy =>
            {
                policy.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
            }));

            #endregion

            #region 配置MVC

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #endregion

            #region 配置Azure 

            services.AddOptions();
            services.Configure<CloudOptions>(Configuration.GetSection("CloudStorage"));

            #endregion

            #region 配置EFCore 数据库

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole(LogLevel.Debug);

            services.AddEntityFrameworkSqlServer().AddDbContext<MyDbContext>(
                options =>
                {
                    //options.UseSqlServer(Configuration.GetConnectionString("AzureConnection"));
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                    options.UseLazyLoadingProxies();
                    options.UseLoggerFactory(loggerFactory);
                });

            #endregion

            #region 配置AutoMapper

            services.AddAutoMapper();

            #endregion

            #region 配置Autofac

            var builder = new ContainerBuilder();
            builder.RegisterModule<RepositoryModule>();
            builder.RegisterModule<ServiceModule>();
            builder.RegisterModule<ControllerModule>();
            builder.Populate(services);
            var container = builder.Build();
            return new AutofacServiceProvider(container);

            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //配置跨域,注意引入的策略名一定要在ConfigureServices中配置过
            app.UseCors("SignalrCore");

            //添加SignalR的路由
            app.UseSignalR(route =>
            {
                route.MapHub<MyChatHub>("/myChatHub");
            });

            app.UseStaticFiles();   //使用静态资源
            app.UseAuthentication();    //添加认证

            app.UseMvc();
            app.UseHttpsRedirection();
        }
    }
}
