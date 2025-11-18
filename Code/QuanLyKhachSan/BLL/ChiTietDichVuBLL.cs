using DAL;
using DTO;
using System.Collections.Generic;

namespace BLL
{
    public class ChiTietDichVuBLL
    {
        public static bool LuuChiTietDichVu(string maDatChiTiet, List<DichVuPhongDTO> ds)
        {
            return ChiTietDichVuDAL.LuuChiTietDichVu(maDatChiTiet, ds);
        }

        public static List<ChiTietDichVuDTO> LoadChiTietDichVu(string maDatChiTiet)
        {
            return ChiTietDichVuDAL.LoadChiTietDichVu(maDatChiTiet);
        }
    }
}
