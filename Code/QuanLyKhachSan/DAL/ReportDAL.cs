using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class ReportDAL
    {
        public static List<TopServiceDTO> LayTopDichVu()
        {
            List<TopServiceDTO> list = new List<TopServiceDTO>();

            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                string sql = @"
SELECT 
    dv.TenDichVu,
    SUM(ISNULL(ct.SoLuong, 0)) AS TongSoLan,
    SUM(ISNULL(ct.SoLuong, 0) * ISNULL(ct.DonGiaTaiThoiDiem, 0)) AS DoanhThu
FROM ChiTietDichVu ct
JOIN DichVuPhong dv ON ct.DichVuID = dv.DichVuID
GROUP BY dv.TenDichVu
ORDER BY DoanhThu DESC;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new TopServiceDTO
                        {
                            TenDichVu = reader["TenDichVu"].ToString(),
                            TongSoLan = Convert.ToInt32(reader["TongSoLan"]),
                            DoanhThu = Convert.ToDecimal(reader["DoanhThu"])
                        });
                    }
                }
            }

            return list;
        }

    }
}
