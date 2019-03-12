using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WalletComponent.ChatMessage;

namespace WalletComponent.Domains
{
    public class ChatMessages
    {
        public Guid Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
        public MessageType Type { get; set; }
        public DateTime CreateTime { get; set; }

    }
    /// <summary>
    /// 用于消息中显示用户信息
    /// </summary>
    public class ChatMessageUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string HeadImg { get; set; }
        public string Mobile { get; set; }
        public bool Sex { get; set; }
        public Int32 Age { get; set; }
    }

    public class TextMessage
    {
        public Guid Id { get; set; }
        public ChatMessageUser From { get; set; }
        public ChatMessageUser To { get; set; }
        public string Text { get; set; }
        public MessageType Type { get; set; }
        public DateTime CreateTime { get; set; }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class SoundMessage
    {
        public Guid Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
        public Int32 Type { get; set; }
        public DateTime CreateTime { get; set; }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class ChatMessagesDemo
    {
        public Guid Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
        public MessageType Type { get; set; }
        public DateTime CreateTime { get; set; }

    }


}
