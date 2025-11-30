using DTO;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class ServiceReportBLL
    {
        public static List<TopServiceDTO> GetTopService()
        {
            return ReportDAL.LayTopDichVu();
        }

        public static List<CongSuatPhongDTO> GetCongSuatLoaiPhong()
        {
            return ReportDAL.LayCongSuatTheoLoaiPhong();
        }

        public static List<DoanhThuNgayDTO> LayDoanhThuTheoNgay(DateTime? start, DateTime? end)
        {
            return ReportDAL.LayDoanhThuTheoNgay(start, end);
        }

    }
}
