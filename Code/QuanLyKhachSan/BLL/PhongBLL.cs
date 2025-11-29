using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
using static DAL.PhongDAL;

namespace BLL
{
    public class PhongBLL
    {
        public static List<PhongDTO> LayDanhSachPhongTrong()
        {
            return PhongDAL.GetDanhSachPhongTrong();
        }
        public static string LayTrangThaiPhong(string phongId)
        {
            return PhongDAL.LayTrangThaiPhong(phongId);
        }

        public static void UpdateTrangThaiPhong(string phongId, string trangThai)
        {
            PhongDAL.UpdateTrangThaiPhong(phongId, trangThai);
        }

        public static List<PhongDTO> LayTatCaPhong()
        {
            return PhongDAL.LayTatCaPhong();
        }
        public static void UpdateGhiChuPhong(string phongId, string ghiChu)
        {
            PhongDAL.UpdateGhiChuPhong(phongId, ghiChu);
        }
    }
    public class LoaiPhongBLL
    {
        public static List<string> LayDanhSachLoaiPhong()
        {
            return LoaiPhongDAL.GetTenLoaiPhong();
        }

    }
}

