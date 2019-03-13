using System;
using System.Collections.Generic;
using System.Text;
using WalletComponent.ChatMessage;
using WalletComponent.Domains;

namespace WalletComponent.Services
{
    public interface IChatMessagesService
    {
        List<ChatMsg> GetList(int index, int size, string from, string to, out int total);

        /*测试IOC 的生命周期*/
        Guid GetDbContextId();
    }
}
