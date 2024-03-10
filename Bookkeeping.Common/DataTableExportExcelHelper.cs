using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.Common
{
    public static class DataTableExportExcelHelper
    {
        /// <summary>
        /// 数据表格导出Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="pathName"></param>
        /// <param name="columnName"></param>
        public static void DataTableExportExcel(DataTable dt, string pathName, string[] columnName)
        {
            HSSFWorkbook workBook = new HSSFWorkbook(); //导出Excel处理对象
            //创建工作表
            ISheet sheet = string.IsNullOrEmpty(dt.TableName) ? workBook.CreateSheet("Sheet1") : workBook.CreateSheet(dt.TableName);
            //在工作表中添加一行
            IRow row = sheet.CreateRow(0);
            for(int i = 0; i < columnName.Length; i++)
            {
                //在行中添加一列
                ICell cell = row.CreateCell(i);
                //设置列内容
                cell.SetCellValue(columnName[i]);
            }
            //添加数据
            for(int i = 0; i < dt.Rows.Count; i++)  //遍历DataTable行
            {
                DataRow dataRow = dt.Rows[i];
                row = sheet.CreateRow(i + 1);   //在工作表中添加一行
                for(int j = 0; j < columnName.Length; j++)
                {
                    ICell cell = row.CreateCell(j); //在行中添加一列
                    cell.SetCellValue(dataRow[j].ToString().Trim());    //设置列的内容
                }
            }
            //输出Excel
            MemoryStream ms = new MemoryStream();
            workBook.Write(ms);
            using(FileStream fs = new FileStream(pathName, FileMode.Create, FileAccess.Write))
            {
                byte[] bytes = ms.ToArray();
                fs.Write(bytes, 0, bytes.Length);
                fs.Flush();
            }
        }
    }
}
