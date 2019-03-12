using System;
using System.Collections.Generic;
using System.Text;

namespace WalletComponent.Common.Excel
{
    public class ExcelColumnInfoAttribute : Attribute
    {
        public string ColumnName { get; private set; }
        public ExcelColumnInfoAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }
}
