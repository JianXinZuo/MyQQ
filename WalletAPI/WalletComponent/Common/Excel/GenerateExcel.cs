using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WalletComponent.Common.Excel
{
    public class GenerateExcel
    {
        private List<BaseGenerateSheet> _sheetList;
        private string _path;

        public GenerateExcel(string path)
        {
            _path = path;
            _sheetList = new List<BaseGenerateSheet>();
        }

        public void AddSheet(BaseGenerateSheet sheet)
        {
            _sheetList.Add(sheet);
        }

        public void ExportExcel()
        {
            using (Stream stream = new FileStream(_path, FileMode.Create))
            {
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorkbook workbook = package.Workbook;

                    for (int i = 0, imax = _sheetList.Count; i < imax; i++)
                    {
                        ExcelWorksheet sheet = workbook.Worksheets.Add(_sheetList[i].SheetName);
                        _sheetList[i].GenSheet(sheet);
                    }

                    package.Save();
                }
            }
        }

    }
}
