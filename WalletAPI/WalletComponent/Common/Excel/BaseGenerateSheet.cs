using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace WalletComponent.Common.Excel
{
    public abstract class BaseGenerateSheet
    {
        //public ExcelWorkbook WorkBook { get; protected set; }
        public string SheetName { get; protected set; }
        public abstract void GenSheet(ExcelWorksheet sheet);
    }
}
