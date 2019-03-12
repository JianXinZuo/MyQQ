using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletComponent.Domains;
using WalletComponent.Repositorys;
using WalletComponent.Services;

namespace WalletAPI.Controllers
{
    //[Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public IUsersRepository UsersRepository { get; set; }
        public IChatMessagesService ChatMessagesService { get; set; }

        // GET api/values
        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(new { TestStr = "你好，恭喜你测试成功123abcABC"});

            //var Id1 = UsersRepository.GetDbContextId();
            //var Id2 = ChatMessagesService.GetDbContextId();
            //return new JsonResult(new
            //{
            //    TestIOC1 = Id1,
            //    TestIOC2 = Id2
            //});
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            Users model = new Users();
            model.NickName = "张三";
            model.PassWord = "123456";
            model.UserName = "admin";
            model.Description = "xx";

            UsersRepository.Add(model);
            UsersRepository.SaveChanges();
            return new JsonResult(new { users = model });
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
