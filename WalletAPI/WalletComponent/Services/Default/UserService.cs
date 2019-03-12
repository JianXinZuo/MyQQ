using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletComponent.Domains;
using WalletComponent.DtoModel;
using WalletComponent.Repositorys;
using WalletComponent.Repositorys.EF;

namespace WalletComponent.Services.Default
{
    public class UserService : IUserService
    {
        public IUsersRepository UsersRepository { get; set; }
        public IMapper Mapper { get; set; }

        public List<Users> GetUserAll()
        {
            return UsersRepository.FindAll().ToList();
        }

        public Users GetUserById(Guid id)
        {
            var user = UsersRepository.Find(u => u.Id == id).FirstOrDefault();  //.Load(id);
            //int count = UsersRepository.IsExistsByUserName("SDSDS");
            return user;
        }

        public List<Users> GetUserByUserName(string uName)
        {
            try
            {
                var userList = UsersRepository.Find(u => u.UserName == uName).ToList();
                return userList;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public bool IsExists(string userName)
        {
            var user = UsersRepository.Find(u => u.UserName == userName).FirstOrDefault();
            var res = user != null ? true : false;
            return res;
        }

        public Users RegisterUser(UserDTO vm)
        {
            var user = Mapper.Map<Users>(vm);

            user.Id = Guid.NewGuid();
            user.CreateTime = DateTime.Now;
            user.UpdateTime = DateTime.Now;

            List<Groups> li = new List<Groups>();
            Groups group = new Groups();
            group.Id = Guid.NewGuid();
            group.UserId = user.Id;
            group.GroupName = "好友";
            group.CreateTime = DateTime.Now;
            group.UpdateTime = DateTime.Now;
            li.Add(group);

            user.GroupList = li;
            UsersRepository.Add(user);
            UsersRepository.SaveChanges();

            return user;
        }

        public async Task<int> UpdateUserInfo(UserDTO vm)
        {
            var model = await UsersRepository.LoadAsync(vm.Id);
            model.PassWord = vm.PassWord;
            model.NickName = vm.NickName;
            return await UsersRepository.SaveChangesAsync();
        }

        public Users UserLogin(UserDTO vm)
        {
            return UsersRepository.Find(u => u.UserName == vm.UserName && u.PassWord == vm.PassWord).FirstOrDefault();
        }
    }
}
