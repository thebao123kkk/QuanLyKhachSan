using System;

namespace DTO
{
    public class DatPhongViewDTO
    {
        public string MaDatTong { get; set; }
        public string MaDatChiTiet { get; set; }
        public string MaCode { get; set; }
        public string TenKhach { get; set; }
        public string PhongID { get; set; }
        public string SoPhong { get; set; }
        public string LoaiPhong { get; set; }
        public DateTime NgayNhan { get; set; }
        public DateTime NgayTra { get; set; }
        public int SoLuongPhong { get; set; }
        public string TrangThai { get; set; }
    }
}
