using System;
using System.Data.SqlClient;

namespace DAL
{
    public class LogDAL
    {
        public void GhiLog(string taiKhoanID, string tenDangNhap, string hanhDong, string noiDung)
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GhiLog", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TaiKhoanID", taiKhoanID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap ?? "");
                cmd.Parameters.AddWithValue("@HanhDong", hanhDong ?? "");
                cmd.Parameters.AddWithValue("@NoiDung", noiDung ?? "");

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
