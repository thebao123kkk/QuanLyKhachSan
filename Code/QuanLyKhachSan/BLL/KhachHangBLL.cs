using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class KhachHangBLL
    {
        public static List<KhachHangDTO> LayTatCaKhachHang()
        {
            return KhachHangDAL.LayTatCaKhachHang();
        }
        public static string LayEmailKhach(string sdt)
        {
            return KhachHangDAL.LayEmailKhach(sdt);
        }


    }
}
