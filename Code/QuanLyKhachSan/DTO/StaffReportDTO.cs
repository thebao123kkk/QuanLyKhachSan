using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class StaffReportDTO
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public int BookingCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
