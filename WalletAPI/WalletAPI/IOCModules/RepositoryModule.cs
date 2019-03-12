using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;
using WalletComponent.Providers;
using WalletComponent.Providers.Default;
using WalletComponent.Repositorys;
using WalletComponent.Repositorys.EF;

namespace WalletAPI.IOCModules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            
            builder.RegisterType<MyDbContext>().AsSelf().PropertiesAutowired();      //.EnableInterfaceInterceptors()

            builder.RegisterType<UsersRepository>().As<IUsersRepository>().PropertiesAutowired().InstancePerLifetimeScope();
            builder.RegisterType<FriendNotificationRepository>().As<IFriendNotificationRepository>().PropertiesAutowired().InstancePerLifetimeScope();
            builder.RegisterType<UserGroupRelationshipRepository>().As<IUserGroupRelationshipRepository>().PropertiesAutowired().InstancePerLifetimeScope();
            builder.RegisterType<ChatMessagesRepository>().As<IChatMessagesRepository>().PropertiesAutowired().InstancePerLifetimeScope();
            
            //微软云存储注入
            builder.RegisterType<AzureStorageProvider>().As<IStorageProvider>().PropertiesAutowired().InstancePerLifetimeScope();
            builder.RegisterType<JwtSettingsProvider>().As<IJwtProvider>().PropertiesAutowired().InstancePerLifetimeScope();

        }
    }
}
