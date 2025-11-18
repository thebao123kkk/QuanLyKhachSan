using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BLL
{
    public class DatPhongBLL
    {
        public static List<DatPhongViewDTO> LayPhongDaDatTheoKhach(string tenKhach)
        {
            return DatPhongDAL.LayPhongDaDatTheoKhach(tenKhach);
        }

        public static bool CheckoutPhong(string phongID)
        {
            return DatPhongDAL.CheckoutPhong(phongID);
        }

        public static List<DatPhongViewDTO> SearchBooking(string criteria, string keyword)
        {
            return DatPhongDAL.SearchBooking(criteria, keyword);
        }

        public static decimal LayTienPhong(string maDatChiTiet)
        {
            return DatPhongDAL.LayTienPhong(maDatChiTiet);
        }

        public static decimal LayTienCoc(string maDatChiTiet)
        {
            return DatPhongDAL.LayTienCoc(maDatChiTiet);
        }

        public static bool KiemTraPhongTrungLich(string phongID, DateTime newCheckoutDate, DateTime ngayNhanHienTai, string maDatChiTiet)
        {
            return DatPhongDAL.KiemTraPhongTrungLich(phongID, newCheckoutDate, ngayNhanHienTai, maDatChiTiet);
        }

        public static bool GiaHanPhong(string maDatChiTiet, DateTime newCheckoutDate, decimal tienPhongMoi)
        {
            return DatPhongDAL.GiaHanPhong(maDatChiTiet, newCheckoutDate, tienPhongMoi);
        }

        public static decimal LayGiaPhongHienTai(string phongID)
        {
            return DatPhongDAL.LayGiaPhongHienTai(phongID);
        }


    }
}
