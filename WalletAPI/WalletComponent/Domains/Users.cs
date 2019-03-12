using System;
using System.Collections.Generic;
using System.Text;
using WalletComponent.Common.Excel;

namespace WalletComponent.Domains
{
    public class Users
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string PassWord { get; set; }
        public string NickName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImg { get; set; }
        public string Mobile { get; set; }
        public bool Sex { get; set; }
        public Int32 Age { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IdCard{ get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string FullName { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// 分组列表    1对多  一个用户可以创建多个分组，一个分组只能属于一个用户
        /// </summary>
        public virtual List<Groups> GroupList { get; set; }

        public virtual List<UserGroupRelationship> UserGroupRelationshipList { get; set; }

        /// <summary>
        /// 收到的好友通知列表
        /// </summary>
        public virtual List<FriendNotification> FriendNotificationListByTo { get; set; }

        /// <summary>
        /// 发起的好友通知
        /// </summary>
        public virtual List<FriendNotification> FriendNotificationListByFrom { get; set; }
    }
}
