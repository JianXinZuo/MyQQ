using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NLog;
using WalletComponent.DtoModel;
using WalletComponent.Providers;
using WalletComponent.Services;

namespace WalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //private JwtSettings jwtSettings;
        //public AuthorizeController(IOptions<JwtSettings> setting) => jwtSettings = setting.Value;

        public IUserService UserService { get; set; }
        public IJwtProvider JwtSettingsProvider { get; set; }
        public IMapper Mapper { get; set; }

        [HttpPost]
        public IActionResult Token([FromBody]UserDTO viewModel)
        {
            try
            {
                if (ModelState.IsValid) //判断是否合法
                {
                    var user = UserService.UserLogin(viewModel);

                    if (user == null)//判断账号密码是否正确
                        return new JsonResult(new { isok = false, data = "", msg = "未登录" });

                    var claim = new Claim[]{
                        new Claim("UserId",user.Id.ToString()),
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.Role,"admin"),
                        new Claim("SuperAdminOnly","true")
                    };

                    //对称秘钥
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettingsProvider.Setting.SecretKey));

                    //签名证书(秘钥，加密算法)
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    //生成token  [注意]需要nuget添加Microsoft.AspNetCore.Authentication.JwtBearer包，并引用System.IdentityModel.Tokens.Jwt命名空间
                    var token = new JwtSecurityToken(
                        JwtSettingsProvider.Setting.Issuer,
                        JwtSettingsProvider.Setting.Audience,
                        claim,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(30),
                        creds);

                    var tokenstr = new JwtSecurityTokenHandler().WriteToken(token);
                    var res = Mapper.Map<UserDTO>(user);

                    return new JsonResult(new { isok = true, data = new { token = tokenstr, user = res }, msg = "成功" });
                }

                return new JsonResult(new { isok = false, data = "", msg = "未登录" });
            }
            catch(Exception exp)
            {
                logger.Info("登录时系统错误：" + exp.Message);
                return new JsonResult(new { isok = false, data = exp.Message, msg = "失败" });
            }
        }
    }
}