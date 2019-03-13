using System;
using System.Collections.Generic;
using System.Text;
using WalletComponent.ChatMessage;
using WalletComponent.Domains;
using WalletComponent.Repositorys;
using WalletComponent.Repositorys.EF;

namespace WalletComponent.Services.Default
{
    public class ChatMessagesService : IChatMessagesService
    {
        public IChatMessagesRepository ChatMessagesRepository { get; set; }
        public List<ChatMsg> GetList(int index, int size, string from, string to, out int total)
        {
            int start = (index - 1) * size + 1;
            int end = index * size;

            return ChatMessagesRepository.GetList(start, end, from, to, out total);
        }

        /**
         * 测试IOC生命周期
         * **/
        public MyDbContext DbContextTest2 { get; set; }
        public Guid GetDbContextId()
        {
            return DbContextTest2.Id;
        }

    }
}
