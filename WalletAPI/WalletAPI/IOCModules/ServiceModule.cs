using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using WalletComponent.Services;
using WalletComponent.Services.Default;

namespace WalletAPI.IOCModules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            //配置AutoMapper
            builder.RegisterType<Mapper>().As<IMapper>().PropertiesAutowired().InstancePerLifetimeScope();

            builder.RegisterType<UserService>().As<IUserService>().PropertiesAutowired().InstancePerLifetimeScope();
            builder.RegisterType<ChatMessagesService>().As<IChatMessagesService>().PropertiesAutowired().InstancePerLifetimeScope();
        }
    }
}
