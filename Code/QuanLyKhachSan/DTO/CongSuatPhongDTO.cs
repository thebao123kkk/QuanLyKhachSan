using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CongSuatPhongDTO
    {
        public string TenLoaiPhong { get; set; }   // Tên loại phòng
        public int SoPhong { get; set; }           // Tổng số phòng thuộc loại này
        public int SoPhongDaDat { get; set; }      // Số phòng đã được đặt
        public double TiLeGopLapDay { get; set; }  // Tỷ lệ lấp đầy (phần trăm)

    }
}
