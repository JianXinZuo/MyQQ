using System;
using System.Collections.Generic;
using System.Text;
using WalletComponent.Domains;

namespace WalletComponent.Repositorys
{
    public interface IChatMessagesRepository: IRepository<ChatMessages, Guid>
    {
        /// <summary>
        /// 获取个人聊天记录
        /// </summary>
        /// <param name="start">开始行</param>
        /// <param name="end">结束行</param>
        /// <param name="from">发起人</param>
        /// <param name="to">接受人</param>
        /// <param name="total">总数</param>
        /// <returns></returns>
        List<ChatMessages> GetList(int start, int end, string from, string to, out int total);
    }
}
