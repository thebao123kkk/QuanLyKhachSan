using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class StaffReportDAL
    {
        public List<StaffReportDTO> GetStaffReport()
        {
            List<StaffReportDTO> list = new List<StaffReportDTO>();

            string sql = @"
SELECT 
    nv.NhanVienID,
    nv.HoTen,
    ISNULL(bc.SoBooking, 0) AS SoBooking,
    ISNULL(hd.TotalRevenue, 0) AS TotalRevenue
FROM NhanVien nv
LEFT JOIN (
    SELECT 
        NhanVienID,
        COUNT(*) AS SoBooking
    FROM DatPhongTong
    GROUP BY NhanVienID
) bc ON nv.NhanVienID = bc.NhanVienID
LEFT JOIN (
    SELECT 
        NhanVienID,
        SUM(DaThu) AS TotalRevenue
    FROM HoaDonThanhToan
    GROUP BY NhanVienID
) hd ON nv.NhanVienID = hd.NhanVienID
ORDER BY nv.NhanVienID;
";

            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StaffReportDTO report = new StaffReportDTO
                        {
                            StaffId = reader["NhanVienID"].ToString(),
                            StaffName = reader["HoTen"].ToString(),
                            BookingCount = (int)reader["SoBooking"],
                            TotalRevenue = reader["TotalRevenue"] == DBNull.Value
                                            ? 0
                                            : (decimal)reader["TotalRevenue"]
                        };

                        list.Add(report);
                    }
                }
            }

            return list;
        }
    }
}
