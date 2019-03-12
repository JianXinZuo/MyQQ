using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using WalletComponent.Services;

namespace WalletAPI.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessageController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public IChatMessagesService ChatMessagesService { get; set; }

        // GET: api/ChatMessage
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ChatMessage/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // GET: api/ChatMessage/List?from=
        [HttpGet("List")]
        public JsonResult Get(string from, string to, int index, int size)
        {
            try
            {
                int total = 0;
                var list = ChatMessagesService.GetList(index,size,from,to, out total);
                return new JsonResult(new { isok = true, data = list, total, msg = "成功" });
            }
            catch(Exception exp)
            {
                logger.Error("查询用户时出错：" + exp.Message);
                return new JsonResult(new { isok = false, data = "", total=0, msg = exp.Message });
            }
        }

        // POST: api/ChatMessage
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ChatMessage/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
