using System;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NLog;
using OfficeOpenXml;
using WalletComponent.Common.Excel;
using WalletComponent.Domains;
using WalletComponent.DtoModel;
using WalletComponent.Repositorys;
using WalletComponent.Services;

namespace WalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public IHostingEnvironment HostingEnvironment { get; set; }
        public IUsersRepository UsersRepository { get; set; }
        public IUserService UserService { get; set; }
        public IMapper Mapper { get; set; }

        // GET: api/Excel
        [HttpGet("Export")]
        public JsonResult Export()
        {
            try
            {
                string webRootPath = HostingEnvironment.WebRootPath;
                string basePath = webRootPath + "/excelTemp/";

                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                string fileName = Guid.NewGuid().ToString() + ".xlsx";
                var filePath = basePath + fileName;

                List<UserDTO> resultRows = UsersRepository.GetListByName("admin");
                GenerateSheet<UserDTO> sheet = new GenerateSheet<UserDTO>(resultRows, "sheet1");
                GenerateExcel excel = new GenerateExcel(filePath);      //目录
                excel.AddSheet(sheet);
                excel.ExportExcel();

                return new JsonResult(new { isok = true, data = "导出成功", msg = "成功" });
            }
            catch (Exception exp)
            {
                logger.Info("上传文件错误：" + exp.Message);
                return new JsonResult(new { isok = false, data = exp.Message, msg = "失败" });
            }
        }

        [HttpGet("Import")]
        public JsonResult Import()
        {
            string sWebRootFolder = HostingEnvironment.WebRootPath + "/excelTemp/";
            string sFileName = @"测试反射.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            ExcelPackage package = new ExcelPackage(file);
            ExcelWorksheet workSheet = package.Workbook.Worksheets[1];

            int rowEnd = workSheet.Dimension.Rows;
            int colEnd = workSheet.Dimension.Columns;

            for (int row = 2; row < rowEnd; row++)
            {
                for (int col = workSheet.Dimension.Start.Column; col < colEnd; col++)
                {
                    workSheet.Cells[row, col].Value.ToString();
                }
            }
            return new JsonResult(new { isok = false, data = "", msg = "失败" });
        }

        // GET: api/ExportExcel/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ExportExcel
        [HttpPost]
        public JsonResult Post([FromBody] string value)
        {
            return new JsonResult(new { isok = true, data = "", msg = "成功" });
        }

        // PUT: api/ExportExcel/5
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
