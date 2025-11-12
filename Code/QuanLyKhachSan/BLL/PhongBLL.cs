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
        public static List<PhongDTO> LayDanhSachPhong()
        {
            return PhongDAL.GetDanhSachPhong();
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
