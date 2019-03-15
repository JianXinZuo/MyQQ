using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WalletComponent.Domains;
using WalletComponent.DtoModel;

namespace WalletComponent.Services
{
    public interface IUserService
    {
        Users RegisterUser(UserDTO vm);
        Task<int> UpdateUserInfo(UserDTO vm);
        List<Users> GetUserAll();
        Users GetUserById(Guid id);
        List<Users> GetUserByUserName(string uName);
        Users UserLogin(UserDTO vm);
        bool IsExists(string userName);
    }
}
