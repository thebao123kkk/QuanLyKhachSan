using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

namespace BLL
{
    public static class ExcelImporter
    {
        public static DataTable ReadFirstSheet(string filePath)
        {
            using (var workbook = new XLWorkbook(filePath))
            {
                var sheet = workbook.Worksheets.First();
                return WorksheetToDataTable(sheet);
            }
        }

     
        private static DataTable WorksheetToDataTable(IXLWorksheet worksheet)
        {
            DataTable dt = new DataTable();
            bool firstRow = true;

            foreach (IXLRow row in worksheet.RowsUsed())
            {
                if (firstRow)
                {
                    foreach (IXLCell cell in row.CellsUsed())
                        dt.Columns.Add(cell.Value.ToString());

                    firstRow = false;
                }
                else
                {
                    dt.Rows.Add(row.CellsUsed().Select(c => c.Value.ToString()).ToArray());
                }
            }

            return dt;
        }


    }

}
