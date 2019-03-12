using System;
using System.Collections.Generic;
using System.Text;

namespace WalletComponent.DtoModel
{
    public class UserGroupRelationshipDTO
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
        public virtual UserInfo Users { get; set; }
    }

    public class UserInfo
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string HeadImg { get; set; }
        public string Mobile { get; set; }
        public bool Sex { get; set; }
        public Int32 Age { get; set; }
    }
}
