using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using BLL;
using DTO;
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

public static class PhieuTamTinhBLL
{
    public static void InPhieuTamTinh(PhieuTamTinhDTO dto)
    {
        PdfDocument pdf = new PdfDocument();
        pdf.Info.Title = "Phiếu Tạm Tính";

        PdfPage page = pdf.AddPage();
        page.Size = PdfSharp.PageSize.A4;

        XGraphics gfx = XGraphics.FromPdfPage(page);

        // Font
        XFont fontTitle = new XFont("Arial", 22, XFontStyle.Bold);
        XFont fontHotel = new XFont("Arial", 18, XFontStyle.Bold);
        XFont fontSub = new XFont("Arial", 10, XFontStyle.Regular);
        XFont fontHeader = new XFont("Arial", 11, XFontStyle.Bold);
        XFont fontText = new XFont("Arial", 11, XFontStyle.Regular);
        XFont fontBold = new XFont("Arial", 11, XFontStyle.Bold);
        XFont fontTotal = new XFont("Arial", 13, XFontStyle.Bold);

        double y = 40;

        // HEADER 2 CỘT ----------------------------------------------------
        gfx.DrawString("LUXURY HOTEL", fontHotel, XBrushes.Black, 40, y);
        gfx.DrawString("PHIẾU TẠM TÍNH", fontHotel, XBrushes.Black,
            new XRect(0, y, page.Width - 40, 20), XStringFormats.TopRight);
        y += 22;

        gfx.DrawString("Hà Nội, Việt Nam", fontSub, XBrushes.Gray, 40, y);
        gfx.DrawString("PROVISIONAL BILL", fontSub, XBrushes.Gray,
            new XRect(0, y, page.Width - 40, 20), XStringFormats.TopRight);
        y += 18;
        // Lấy 4 số cuối của mã booking
        string last4 = dto.Booking.MaCode.Substring(dto.Booking.MaCode.Length - 4);

        dto.SoPhieu = $"TMP-{dto.Booking.NgayNhan:MMdd}-{last4}";


        // Ghép thành TMP-MMDD
        gfx.DrawString("Tel: 024.3934.1234", fontSub, XBrushes.Gray, 40, y);
        gfx.DrawString($"Số: TMP-{dto.Booking.NgayNhan:MMdd}-{last4}", fontBold, XBrushes.Black,
            new XRect(0, y, page.Width - 40, 20), XStringFormats.TopRight);
        y += 25;

        DrawLine(gfx, y); y += 25;

        // THÔNG TIN KHÁCH ------------------------------------------------
        gfx.DrawString("KHÁCH HÀNG / GUEST", fontHeader, XBrushes.Black, 40, y);
        y += 22;

        gfx.DrawString(dto.TenKhach, new XFont("Arial", 12, XFontStyle.Bold),
                       XBrushes.Black, 40, y);
        y += 18;

        gfx.DrawString($"Phòng: {dto.Phong}", fontText, XBrushes.Black, 40, y); y += 18;
        gfx.DrawString($"Ngày đến: {dto.NgayDen:dd/MM/yyyy}", fontText, XBrushes.Black, 40, y); y += 18;
        gfx.DrawString($"Ngày đi: {dto.NgayDi:dd/MM/yyyy}", fontText, XBrushes.Black, 40, y); y += 18;
        gfx.DrawString($"Thu ngân: {dto.NhanVien}", fontText, XBrushes.Black, 40, y);
        y += 30;

        DrawLine(gfx, y); y += 25;

        // BẢNG CHI TIẾT ---------------------------------------------------
        double[] col = {
    40,   // STT
    80,   // Mô tả
    280,  // DVT
    320,  // SL
    400,  // Giá
    500   // Thành tiền
};


        gfx.DrawString("#", fontHeader, XBrushes.Black, col[0], y);
        gfx.DrawString("DIỄN GIẢI / DESCRIPTION", fontHeader, XBrushes.Black, col[1], y);
        gfx.DrawString("ĐVT", fontHeader, XBrushes.Black, col[2], y);
        gfx.DrawString("SL", fontHeader, XBrushes.Black, col[3], y);
        gfx.DrawString("ĐƠN GIÁ", fontHeader, XBrushes.Black, col[4], y);
        gfx.DrawString("THÀNH TIỀN", fontHeader, XBrushes.Black, col[5], y);
        y += 20;

        DrawLine(gfx, y); y += 10;

        int stt = 1;

        foreach (var item in dto.ChiTiet)
        {
            if (item == null) continue;

            string desc = item.Description ?? "";
            bool laPhong = desc.StartsWith("Tiền phòng");

            string dvt;
            if (laPhong)
                dvt = "Đêm";
            else
            {
                string tenDV = desc.Split('–', '-')[0].Trim();
                dvt = DichVuBLL.LayDonVi(tenDV);
                if (string.IsNullOrWhiteSpace(dvt)) dvt = "Suất";
            }

            decimal soLuong = item.Quantity;
            if (laPhong)
                soLuong = (dto.NgayDi - dto.NgayDen).Days;

            string slFormatted = (soLuong % 1 == 0)
                ? soLuong.ToString("0.0")
                : soLuong.ToString("0.##");

            gfx.DrawString(stt.ToString(), fontText, XBrushes.Black, col[0], y);
            gfx.DrawString(desc, fontText, XBrushes.Black, col[1], y);
            gfx.DrawString(dvt, fontText, XBrushes.Black, col[2], y);
            gfx.DrawString(slFormatted, fontText, XBrushes.Black, col[3], y);
            gfx.DrawString(item.Price.ToString("N0"), fontText, XBrushes.Black, col[4], y);
            gfx.DrawString(item.Total.ToString("N0"), fontText, XBrushes.Black, col[5], y);

            stt++;
            y += 25;
        }

        y += 10;
        DrawLine(gfx, y); y += 20;
        //TỔNG KẾT
        // ================== TỔNG KẾT ==================
        double left = 40;
        double right = page.Width - 40;

        // Subtotal
        gfx.DrawString("Cộng tiền hàng / Subtotal:", fontText, XBrushes.Black, col[3], y);
        gfx.DrawString($"{dto.TongTienHang:N0} đ", fontBold, XBrushes.Black, col[5], y);
        y += 20;

        // Giảm giá
        if(dto.TienGiamGia > 0)
            {
                gfx.DrawString($"Giảm giá ({dto.PhanTramGiamGia}%):", fontText, XBrushes.Black, col[3], y);
                gfx.DrawString($"- {dto.TienGiamGia:N0} đ", fontBold, XBrushes.Black, col[5], y);
                y += 20;
            }


        // VAT
        gfx.DrawString("VAT (8%):", fontText, XBrushes.Black, col[3], y);
        gfx.DrawString($"{dto.VAT:N0} đ", fontBold, XBrushes.Black, col[5], y);
        y += 25;

        // Line
        gfx.DrawLine(new XPen(XColors.LightGray, 0.8), left, y, right, y);
        y += 15;

        // Total
        gfx.DrawString("TỔNG CỘNG / TOTAL:", fontTotal, XBrushes.Black, col[3], y);
        decimal tongSauGiam = dto.TongTienHang - dto.TienGiamGia;
        decimal tongFinal = tongSauGiam + dto.VAT;

        gfx.DrawString($"{tongFinal:N0} đ", fontTotal, XBrushes.Black, col[5], y);

        y += 35;


        // ================== KHUNG ĐẶT CỌC + CẦN THANH TOÁN ==================
        // Chiều rộng khung vừa đẹp, không full width
        double boxLeft = col[3] - 20;        // lùi nhẹ về trái
        double boxRight = col[5] + 80;       // mở rộng nhẹ về phải
        double boxWidth = boxRight - boxLeft;
        double boxHeight = 65;

        // Khung xám nhỏ gọn
        gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(240, 240, 240)),
                          boxLeft, y, boxWidth, boxHeight);

        // Dòng 1: Deposit
        gfx.DrawString("Đã đặt cọc / Deposit:", fontText, XBrushes.Black,
                       boxLeft + 15, y + 15);
        gfx.DrawString($"({dto.DaDatCoc:N0} đ)", fontBold, XBrushes.Black,
                       boxRight - 70, y + 15);

        // Dòng 2: Balance
        gfx.DrawString("Cần thanh toán / Balance:", fontBold, XBrushes.Black,
                       boxLeft + 15, y + 38);
        gfx.DrawString($"{dto.ConLai:N0} đ", fontTotal, XBrushes.Black,
                       boxRight - 70, y + 35);

        y += boxHeight + 40;



        // ================== GHI CHÚ + CHỮ KÝ ==================
        // Ghi chú
        gfx.DrawLine(new XPen(XColors.LightGray, 0.8), left, y, right, y);
        y += 20;

        gfx.DrawString("* Phiếu này dùng để đối chiếu, không có giá trị thay thế hóa đơn GTGT.",
                       fontSub, XBrushes.Gray, left, y);
        y += 15;

        gfx.DrawString("* This is a provisional bill for reference only.",
                       fontSub, XBrushes.Gray, left, y);
        y += 40;

        // Xác nhận khách hàng - căn phải
        gfx.DrawString("Xác nhận của khách hàng", fontHeader, XBrushes.Black,
                       new XRect(0, y, page.Width - 40, 20), XStringFormats.TopRight);

        y += 30;

        // Gạch ký tên NGẮN
        double lineWidth = 120;
        gfx.DrawLine(new XPen(XColors.Gray, 0.8),
                     (page.Width - 40) - lineWidth,  // bên phải
                     y,
                     (page.Width - 40),
                     y);





        // SAVE ------------------------------------------------------------
        SaveFileDialog dlg = new SaveFileDialog();
        dlg.Filter = "PDF Files (*.pdf)|*.pdf";
        dlg.FileName = $"PhieuTamTinh_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

        if (dlg.ShowDialog() == DialogResult.OK)
        {
            pdf.Save(dlg.FileName);
            Process.Start(new ProcessStartInfo()
            {
                FileName = dlg.FileName,
                UseShellExecute = true
            });
        }
    }

    // Line mảnh luxury
    private static void DrawLine(XGraphics gfx, double y)
    {
        gfx.DrawLine(new XPen(XColors.LightGray, 0.8), 40, y, 550, y);
    }

    // Tự động xuống dòng mô tả
    private static void DrawMultiLineText(XGraphics gfx, string text, XFont font,
                                          double x, ref double y, double maxWidth)
    {
        var words = text.Split(' ');
        string line = "";

        foreach (var word in words)
        {
            string test = (line == "" ? word : line + " " + word);

            if (gfx.MeasureString(test, font).Width > maxWidth)
            {
                gfx.DrawString(line, font, XBrushes.Black, x, y);
                y += 18;
                line = word;
            }
            else
            {
                line = test;
            }
        }

        if (line != "")
            gfx.DrawString(line, font, XBrushes.Black, x, y);
    }
}
