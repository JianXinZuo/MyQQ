using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WalletComponent.ChatMessage
{
    public class AddFriend
    {
        public string Id { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string HeadImg { get; set; }
        public MessageType Type { get; set; }

        public string FromUid { get; set; }
        public string ToUid { get; set; }
        public string Message { get; set; }
        /// <summary>
        /// 1：未读通知 2：拒绝添加 3：同意添加
        /// </summary>
        public int State { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public enum MessageType
    {
        AddUser,
        Text,
        Sound,
        //Online,
        //Offline,
        //Text,
        //Sound,
        //Command,
        //Info,
        //Image,
        //Bonus,
        //PowerPoint,
        //Video,
        //Music,
        //Sign,
        //SignIn,
        //ShortVideo
    }
}
