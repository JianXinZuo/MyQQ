using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using WalletComponent.Common.EFCoreExtend;
using WalletComponent.Domains;
using WalletComponent.DtoModel;

namespace WalletComponent.Repositorys.EF
{
    public class UsersRepository : Repository<Users, Guid>, IUsersRepository
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public List<UserDTO> GetListByName(string nickName)
        {
            return
                DbContext.Database.SqlQuery<UserDTO>(
                    String.Format(@"Select * From Users Where NickName like '%@NickName%'"), new[] { new SqlParameter("@NickName", nickName) }  //  Where NickName like '%@NickName%'
                ).ToList();
        }

        /// <summary>
        /// 根据用户名搜索
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<UserDTO> GetListByUserName(string userName)
        {
            return
                DbContext.Database.SqlQuery<UserDTO>(
                    String.Format(@"Select * From Users Where UserName=@UserName"), new[] { new SqlParameter("@UserName", userName) }  //  Where NickName like '%@NickName%'
                ).ToList();
        }

        public List<UserDTO> GetListByUserNameTest(string userName)
        {
            return
                DbContext.Database.SqlQuery<UserDTO>(
                    String.Format(@"Select Id,NickName,UserName,PassWord From Users Where UserName=@UserName"), new[] { new SqlParameter("@UserName", userName) }  //  Where NickName like '%@NickName%'
                ).ToList();
        }

        public int IsExistsByUserName(string userName)
        {
            int count = DbContext.Database.SqlQuery<int>(
                    String.Format(@"Select Count(Id) From Users Where Id=@Id"), 
                    new SqlParameter("@Id", new Guid("DA4E1995-96E5-4C67-A4EF-0CEAAF200395"))
                ).SingleOrDefault();

            int count2 = DbContext.Database.SqlQuery<int>("Select Count(Id) From Users").SingleOrDefault();
            return count;
        }

        /**
         * 测试IOC的生命周期
         * **/
        public Guid GetDbContextId()
        {
            return DbContext.Id;
        }
    }
}
