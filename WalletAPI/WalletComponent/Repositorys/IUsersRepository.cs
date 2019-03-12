using System;
using System.Collections.Generic;
using System.Text;
using WalletComponent.Domains;
using WalletComponent.DtoModel;

namespace WalletComponent.Repositorys
{
    public interface IUsersRepository : IRepository<Users, Guid>
    {
        int IsExistsByUserName(string userName);
        List<UserDTO> GetListByName(string nickName);
        List<UserDTO> GetListByUserName(string userName);
        List<UserDTO> GetListByUserNameTest(string userName);

        /*测试IOC 的生命周期*/
        Guid GetDbContextId();
        
    }
}
