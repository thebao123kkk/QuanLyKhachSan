using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public static class MaGiamGiaBLL
    {
        public static MaGiamGiaDTO KiemTraMa(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma))
                return null;

            var mgg = MaGiamGiaDAL.LayMaGiamGia(ma.Trim());
            if (mgg == null)
                return null;

            DateTime now = DateTime.Now;

            if (now < mgg.TuNgay || now > mgg.DenNgay)
                return null;

            return mgg;
        }

        public static decimal TinhGiamGia(decimal tongPhong, decimal tongDV, int phanTram)
        {
            decimal tong = tongPhong + tongDV;
            return tong * phanTram / 100;
        }
    }

}
