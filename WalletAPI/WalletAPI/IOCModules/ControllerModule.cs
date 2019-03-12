using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using WalletAPI.Chat;
using WalletAPI.Controllers;

namespace WalletAPI.IOCModules
{
    public class ControllerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ChatMessageController>().PropertiesAutowired();
            builder.RegisterType<ExcelController>().PropertiesAutowired();
            builder.RegisterType<FileUpLoadController>().PropertiesAutowired();     //文件上传Controller
            builder.RegisterType<ValuesController>().PropertiesAutowired();
            builder.RegisterType<UserController>().PropertiesAutowired();
            builder.RegisterType<AuthorizeController>().PropertiesAutowired();
            
            builder.RegisterType<HostingEnvironment>().As<IHostingEnvironment>().PropertiesAutowired().InstancePerLifetimeScope();

            /*Signalr 注入*/
            builder.RegisterType<MyChatHub>().PropertiesAutowired();
        }
    }
}
