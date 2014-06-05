using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Net.SourceForge.Koogra.Excel;

namespace LotteryApp
{
    public class ExcelUtils
    {
        //头部的行数
        public static uint headRowCount = 0;
        //尾部的行数
        public static uint endRowCount = 0;
        //数据的行数
        public static uint dataRowCount = 0;
        //数据的列数
        public static uint dataColCount = 0;

        //DataTable存放excel读取到的数据
        public static DataTable dtExcel;
        //日期格式数据的列号，用逗号分开。
        public static string indexDatatimeCol;

        private Workbook book;

        public ExcelUtils(string path)
        {
            this.book = new Workbook(path);
        }

        public ExcelUtils(System.IO.Stream stream)
        {
            this.book = new Workbook(stream);
        }

        protected DataTable SaveAsDataTable(Worksheet sheet)
        {
            uint minRow = sheet.Rows.MinRow + headRowCount;
            uint maxRow = sheet.Rows.MaxRow - endRowCount;

            //如果确定了需要导入的数据行数
            if (dataRowCount != 0)
            {
                maxRow = minRow + dataRowCount - 1;
            }

            Row firstRow = sheet.Rows[minRow];

            uint minCol = firstRow.Cells.MinCol;
            uint maxCol = firstRow.Cells.MaxCol;
            if (dataColCount != 0)
            {
                maxCol = minCol + dataColCount - 1;
            }

            //for (uint i = minCol; i <= maxCol; i++)
            //{
            //    dt.Columns.Add(firstRow.Cells[i].FormattedValue());
            //}

            string[] indexCol = indexDatatimeCol.Split(',');

            for (uint i = minRow; i <= maxRow; i++)
            {
                Row row = sheet.Rows[i];
                if (row != null)
                {
                    DataRow dr = dtExcel.NewRow();
                    if (row.Cells[0] == null)
                    {
                        continue;
                    }

                    bool isDate;
                    for (uint j = minCol; j <= maxCol; j++)
                    {
                        Cell cell = row.Cells[j];

                        isDate = false;
                        if (cell != null)
                        {
                            //判断此列是否是日期格式数据
                            foreach (string col in indexCol)
                            {
                                if (string.IsNullOrEmpty(col))
                                {
                                    continue;
                                }
                                else
                                {
                                    if (j == int.Parse(col))
                                    {
                                        isDate = true;
                                        break;
                                    }
                                }
                            }
                            if (isDate)
                            {   //如果此列是日期格式数据，进行转换。
                                dr[Convert.ToInt32(j)] = cell.Value != null ? ParseDateTime(cell.Value.ToString()).ToString("yyyy-MM-dd HH:mm") : string.Empty;
                            }
                            else
                            {
                                dr[Convert.ToInt32(j)] = cell.Value != null ? cell.Value.ToString().Trim('↑').Trim('↓').Trim('→') : string.Empty;
                            }
                        }
                    }

                    dtExcel.Rows.Add(dr);
                }

            }

            return dtExcel;
        }

        public DataTable ToDataTable(int index)
        {
            Worksheet sheet = this.book.Sheets[index];

            if (sheet == null)
            {
                throw new ApplicationException(string.Format("索引[{0}]所指定的电子表格不存在！", index));
            }

            return this.SaveAsDataTable(sheet);
        }

        public DataTable ToDataTable(string sheetName)
        {
            Worksheet sheet = this.book.Sheets.GetByName(sheetName);

            if (sheet == null)
            {
                throw new ApplicationException(string.Format("名称[{0}]所指定的电子表格不存在！", sheetName));
            }

            return this.SaveAsDataTable(sheet);
        }

        #region 静态方法



        /// <summary>
        /// 单元格格式为日期时间，使用此方法转换为DateTime类型，若解析失败则返回‘0001-01-01’
        /// </summary>
        public static DateTime ParseDateTime(string cellValue)
        {
            DateTime date = default(DateTime);

            double value = default(double);

            if (double.TryParse(cellValue, out value))
            {
                date = DateTime.FromOADate(value);
            }
            else
            {
                DateTime.TryParse(cellValue, out date);
            }

            return date;
        }

        /// <summary>
        /// 转换为DataTable(文件路径+表名)
        /// </summary>
        public static DataTable TranslateToTable(string path, string sheetName)
        {
            ExcelUtils utils = new ExcelUtils(path);
            return utils.ToDataTable(sheetName);
        }

        /// <summary>
        /// 转换为DataTable(文件路径+表索引)
        /// </summary>
        public static DataTable TranslateToTable(string path, int sheetIndex)
        {
            ExcelUtils utils = new ExcelUtils(path);
            return utils.ToDataTable(sheetIndex);
        }

        /// <summary>
        /// 转换为DataTable(文件路径)
        /// </summary>
        public static DataTable TranslateToTable(string path)
        {
            ExcelUtils utils = new ExcelUtils(path);
            return utils.ToDataTable(0);
        }

        /// <summary>
        /// 转换为DataTable(内存流+表名)
        /// </summary>
        public static DataTable TranslateToTable(System.IO.Stream stream, string sheetName)
        {
            ExcelUtils utils = new ExcelUtils(stream);
            return utils.ToDataTable(sheetName);
        }

        /// <summary>
        /// 转换为DataTable(内存流+表索引)
        /// </summary>
        public static DataTable TranslateToTable(System.IO.Stream stream, int sheetIndex)
        {
            ExcelUtils utils = new ExcelUtils(stream);
            return utils.ToDataTable(sheetIndex);
        }

        /// <summary>
        /// 转换为DataTable(内存流)
        /// </summary>
        public static DataTable TranslateToTable(System.IO.Stream stream)
        {
            ExcelUtils utils = new ExcelUtils(stream);
            return utils.ToDataTable(0);
        }

        /// <summary>
        /// 获取sheets数量
        /// </summary>
        public static int GetSheetsCount(string path)
        { 
            ExcelUtils utils = new ExcelUtils(path);
            return utils.book.Sheets.Count;
        }
        #endregion

    }
}
