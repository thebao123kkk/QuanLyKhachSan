using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
namespace BLL
{
    public class NVBLL
    {
        public static string LayTenNhanVien(string id)
        {
            return NVDAL.LayTenNhanVien(id);
        }

    }
}
