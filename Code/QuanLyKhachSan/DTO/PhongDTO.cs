using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PhongDTO
    {
        public string PhongID { get; set; }
        public string SoPhong { get; set; }
        public string LoaiPhongID { get; set; }
        public string TenLoai { get; set; }
        public int SucChua { get; set; }
        public decimal GiaCoBan { get; set; }
        public string TrangThai { get; set; }
    }
}
