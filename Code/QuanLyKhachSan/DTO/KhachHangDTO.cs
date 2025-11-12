using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class KhachHangDTO
    {
        public string KhachHangID { get; set; }
        public string HoTen { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string CongTy { get; set; }
        public string MST { get; set; }
        public string DiaChi { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
