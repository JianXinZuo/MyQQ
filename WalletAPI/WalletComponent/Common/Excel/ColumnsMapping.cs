using System;
using System.Collections.Generic;
using System.Text;

namespace WalletComponent.Common.Excel
{
    public class ColumnsMapping
    {
        /// <summary>
        /// Excel 列头显示的值
        /// </summary>
        public string ColumnsText { get; set; }

        /// <summary>
        /// Excel 列绑定对像的属性, 可以为空
        /// </summary>
        public string ColumnsData { get; set; }

        /// <summary>
        /// Excel 列的宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Excel列的索引
        /// </summary>
        public int ColumnsIndex { get; set; }

        /// <summary>
        /// Excel列头的相关设置
        /// </summary>
        public ColumnsMapping() { }

        /// <summary>
        /// Excel列头的相关设置
        /// </summary>
        public ColumnsMapping(string colText, string colData, int width, int colIndex)
        {
            this.ColumnsText = colText;
            this.ColumnsData = colData;
            this.Width = width;
            this.ColumnsIndex = colIndex;
        }
    }
}
