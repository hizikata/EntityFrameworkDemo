using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using Microsoft.Win32;

namespace EntityFrameworkDemo.DAL
{
    public class MainService
    {
        /// <summary>
        /// 将数据导出为Excel文档
        /// </summary>
        /// <param name="employees">Employee类型List集合</param>
        /// <returns></returns>
        public bool ExportAsExcel(List<Employee> employees)
        {
            string fileName;
            IWorkbook workbook = null;
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Excel 03文档(*.xls)|*.xls|Excel 07文档(*.xlsx)|*.xlsx";
            saveDialog.FileName = DateTime.Now.ToString("yyMMdd-HHmmss");
            if (saveDialog.ShowDialog()==true)
            {
                
                fileName = saveDialog.FileName;
                if (fileName.EndsWith(".xls"))
                {
                    workbook = new HSSFWorkbook();
                }
                else
                {
                    workbook = new XSSFWorkbook();
                }
                workbook.CreateSheet("sheet1");
                ISheet sheet = workbook.GetSheet("sheet1");
                int rowNum = 0;
                sheet.CreateRow(rowNum);
                IRow rowZero = sheet.GetRow(0);
                //此处不是接口的直接实例化，而是声明一个ICell的数组
                ICell[] cells=new ICell[8];
                for (int i = 0; i < 8; i++)
                {
                    cells[i] = rowZero.CreateCell(i);
                }
                //设置标题
                cells[0].SetCellValue("ID");
                cells[1].SetCellValue("Name");
                cells[2].SetCellValue("Phone");
                cells[3].SetCellValue("Alternate");
                cells[4].SetCellValue("Education");
                cells[5].SetCellValue("Address");
                cells[6].SetCellValue("Email");
                cells[7].SetCellValue("Remark");

                foreach (var item in employees)
                {
                    sheet.CreateRow(++rowNum);
                    //新建Row
                    IRow currentRow = sheet.GetRow(rowNum);
                    //新建相应的Cells
                    for (int i = 0; i < 8; i++)
                    {
                        cells[i] = currentRow.CreateCell(i);
                    }
                    cells[0].SetCellValue(item.ID);
                    cells[1].SetCellValue(item.Name);
                    cells[2].SetCellValue(item.Phone);
                    cells[3].SetCellValue(item.Alternate);
                    cells[4].SetCellValue(item.Education);
                    cells[5].SetCellValue(item.Address);
                    cells[6].SetCellValue(item.Email);
                    cells[7].SetCellValue(item.Remark);
                }
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    workbook.Write(fs);
                    workbook.Close();
                }
                System.Diagnostics.Process.Start("explorer.exe", @"/select," + fileName);
                return true;
            }
            else
            {
                return false;
            }
            

        }
    }
}
