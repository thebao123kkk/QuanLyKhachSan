using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace BLL
{
    public static class ThanhToanTongHopBLL
    {
        public static TongKetThanhToanDTO TinhTongKetThanhToan(
            List<RoomFullChargeDTO> danhSachPhong,
            List<ServiceUsedDTO> danhSachDichVu,
            decimal tienCoc,
            decimal daThuHoaDon)
        {
            TongKetThanhToanDTO kq = new TongKetThanhToanDTO();

            // 1. Tổng tiền phòng
            kq.TongTienPhong = 0;
            foreach (var p in danhSachPhong)
            {
                kq.TongTienPhong += p.ThanhTien;
            }

            // 2. Tổng tiền dịch vụ
            kq.TongTienDichVu = 0;
            foreach (var dv in danhSachDichVu)
            {
                kq.TongTienDichVu += dv.SoLuong * dv.DonGiaTaiThoiDiem;
            }

            // 3. Tổng trước VAT
            decimal tongTruocVAT = kq.TongTienPhong + kq.TongTienDichVu - kq.GiamGia;

            // 4. VAT = 0.8 × tổng
            kq.VAT = tongTruocVAT * 0.8m;

            // 5. Đã thanh toán
            kq.DaThanhToan = tienCoc + daThuHoaDon;

            // 6. Còn lại
            kq.ConLai = tongTruocVAT + kq.VAT - kq.DaThanhToan;

            return kq;
        }
    }
 }
