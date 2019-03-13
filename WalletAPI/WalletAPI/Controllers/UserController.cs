using Microsoft.AspNetCore.Mvc;
using WalletComponent.DtoModel;
using WalletComponent.Services;
using NLog;
using System.Threading;
using AutoMapper;
using WalletComponent.Domains;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Http;
using WalletComponent.Repositorys;

namespace WalletAPI.Controllers
{
    
    //[Authorize(Roles = "admin")]
    //[Consumes("application/json", "multipart/form-data")]   //此处为新增
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        /**
         * 依赖注入
         */
        public IUserService UserService { get; set; }
        public IUsersRepository UsersRepository { get; set; }
        public IMapper Mapper { get; set; }

        // GET: api/User
        
        [HttpGet]
        public JsonResult Get()
        {

            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();  //开始监视代码运行时间

            //var res = UsersRepository.IsExistsByUserName("3F708CC3-715B-4E98-B937-1DF818668D33");
            var res = UsersRepository.GetListByUserName("dupeng");
            var res2 = UsersRepository.GetListByUserNameTest("dupeng");

            watch.Stop();  //停止监视
            TimeSpan timespan = watch.Elapsed;  //获取当前实例测量得出的总时间

            logger.Info("本次扩展Sql方法执行时间为：" + timespan.TotalMilliseconds + "(毫秒)");

            return new JsonResult(new { isok = false, data = res, res2 = res2, msg = "暂无数据" });
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        [Authorize(Policy = "SuperAdminOnly")]
        public JsonResult Get(Guid id)
        {
            try
            {
                var user = UserService.GetUserById(id);
                var res = Mapper.Map<UserDTO>(user);
                return new JsonResult(new { isok = true, data = res, msg = "成功" });
            }
            catch(Exception exp)
            {
                logger.Error("查询用户时出错：" + exp.Message);
                return new JsonResult(new { isok = false, data = "", msg = exp.Message });
            }
        }

        // GET: api/User/GetUserByUserName?uname=
        [HttpGet("GetUserByUserName")]
        [Authorize(Policy = "SuperAdminOnly")]
        public JsonResult Get(string uname)
        {
            try
            {
                var userlist = UsersRepository.GetListByUserName(uname);
                return new JsonResult(new { isok = true, data = userlist, msg = "成功" });
            }
            catch (Exception exp)
            {
                logger.Error("查询用户时出错：" + exp.Message);
                return new JsonResult(new { isok = false, data = "", msg = exp.Message });
            }
        }

        // POST: api/User
        [HttpPost]
        public JsonResult Post([FromBody] UserDTO model)
        {
            try
            {
                bool isExists = UserService.IsExists(model.UserName);   //判断用户是否存在
                if (isExists == false)
                {
                    var user = UserService.RegisterUser(model);
                    var res = Mapper.Map<UserDTO>(user);
                    return new JsonResult(new { isok = true, data = res, msg = "成功" });
                }
                else
                {
                    return new JsonResult(new { isok = false, data = "", msg = "该用户已经注册" });
                }
            }
            catch(Exception exp)
            {
                logger.Info("注册用户时系统错误：" + exp.Message);
                return new JsonResult(new { isok = false, data = exp.Message, msg = "失败" });
            }
            
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        [Authorize(Policy = "SuperAdminOnly")]
        public JsonResult Put(Guid id, [FromBody] UserDTO model)
        {
            return new JsonResult(new { isok = true, data = "", msg = "成功" });
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "SuperAdminOnly")]
        public void Delete(Guid id)
        {
        }
    }
}
