using System;
using System.Collections.Generic;
using System.Text;

namespace WalletComponent.DtoModel
{
    public class FriendNotificationDTO
    {
        public Guid Id { get; set; }

        public Guid FromUvserId { get; set; }
        public UserDTO FromUser { get; set; }

        public Guid ToUserId { get; set; }
        public UserDTO ToUser { get; set; }

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
