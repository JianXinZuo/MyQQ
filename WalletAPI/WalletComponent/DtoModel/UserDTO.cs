using System;
using System.Collections.Generic;
using WalletComponent.Common.Excel;

namespace WalletComponent.DtoModel
{
    public class UserDTO
    {
        [ExcelColumnInfo("ID")]
        public Guid Id { get; set; }
        [ExcelColumnInfo("用户名")]
        public string UserName { get; set; }
        [ExcelColumnInfo("Code")]
        public string UserCode { get; set; }
        [ExcelColumnInfo("密码")]
        public string PassWord { get; set; }
        [ExcelColumnInfo("昵称")]
        public string NickName { get; set; }
        [ExcelColumnInfo("头像")]
        public string HeadImg { get; set; }
        [ExcelColumnInfo("电话")]
        public string Mobile { get; set; }
        [ExcelColumnInfo("性别")]
        public bool Sex { get; set; }
        [ExcelColumnInfo("年龄")]
        public Int32 Age { get; set; }
        [ExcelColumnInfo("身份证")]
        public string IdCard { get; set; }
        [ExcelColumnInfo("全称")]
        public string FullName { get; set; }
        [ExcelColumnInfo("创建时间")]
        public DateTime CreateTime { get; set; }
        [ExcelColumnInfo("修改时间")]
        public DateTime UpdateTime { get; set; }
        [ExcelColumnInfo("描述")]
        public string Description { get; set; }
        public List<GroupsDTO> GroupList { get; set; }

    }
}
