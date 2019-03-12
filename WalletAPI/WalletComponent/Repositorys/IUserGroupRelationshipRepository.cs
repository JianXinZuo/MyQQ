using System;
using System.Collections.Generic;
using System.Text;
using WalletComponent.Domains;

namespace WalletComponent.Repositorys
{
    public interface IUserGroupRelationshipRepository : IRepository<UserGroupRelationship, Guid>
    {
    }
}
