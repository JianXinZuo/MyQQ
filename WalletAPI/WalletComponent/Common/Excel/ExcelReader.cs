using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace WalletComponent.Common.Excel
{
    public class ExcelReader<T> where T : new()
    {
        private Dictionary<string, ColumnPropertyInfo> _columnPropertyList;
        private string _path;
        private int _sheetIndex;

        public ExcelReader(string path, int sheetIndex)
        {
            _path = path;
            _sheetIndex = sheetIndex;
            InitColumnHeadData();
        }

        public List<T> GetList()
        {
            List<T> list = new List<T>();

            using (ExcelPackage package = new ExcelPackage(new FileStream(_path, FileMode.Open)))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets[_sheetIndex];
                for (int i = sheet.Dimension.Start.Row + 1, imax = sheet.Dimension.End.Row; i < imax; i++)
                {
                    T data = new T();

                    for (int j = sheet.Dimension.Start.Column, jmax = sheet.Dimension.End.Column; j < jmax; j++)
                    {
                        if (sheet.Cells[i, j].Value == null)
                            continue;

                        string valueStr = sheet.Cells[i, j].Value.ToString();
                        string columnName = sheet.Cells[sheet.Dimension.Start.Row, j].Value.ToString();
                        SetData(data, valueStr, columnName);
                    }

                    list.Add(data);
                }
            }

            return list;
        }

        public IEnumerable<T> GetIEnumerable()
        {
            using (ExcelPackage package = new ExcelPackage(new FileStream(_path, FileMode.Open)))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets[_sheetIndex];
                for (int i = sheet.Dimension.Start.Row + 1, imax = sheet.Dimension.End.Row; i < imax; i++)
                {
                    T data = new T();

                    for (int j = sheet.Dimension.Start.Column, jmax = sheet.Dimension.End.Column; j < jmax; j++)
                    {
                        if (sheet.Cells[i, j].Value == null)
                            continue;

                        string valueStr = sheet.Cells[i, j].Value.ToString();
                        string columnName = sheet.Cells[sheet.Dimension.Start.Row, j].Value.ToString();
                        SetData(data, valueStr, columnName);
                    }

                    yield return data;
                }
            }
        }


        /// <summary>
        /// 初始化表头行数据
        /// </summary>
        protected virtual void InitColumnHeadData()
        {
            _columnPropertyList = GetObjectColumnPropertyList();
        }



        /// <summary>
        /// 获取 T 对像的所有属性
        /// </summary>
        private Dictionary<string, ColumnPropertyInfo> GetObjectColumnPropertyList()
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

                        result.Add(ci.ColumnName, colPropInfo);
                    }
                }
            }
            return result;
        }


        private void SetData(T data, string rowData, string columnName)
        {
            if (!_columnPropertyList.TryGetValue(columnName, out ColumnPropertyInfo info))
                return;


            PropertyInfo property = info.Property;

            switch (property.PropertyType.ToString())
            {
                case "System.Char":
                    char charValue = char.Parse(rowData);
                    property.SetValue(data, charValue, null);
                    break;

                case "System.String":
                    property.SetValue(data, rowData, null);
                    break;

                case "System.Int32":
                    int intValue = int.Parse(rowData);
                    property.SetValue(data, intValue, null);
                    break;

                case "System.Int64":
                    long longValue = long.Parse(rowData);
                    property.SetValue(data, longValue, null);
                    break;

                case "System.Nullable`1[System.Int32]":
                    int intNull;
                    if (!int.TryParse(rowData, out intNull))
                        property.SetValue(data, null, null);
                    else
                        property.SetValue(data, intNull, null);
                    break;

                case "System.Nullable`1[System.Int64]":
                    long longNull;
                    if (!long.TryParse(rowData, out longNull))
                        property.SetValue(data, null, null);
                    else
                        property.SetValue(data, longNull, null);
                    break;

                case "System.Single":
                    float floatValue = float.Parse(rowData);
                    property.SetValue(data, floatValue, null);
                    break;

                case "System.Nullable`1[System.Single]":
                    float floatNull;
                    if (!float.TryParse(rowData, out floatNull))
                        property.SetValue(data, null, null);
                    else
                        property.SetValue(data, floatNull, null);
                    break;

                case "System.Decimal":
                    decimal decimalValue = decimal.Parse(rowData);
                    property.SetValue(data, decimalValue, null);
                    break;

                case "System.Nullable`1[System.Decimal]":
                    decimal decimalNull;
                    if (!decimal.TryParse(rowData, out decimalNull))
                        property.SetValue(data, null, null);
                    else
                        property.SetValue(data, decimalNull, null);
                    break;

                case "System.Double":
                    double doubleValue = double.Parse(rowData);
                    property.SetValue(data, doubleValue, null);
                    break;

                case "System.Nullable`1[System.Double]":
                    double doubleNull;
                    if (!double.TryParse(rowData, out doubleNull))
                        property.SetValue(data, null, null);
                    else
                        property.SetValue(data, doubleNull, null);
                    break;

                case "System.Boolean":
                    bool boolValue = bool.Parse(rowData);
                    property.SetValue(data, boolValue, null);
                    break;

                case "System.Nullable`1[System.Boolean]":
                    bool boolNull;
                    if (!bool.TryParse(rowData, out boolNull))
                        property.SetValue(data, null, null);
                    else
                        property.SetValue(data, boolNull, null);
                    break;

                case "System.DateTime":
                    DateTime dateTime = DateTime.Parse(rowData);
                    property.SetValue(data, dateTime, null);
                    break;

                case "System.Nullable`1[System.DateTime]":
                    DateTime dateTimeNull;
                    if (!DateTime.TryParse(rowData, out dateTimeNull))
                        property.SetValue(data, null, null);
                    else
                        property.SetValue(data, dateTimeNull, null);
                    break;
            }
        }
    }
}
