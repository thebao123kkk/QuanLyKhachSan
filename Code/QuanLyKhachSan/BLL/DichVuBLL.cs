using DAL;
using DTO;
using System.Collections.Generic;

namespace BLL
{
    public class DichVuBLL
    {
        public static List<DichVuPhongDTO> LayDichVuHoatDong()
        {
            return DichVuDAL.LayDichVuHoatDong();
        }

        public static string LayDonVi(string tenDichVu)
        {
            return DichVuDAL.LayDonViTheoTen(tenDichVu);
        }

    }
}
