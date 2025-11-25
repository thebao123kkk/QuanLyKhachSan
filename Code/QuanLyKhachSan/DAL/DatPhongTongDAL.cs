using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class DatPhongTongDAL
    {
        public static decimal LayTienCoc(string maDatTong)
        {
            string query = @"
            SELECT ISNULL(TongTienCoc, 0)
            FROM DatPhongTong
            WHERE MaDatTong = @MaDatTong
        ";

            using (var conn = DatabaseAccess.GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaDatTong", maDatTong);
                conn.Open();
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }
    }

}
