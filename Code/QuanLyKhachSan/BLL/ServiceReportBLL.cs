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
    }
}
