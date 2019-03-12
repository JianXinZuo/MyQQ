using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace WalletComponent.Common.Excel
{
    public class GenerateSheetDataReader : GenerateSheet<object>
    {
        private IDataReader _dataSource;

        public GenerateSheetDataReader(IDataReader dataSource, string sheetName)
        {
            _dataSource = dataSource;
            SheetName = sheetName;
        }

        protected override List<ColumnsMapping> InitColumnHeadData()
        {
            List<ColumnsMapping> columnsList = new List<ColumnsMapping>();

            for (int i = 0, imax = _dataSource.FieldCount; i < imax; i++)
            {
                columnsList.Add(new ColumnsMapping()
                {
                    ColumnsData = _dataSource.GetName(i),
                    ColumnsText = _dataSource.GetName(i),
                    ColumnsIndex = i,
                    Width = 15
                });
            }

            return columnsList;
        }

        protected override void SetCellValue(ExcelRange cells)
        {
            if (null != _dataSource)
            {
                int row = 2;
                while (_dataSource.Read())
                {
                    for (int i = 0, imax = _dataSource.FieldCount; i < imax; i++)
                    {
                        cells[row, i + 1].Value = _dataSource[i];
                    }
                    row++;
                }
            }
        }
    }
}
