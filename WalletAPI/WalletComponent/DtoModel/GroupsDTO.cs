using System;
using System.Collections.Generic;
using System.Text;

namespace WalletComponent.DtoModel
{
    public class GroupsDTO
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Description { get; set; }
        //外键关联
        public Guid UserId { get; set; }
        //一个分组可以有多个用户信息
        public virtual List<UserGroupRelationshipDTO> UserGroupRelationship { get; set; }
    }
}
