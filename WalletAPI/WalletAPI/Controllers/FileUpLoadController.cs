using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using WalletComponent.Common.Helpers;
using WalletComponent.Providers;
using WalletComponent.Services;

namespace WalletAPI.Controllers
{
    [Consumes("application/json", "multipart/form-data")]   //此处为新增表单提交方式
    [Route("api/[controller]")]
    [ApiController]
    public class FileUpLoadController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public IUserService UserService { get; set; }
        public IHostingEnvironment HostingEnvironment { get; set; }
        public IStorageProvider StorageProvider { get; set; }

        // POST: api/UpLoad
        [HttpPost]
        public async Task<JsonResult> Post(IFormCollection files)
        {
            try
            {
                /* 开启 Azure 云存储*/
                IFormFile file = Request.Form.Files[0];
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName).ToString();
                string path = String.Format("headimg01/{0}", fileName);
                Stream stream = file.OpenReadStream();

                //将上传的文件生成缩略图
                byte[] buffer = Utils.StreamToBytes(stream);
                int width = 200;
                int height = 200;
                byte[] thumBuffer = Utils.GenThumbnail(buffer, 800, 800, out width, out height);

                //将缩略图转成流并上传Azure
                Stream thumStream = Utils.BytesToStream(thumBuffer);
                await StorageProvider.UploadStreamAsync(thumStream, path).ConfigureAwait(false);

                return new JsonResult(new { isok = true, data = path, msg = "成功" });

                /*
                IFormFile file = Request.Form.Files[0];
                string webRootPath = HostingEnvironment.WebRootPath;
                string fileExt = Path.GetExtension(file.FileName);  //获取文件扩展名
                string fileName = Guid.NewGuid().ToString() + fileExt;  //生成新文件名
                var basePath = webRootPath + "/upload/";    //上传的目录

                if (!Directory.Exists(basePath))     //判断目录是否存在，如果不存在创建目录 
                    Directory.CreateDirectory(basePath);

                var filePath = basePath + fileName;

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                string res = "/upload/" + fileName;
                return new JsonResult(new { isok = true, data = res, msg = "成功" });
                */
            }
            catch (Exception exp)
            {
                logger.Info("上传文件错误：" + exp.Message);
                return new JsonResult(new { isok = false, data = exp.Message, msg = "失败" });
            }
        }
    }
}
