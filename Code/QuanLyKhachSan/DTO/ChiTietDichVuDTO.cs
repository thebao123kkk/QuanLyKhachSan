using System;

namespace DTO
{
    public class ChiTietDichVuDTO
    {
        public string MaChiTietDV { get; set; }
        public string MaDatChiTiet { get; set; }
        public string DichVuID { get; set; }
        public string TenDichVu { get; set; }
        public decimal SoLuong { get; set; }
        public decimal DonGiaTaiThoiDiem { get; set; }
        public decimal ThanhTien => SoLuong * DonGiaTaiThoiDiem;
        public DateTime NgaySuDung { get; set; }
    }
}
