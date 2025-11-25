using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class HoaDonDAL
    {
        public static decimal LayDaThuTheoMaDatChiTiet(string maDatChiTiet)
        {
            string query = @"
            SELECT ISNULL(SUM(hd.DaThu), 0)
            FROM CTHD c
            JOIN HoaDonThanhToan hd ON hd.MaHoaDon = c.MaHoaDon
            WHERE c.MaDatChiTiet = @MaDatChiTiet
        ";

            using (var conn = DatabaseAccess.GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaDatChiTiet", maDatChiTiet);
                conn.Open();
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }
    }

}
