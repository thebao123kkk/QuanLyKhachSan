using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class BookingBLL
    {
        // 1. Insert khách hàng
        public static string TaoKhachHang(KhachHangDTO kh)
        {
            return BookingDAL.InsertKhachHang(kh);
        }

        // 2. Insert từng phòng
        public static string TaoPhong(string soPhong, string loaiPhongID)
        {
            return BookingDAL.InsertPhong(soPhong, loaiPhongID);
        }

        // 3. Insert đặt phòng tổng
        public static string TaoDatPhongTong(
            string khId,
            string tenDaiDien,
            string sdtDaiDien,
            bool laDoan,
            decimal tienCoc,
            string ghiChu,
            string nhanVienId,
            string phongId)
        {
            return BookingDAL.InsertDatPhongTong(
                khId, tenDaiDien, sdtDaiDien, laDoan,
                tienCoc, ghiChu, nhanVienId, phongId);
        }

        // 4. Insert đặt phòng chi tiết
        public static string TaoDatPhongChiTiet(
            string maDatTong,
            DateTime ngayNhan,
            DateTime ngayTra,
            int nguoiLon,
            int treEm,
            int soLuongPhong,
            decimal vat,
            decimal thanhTien,
            string ghiChu)
        {
            return BookingDAL.InsertDatPhongChiTiet(
                maDatTong, ngayNhan, ngayTra,
                nguoiLon, treEm, soLuongPhong, vat,
                thanhTien, ghiChu);
        }

        // BLL
        


    }
}
