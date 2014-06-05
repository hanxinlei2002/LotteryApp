using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Net.SourceForge.Koogra.Excel;

namespace LotteryApp
{
    public class ExcelUtils
    {
        //ͷ��������
        public static uint headRowCount = 0;
        //β��������
        public static uint endRowCount = 0;
        //���ݵ�����
        public static uint dataRowCount = 0;
        //���ݵ�����
        public static uint dataColCount = 0;

        //DataTable���excel��ȡ��������
        public static DataTable dtExcel;
        //���ڸ�ʽ���ݵ��кţ��ö��ŷֿ���
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

            //���ȷ������Ҫ�������������
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
                            //�жϴ����Ƿ������ڸ�ʽ����
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
                            {   //������������ڸ�ʽ���ݣ�����ת����
                                dr[Convert.ToInt32(j)] = cell.Value != null ? ParseDateTime(cell.Value.ToString()).ToString("yyyy-MM-dd HH:mm") : string.Empty;
                            }
                            else
                            {
                                dr[Convert.ToInt32(j)] = cell.Value != null ? cell.Value.ToString().Trim('��').Trim('��').Trim('��') : string.Empty;
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
                throw new ApplicationException(string.Format("����[{0}]��ָ���ĵ��ӱ�񲻴��ڣ�", index));
            }

            return this.SaveAsDataTable(sheet);
        }

        public DataTable ToDataTable(string sheetName)
        {
            Worksheet sheet = this.book.Sheets.GetByName(sheetName);

            if (sheet == null)
            {
                throw new ApplicationException(string.Format("����[{0}]��ָ���ĵ��ӱ�񲻴��ڣ�", sheetName));
            }

            return this.SaveAsDataTable(sheet);
        }

        #region ��̬����



        /// <summary>
        /// ��Ԫ���ʽΪ����ʱ�䣬ʹ�ô˷���ת��ΪDateTime���ͣ�������ʧ���򷵻ء�0001-01-01��
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
        /// ת��ΪDataTable(�ļ�·��+����)
        /// </summary>
        public static DataTable TranslateToTable(string path, string sheetName)
        {
            ExcelUtils utils = new ExcelUtils(path);
            return utils.ToDataTable(sheetName);
        }

        /// <summary>
        /// ת��ΪDataTable(�ļ�·��+������)
        /// </summary>
        public static DataTable TranslateToTable(string path, int sheetIndex)
        {
            ExcelUtils utils = new ExcelUtils(path);
            return utils.ToDataTable(sheetIndex);
        }

        /// <summary>
        /// ת��ΪDataTable(�ļ�·��)
        /// </summary>
        public static DataTable TranslateToTable(string path)
        {
            ExcelUtils utils = new ExcelUtils(path);
            return utils.ToDataTable(0);
        }

        /// <summary>
        /// ת��ΪDataTable(�ڴ���+����)
        /// </summary>
        public static DataTable TranslateToTable(System.IO.Stream stream, string sheetName)
        {
            ExcelUtils utils = new ExcelUtils(stream);
            return utils.ToDataTable(sheetName);
        }

        /// <summary>
        /// ת��ΪDataTable(�ڴ���+������)
        /// </summary>
        public static DataTable TranslateToTable(System.IO.Stream stream, int sheetIndex)
        {
            ExcelUtils utils = new ExcelUtils(stream);
            return utils.ToDataTable(sheetIndex);
        }

        /// <summary>
        /// ת��ΪDataTable(�ڴ���)
        /// </summary>
        public static DataTable TranslateToTable(System.IO.Stream stream)
        {
            ExcelUtils utils = new ExcelUtils(stream);
            return utils.ToDataTable(0);
        }

        /// <summary>
        /// ��ȡsheets����
        /// </summary>
        public static int GetSheetsCount(string path)
        { 
            ExcelUtils utils = new ExcelUtils(path);
            return utils.book.Sheets.Count;
        }
        #endregion

    }
}
