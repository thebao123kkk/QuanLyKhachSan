public class TongKetThanhToanDTO
{
    public decimal TongTienPhong { get; set; }
    public decimal TongTienDichVu { get; set; }
    public decimal TongTienHang => TongTienPhong + TongTienDichVu;
    public int PhanTramGiamGia { get; set; }
    public decimal GiamGia { get; set; } = 0;
    public decimal VAT { get; set; }
    public decimal DaThanhToan { get; set; }
    public decimal ConLai { get; set; }
}
