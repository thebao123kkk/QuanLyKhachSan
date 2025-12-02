using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class StaffReportBLL
    {
        private StaffReportDAL dal = new StaffReportDAL();

        public List<StaffReportDTO> LoadStaffReport()
        {
            return dal.GetStaffReport();
        }
    }
}
