namespace DTO
{
    public class DichVuPhongDTO
    {
        public string DichVuID { get; set; }
        public string TenDichVu { get; set; }
        public decimal DonGia { get; set; }
        public string DonVi { get; set; }
        public bool HieuLuc { get; set; }
        public decimal SoLuong { get; set; }
        public decimal ThanhTien => DonGia * SoLuong;
    }
}
