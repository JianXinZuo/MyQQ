using System;
using System.Collections.Generic;
using System.Text;

namespace WalletComponent.Domains
{
    public class Groups
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Description { get; set; }
        //外键关联
        public Guid UserId { get; set; }
        public virtual Users User { get; set; }
        //一个分组可以有多个用户信息
        public virtual List<UserGroupRelationship> UserGroupRelationship { get; set; }
    }
}
