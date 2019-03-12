using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

namespace WalletComponent.Common.Excel
{
    public class GenerateSheet<T> : BaseGenerateSheet
    {
        // 列头集合
        protected List<ColumnsMapping> _columnHeadList;

        // 显示的数据
        private IEnumerable<T> _dataSource;

        private Dictionary<string, ColumnPropertyInfo> _columnPropertyList;

        protected GenerateSheet() { }

        public GenerateSheet(IEnumerable<T> dataSource, string sheetName)
        {
            this._dataSource = dataSource;
            this.SheetName = sheetName;
        }


        public override void GenSheet(ExcelWorksheet sheet)
        {
            SetSheetContents(sheet);
        }

        /// <summary>
        /// 初始化表头行数据
        /// </summary>
        protected virtual List<ColumnsMapping> InitColumnHeadData()
        {
            List<ColumnsMapping> columnsList = new List<ColumnsMapping>();

            _columnPropertyList = this.GetObjectColumnPropertyList();

            int index = 0;
            foreach (var item in _columnPropertyList)
            {
                ColumnPropertyInfo columnPropertyInfo = item.Value;

                columnsList.Add(new ColumnsMapping()
                {
                    ColumnsData = columnPropertyInfo.PropertyName,
                    ColumnsText = columnPropertyInfo.ShowName,
                    ColumnsIndex = index++,
                    Width = 15
                });
            }

            return columnsList;
        }

        /// <summary>
        /// 设置表头行的字体样式
        /// </summary>
        protected virtual ExcelFont SetColumnHeadFontStyle(ExcelFont font)
        {
            font.Bold = true;//加粗
            font.Color.SetColor(Color.Black);//字体颜色
            font.Name = "微软雅黑";//字体名
            font.Size = 12;//字体大小
            font.Italic = false;//斜体
            font.Strike = false;//删除线
            font.UnderLine = false;//下划线
            //font.UnderLineType = ExcelUnderLineType.Single;//下划线样式
            //font.VerticalAlign = ExcelVerticalAlignmentFont.None;//字体垂直对齐
            return font;
        }

        /// <summary>
        /// 设置表头行的边框样式
        /// </summary>
        protected virtual Border SetColumnHeadBorderStyle(Border border)
        {
            border.BorderAround(ExcelBorderStyle.Thin, Color.FromArgb(191, 191, 191));//设置单元格所有边框
            return border;
        }

        /// <summary>
        /// 设置表头行的单元格样式
        /// </summary>
        protected virtual ExcelFill SetColumnHeadFillStyle(ExcelFill fill)
        {
            //fill.PatternType = ExcelFillStyle.Solid;
            return fill;
        }

        /// <summary>
        /// 设置表整体样式
        /// </summary>
        protected virtual void SetSheetStyle(ExcelWorksheet sheet)
        {
            sheet.View.FreezePanes(2, 1);
            for (int i = 0, imax = _columnHeadList.Count; i < imax; i++)
            {
                sheet.Column(i + 1).AutoFit(10, 30);
            }
        }

        /// <summary>
        /// 设置列头的内容
        /// </summary>
        protected virtual void SetColumnHeadContents(ExcelWorksheet sheet)
        {
            for (int i = 0, imax = _columnHeadList.Count; i < imax; i++)
            {
                sheet.Cells[1, i + 1].Value = _columnHeadList[i].ColumnsText;
            }
        }

        /// <summary>
        /// 设置单元格值
        /// </summary>
        protected virtual void SetCellValue(ExcelRange cells)
        {
            int row = 2;
            foreach (T data in _dataSource)
            {
                for (int j = 0, jmax = _columnHeadList.Count; j < jmax; j++)
                {
                    ColumnsMapping cm = _columnHeadList[j];
                    ColumnPropertyInfo cpi;
                    _columnPropertyList.TryGetValue(cm.ColumnsData, out cpi);
                    cells[row, j + 1].Value = cpi.Property.GetValue(data);
                }
                row++;
            }
        }


        #region 私有方法
        /// <summary>
        /// 设置表格内容
        /// </summary>
        private void SetSheetContents(ExcelWorksheet sheet)
        {
            this._columnHeadList = InitColumnHeadData();
            SetColumnHeadContents(sheet);
            SetCellValue(sheet.Cells);
            SetStyles(sheet);
        }

        /// <summary>
        /// 设置样式
        /// </summary>
        /// <param name="sheet"></param>
        private void SetStyles(ExcelWorksheet sheet)
        {
            SetSheetStyle(sheet);
            ExcelStyle headStype = sheet.Cells[1, 1, 1, _columnHeadList.Count].Style;
            SetColumnHeadFontStyle(headStype.Font);
            SetColumnHeadBorderStyle(headStype.Border);
            SetColumnHeadFillStyle(headStype.Fill);
        }

        public static ConcurrentDictionary<string, Dictionary<string, ColumnPropertyInfo>> MyExcelModels = new ConcurrentDictionary<string, Dictionary<string, ColumnPropertyInfo>>();

        /// <summary>
        /// 获取 T 对像的所有属性
        /// </summary>
        private Dictionary<string, ColumnPropertyInfo> GetObjectColumnPropertyList()
        {
            var modelName = typeof(T).Name;
            if (!MyExcelModels.ContainsKey(modelName))
            {
                Dictionary<string, ColumnPropertyInfo> result = new Dictionary<string, ColumnPropertyInfo>();
                Type t = typeof(T);
                if (t != null)
                {
                    PropertyInfo[] piList = t.GetProperties();
                    foreach (var pi in piList)
                    {
                        if (!pi.PropertyType.IsGenericType)
                        {
                            ExcelColumnInfoAttribute ci = pi.GetCustomAttribute<ExcelColumnInfoAttribute>();

                            if (null == ci)
                                continue;

                            ColumnPropertyInfo colPropInfo = new ColumnPropertyInfo(pi, ci);

                            result.Add(pi.Name, colPropInfo);
                        }
                    }
                }
                MyExcelModels.GetOrAdd(modelName, result);
                return result;
            }
            else
            {
                return MyExcelModels[modelName];
            }
        }


        #endregion

    }
}
