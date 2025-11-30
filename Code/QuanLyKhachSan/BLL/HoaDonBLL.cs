using System;
using System.Diagnostics;
using System.Windows.Forms;
using BLL;
using DAL;
using DTO;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

public static class HoaDonBLL
{
    public static decimal LayTongDaThuTheoMaDatTong(string maDatTong)
    {
        return HoaDonDAL.LayTongDaThuTheoMaDatTong(maDatTong);
    }

    public static void LuuHoaDon(HoaDonDTO dto)
    {
        HoaDonDAL.LuuHoaDon(dto);
    }
    public static void InHoaDon(HoaDonDTO dto)
    {
        var booking = dto.Booking;

        PdfDocument pdf = new PdfDocument();
        pdf.Info.Title = "Hóa đơn thanh toán";

        PdfPage page = pdf.AddPage();
        page.Size = PdfSharp.PageSize.A4;

        XGraphics gfx = XGraphics.FromPdfPage(page);

        // Font giống phiếu tạm tính
        XFont fontHotel = new XFont("Arial", 18, XFontStyle.Bold);
        XFont fontTitle = new XFont("Arial", 16, XFontStyle.Bold);
        XFont fontHeader = new XFont("Arial", 11, XFontStyle.Bold);
        XFont fontText = new XFont("Arial", 11, XFontStyle.Regular);
        XFont fontBold = new XFont("Arial", 11, XFontStyle.Bold);
        XFont fontSub = new XFont("Arial", 9, XFontStyle.Regular);
        XFont fontBoldSmall = new XFont("Arial", 14, XFontStyle.Bold);
        double y = 40;
        double left = 40;
        double right = page.Width - 40;

        // ========== HEADER ==========
        gfx.DrawString("LUXURY HOTEL", fontHotel, XBrushes.Black, left, y);
        y += 25;
        gfx.DrawString("HÓA ĐƠN THANH TOÁN / INVOICE", fontTitle, XBrushes.Black, left, y);
        y += 25;

        gfx.DrawString("Hà Nội, Việt Nam", fontSub, XBrushes.Gray, left, y);
        y += 18;

        // Số HĐ: dùng TMP như phiếu tạm tính cho đồng bộ
        string last4 = booking.MaCode.Substring(booking.MaCode.Length - 4);
        string soHoaDon = $"TMP-{booking.NgayNhan:MMdd}-{last4}";
        gfx.DrawString($"Số HĐ: {soHoaDon}", fontBold, XBrushes.Black,
            new XRect(0, y, page.Width - 40, 20), XStringFormats.TopRight);
        y += 25;

        DrawLine(gfx, y); y += 20;

        // ========== THÔNG TIN KHÁCH ==========
        gfx.DrawString("THÔNG TIN KHÁCH HÀNG / CUSTOMER DETAILS", fontHeader, XBrushes.Black, left, y);
        y += 22;

        gfx.DrawString($"Khách hàng: {dto.TenKhach}", fontText, XBrushes.Black, left, y); y += 18;
        gfx.DrawString($"Địa chỉ: {dto.DiaChi}", fontText, XBrushes.Black, left, y); y += 18;
        gfx.DrawString($"SĐT: {dto.SDT}", fontText, XBrushes.Black, left, y); y += 25;

        // ========== THÔNG TIN LƯU TRÚ ==========
        gfx.DrawString("THÔNG TIN LƯU TRÚ / STAY DETAILS", fontHeader, XBrushes.Black, left, y);
        y += 22;

        gfx.DrawString($"Phòng: {dto.Phong}", fontText, XBrushes.Black, left, y); y += 18;
        gfx.DrawString($"Ngày đến: {dto.NgayDen:dd/MM/yyyy}", fontText, XBrushes.Black, left, y); y += 18;
        gfx.DrawString($"Ngày đi: {dto.NgayDi:dd/MM/yyyy}", fontText, XBrushes.Black, left, y); y += 18;
        gfx.DrawString($"Thu ngân: {dto.NhanVien}", fontText, XBrushes.Black, left, y); y += 25;

        DrawLine(gfx, y); y += 15;

        // ========== BẢNG CHI TIẾT ==========
        double[] col = {
                            40,   // STT
    80,   // Mô tả
    280,  // DVT
    320,  // SL
    400,  // Giá
    500   // Thành tiền
                        };


        gfx.DrawString("STT", fontHeader, XBrushes.Black, col[0], y);
        gfx.DrawString("Diễn giải", fontHeader, XBrushes.Black, col[1], y);
        gfx.DrawString("ĐVT", fontHeader, XBrushes.Black, col[2], y);
        gfx.DrawString("SL", fontHeader, XBrushes.Black, col[3], y);
        gfx.DrawString("Đơn giá", fontHeader, XBrushes.Black, col[4], y);
        gfx.DrawString("Thành tiền", fontHeader, XBrushes.Black, col[5], y);

        y += 25;
        DrawLine(gfx, y);
        y += 15;


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

        // ================== TỔNG KẾT CÓ GIẢM GIÁ ==================

        decimal tienHang = dto.TongTienHang;
        decimal tienGiam = dto.GiamGia;           // từ DTO
        decimal vat = (tienHang - tienGiam) * 0.08m;  // VAT sau giảm giá

        decimal tongCong = tienHang - tienGiam + vat;

        decimal daThanhToan = dto.TienCoc + dto.SoTienThanhToanThem;
        decimal conLai = tongCong - daThanhToan;

        y += 25;

        // CỘNG TIỀN HÀNG
        gfx.DrawString("Cộng tiền hàng / Subtotal:", fontText, XBrushes.Black, col[4] - 80, y);
        gfx.DrawString($"{tienHang:N0} đ", fontBold, XBrushes.Black, col[5], y);
        y += 22;

        // GIẢM GIÁ
        if (dto.PhanTramGiamGia > 0)
        {
            gfx.DrawString($"Giảm giá ({dto.PhanTramGiamGia}%):", fontText, XBrushes.Black, col[3], y);
            gfx.DrawString("-" + dto.GiamGia.ToString("N0") + " đ", fontBold, XBrushes.Black, col[5], y);
            y += 22;
        }


        // VAT
        gfx.DrawString("VAT (8%):", fontText, XBrushes.Black, col[4] - 80, y);
        gfx.DrawString($"{vat:N0} đ", fontBold, XBrushes.Black, col[5], y);
        y += 28;

        // TỔNG CỘNG
        gfx.DrawString("TỔNG CỘNG:", fontBoldSmall, XBrushes.Black, col[4] - 80, y);
        gfx.DrawString($"{tongCong:N0} đ", fontBoldSmall, XBrushes.Black, col[5], y);
        y += 35;

        // ĐÃ THANH TOÁN
        gfx.DrawString("Đã thanh toán / Paid:", fontBold, XBrushes.Black, col[4] - 80, y);
        gfx.DrawString($"{daThanhToan:N0} đ", fontBold, XBrushes.Black, col[5], y);
        y += 10;

        // Ghi chú cọc + thanh toán thêm
        if (dto.SoTienThanhToanThem != 0 || dto.TienCoc != 0)
        {
            string ghiChu = $"(Cọc: {dto.TienCoc:N0}đ ; Thanh toán: {dto.SoTienThanhToanThem:N0}đ)";
            gfx.DrawString(ghiChu, fontSub, XBrushes.Gray, col[5] - 10, y, XStringFormats.TopRight);
            y += 20;
        }
        y += 20;

        // CÒN LẠI
        gfx.DrawString("Còn lại / Balance Due:", fontBoldSmall, XBrushes.Black, col[4] - 80, y);
        gfx.DrawString($"{conLai:N0} đ", fontBoldSmall, XBrushes.Black, col[5], y);
        y += 40;


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

        // SAVE
        SaveFileDialog dlg = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            FileName = $"HoaDon_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
        };

        if (dlg.ShowDialog() == DialogResult.OK)
        {
            pdf.Save(dlg.FileName);
            Process.Start(new ProcessStartInfo
            {
                FileName = dlg.FileName,
                UseShellExecute = true
            });
        }
    }

    private static void DrawLine(XGraphics gfx, double y)
    {
        gfx.DrawLine(new XPen(XColors.LightGray, 0.8), 40, y, 550, y);
    }
}
