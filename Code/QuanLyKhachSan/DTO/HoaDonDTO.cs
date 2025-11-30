using System;
using System.Collections.Generic;

namespace DTO
{
    public class HoaDonDTO
    {
        public DatPhongViewDTO Booking { get; set; }

        // Thông tin khách + lưu trú
        public string TenKhach { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
        public string Phong { get; set; }
        public DateTime NgayDen { get; set; }
        public DateTime NgayDi { get; set; }
        public string NhanVien { get; set; }

        // Chi tiết dòng hóa đơn (dùng lại InvoiceItemDTO)
        public List<InvoiceItemDTO> ChiTiet { get; set; }

        // Tổng hợp
        public string MaHoaDon { get; set; }
        public string MGGID { get; set; }
        public decimal TongTienHang { get; set; }
        public decimal VAT { get; set; }           // 8%
        public decimal TienCoc { get; set; }       // Đặt cọc
        public decimal SoTienThanhToanThem { get; set; }  // TextBox SoTienThanhToantb
        public decimal ConLai { get; set; }
        public int PhanTramGiamGia { get; set; }      // % giảm từ mã MGG
        public decimal TienGiamGia { get; set; }      // Số tiền giảm sau khi tính
        public decimal GiamGia { get; set; }

    }
}
