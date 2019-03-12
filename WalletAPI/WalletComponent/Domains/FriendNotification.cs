using System;
using System.Collections.Generic;
using System.Text;

namespace WalletComponent.Domains
{
    public class FriendNotification
    {
        public Guid Id { get; set; }

        public Guid FromUserId { get; set; }
        public virtual Users FromUser { get; set; }

        public Guid ToUserId { get; set; }
        public virtual Users ToUser { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 附加消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 1：未读通知 2：拒绝添加 3：同意添加
        /// </summary>
        public int State { get; set; }

        public string Description { get; set; }
    }
}
