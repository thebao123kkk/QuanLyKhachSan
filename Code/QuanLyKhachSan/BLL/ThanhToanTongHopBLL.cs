using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public static class ThanhToanTongHopBLL
    {
        public static TongKetThanhToanDTO TinhTongKetThanhToan(
            List<RoomFullChargeDTO> danhSachPhong,
            List<ServiceUsedDTO> danhSachDichVu,
            decimal tienCoc,
            decimal daThuHoaDon,
            int phanTramGiamGia = 0,
            int TienGiamGia = 0
        )
        {
            var kq = new TongKetThanhToanDTO();
            
            // 1. Tổng tiền phòng
            kq.TongTienPhong = danhSachPhong.Sum(p => p.ThanhTien);

            // 2. Tổng tiền dịch vụ
            kq.TongTienDichVu = danhSachDichVu.Sum(dv => dv.SoLuong * dv.DonGiaTaiThoiDiem);

            // 3. Tổng tiền hàng
            decimal tongHang = kq.TongTienPhong + kq.TongTienDichVu;

            // 4. Giảm giá
            decimal tienGiamGia = tongHang * phanTramGiamGia / 100m;
            kq.GiamGia = tienGiamGia;
            kq.PhanTramGiamGia = phanTramGiamGia;

            if (phanTramGiamGia > 0)
            {
                kq.GiamGia = tongHang * phanTramGiamGia / 100m;
                kq.PhanTramGiamGia = phanTramGiamGia;
            }

            // 5. Tiền tính VAT
            decimal baseVAT = tongHang - kq.GiamGia;

            // 6. VAT 8%
            kq.VAT = baseVAT * 0.08m;

            // 7. Đã thanh toán
            kq.DaThanhToan = tienCoc + daThuHoaDon;

            // 8. Còn lại
            kq.ConLai = baseVAT + kq.VAT - kq.DaThanhToan;

            return kq;
        }

        public static decimal LayTongDaThu(string maDatTong)
    => HoaDonDAL.LayTongDaThuTheoMaDatTong(maDatTong);

        public static decimal LayConLai(string maDatTong)
            => HoaDonDAL.LayConLaiGanNhat(maDatTong);

    }
}
