using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace WalletComponent.Common.Excel
{
    public class ColumnPropertyInfo
    {
        public PropertyInfo Property { get; private set; }
        public ExcelColumnInfoAttribute ExcelColumnInfo { get; private set; }
        public ColumnPropertyInfo(PropertyInfo property, ExcelColumnInfoAttribute excelColumnInfo)
        {
            Property = property;
            ExcelColumnInfo = excelColumnInfo;
        }

        public string ShowName
        {
            get
            {
                if (null == ExcelColumnInfo)
                    return Property.Name;
                else
                    return ExcelColumnInfo.ColumnName;
            }
        }

        public string PropertyName
        {
            get
            {
                return Property.Name;
            }
        }
    }
}
