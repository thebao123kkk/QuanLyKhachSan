using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PhieuTamTinhDTO
    {
        public string SoPhieu { get; set; }
        public string TenKhach { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public string Phong { get; set; }
        public string LoaiPhong { get; set; }
        public DateTime NgayDen { get; set; }
        public DateTime NgayDi { get; set; }
        public string NhanVien { get; set; }
        public DatPhongViewDTO Booking { get; set; }
        public int PhanTramGiamGia { get; set; }      // % giảm từ mã MGG
        public decimal TienGiamGia { get; set; }      // Số tiền giảm sau khi tính
        public decimal GiamGia { get; set; }

        public List<InvoiceItemDTO> ChiTiet { get; set; }

        public decimal TongTienHang { get; set; }
        public decimal VAT { get; set; }
        public decimal DaDatCoc { get; set; }
   
        public decimal DaThanhToan { get; set; }
        public decimal ConLai { get; set; }
    }

}
