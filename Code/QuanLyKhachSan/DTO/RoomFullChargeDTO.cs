using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DTO
{
    public class RoomFullChargeDTO
    {
        public string MaDatChiTiet { get; set; }
        public string PhongID { get; set; }
        public string LoaiPhong { get; set; }
        public decimal GiaCoBan { get; set; }
        public int SoLuongPhong { get; set; }
        public DateTime NgayNhan { get; set; }
        public DateTime NgayTra { get; set; }
        public decimal ThanhTien { get; set; }

        public int SoDem => (NgayTra - NgayNhan).Days;
        
    }
}