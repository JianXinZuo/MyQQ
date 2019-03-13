using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WalletComponent.Common.EFCoreExtend;
using WalletComponent.Domains;
using WalletComponent.ChatMessage;

namespace WalletComponent.Repositorys.EF
{
    public class ChatMessagesRepository : Repository<ChatMessages, Guid>, IChatMessagesRepository
    {
        public List<ChatMsg> GetList(int start, int end, string from, string to, out int total)
        {
            total = DbContext.Set<ChatMessages>().Count(c => (c.From == from && c.To == to) || (c.From == to && c.To == from));

            SqlParameter[] param ={
                new SqlParameter("@p0",from),
                new SqlParameter("@p1",to),
                new SqlParameter("@start", start),
                new SqlParameter("@end", end),
            };

            var list = DbContext.Database.SqlQuery<ChatMsg>(String.Format(@"
Select * From
(
	Select ROW_NUMBER() over (order by [CreateTime] desc) as rowno,
		[Id],[From],[To],[Message],[Type],[CreateTime]
	From ChatMessages Where ([from]=@p0 and [to]=@p1) or ([from]=@p1 and [to]=@p0)
) as t Where t.rowno between @start and @end"), param).ToList();
            return list;
        }
    }
}
