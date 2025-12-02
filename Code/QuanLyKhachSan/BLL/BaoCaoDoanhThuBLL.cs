using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using ClosedXML.Excel;
using DTO;
using Microsoft.Win32;

public class BaoCaoDoanhThuBLL
{
    public static void XuatExcelDoanhThu(List<DoanhThuNgayDTO> data)
    {
        if (data == null || data.Count == 0)
        {
            MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo");
            return;
        }

        using (var wb = new XLWorkbook())
        {
            var ws = wb.Worksheets.Add("DoanhThuNgay");

            ws.Cell(1, 1).Value = "Ngày";
            ws.Cell(1, 2).Value = "Doanh thu trong ngày";
            ws.Range(1, 1, 1, 2).Style.Font.Bold = true;
            ws.Range(1, 1, 1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;
            decimal total = 0;
            foreach (var item in data)
            {
                ws.Cell(row, 1).Value = item.Ngay.ToString("dd/MM/yyyy");
                ws.Cell(row, 2).Value = item.TongDoanhThu;
                ws.Cell(row, 2).Style.NumberFormat.Format = "#,##0";
                total += item.TongDoanhThu;
                row++;
            }

            ws.Cell(row, 1).Value = "Tổng cộng";
            ws.Cell(row, 2).Value = total;
            ws.Cell(row, 1).Style.Font.Bold = true;
            ws.Cell(row, 2).Style.Font.Bold = true;
            ws.Cell(row, 2).Style.NumberFormat.Format = "#,##0";

            ws.Range(1, 1, row, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range(1, 1, row, 2).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            ws.Range(1, 1, row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Columns().AdjustToContents();

            var dialog = new SaveFileDialog
            {
                FileName = "BaoCaoDoanhThu.xlsx",
                Filter = "Excel Files (*.xlsx)|*.xlsx"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                wb.SaveAs(dialog.FileName);
                MessageBox.Show("Xuất Excel thành công!", "Thông báo");
            }
        }
    }
}
