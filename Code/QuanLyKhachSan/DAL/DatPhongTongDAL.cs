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
    
    public static string LayKhachHangID(string maDatTong)
        {
            string query = @"SELECT KhachHangID FROM DatPhongTong WHERE MaDatTong = @MaDatTong";

            using (var conn = DatabaseAccess.GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaDatTong", maDatTong);

                conn.Open();
                var result = cmd.ExecuteScalar();
                return result?.ToString();
            }
        }

        public static void CapNhatTrangThaiDatPhongTong(string maDatTong)
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                string sql = @"
            UPDATE DatPhongTong
            SET TrangThai = N'Hoàn tất TT'
            WHERE MaDatTong = @MaDatTong";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDatTong", maDatTong);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static string LayTrangThai(string maDatTong)
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                string sql = "SELECT TrangThai FROM DatPhongTong WHERE MaDatTong = @MaDatTong";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDatTong", maDatTong);
                    object result = cmd.ExecuteScalar();
                    return result?.ToString() ?? "";
                }
            }
        }




    }
}
