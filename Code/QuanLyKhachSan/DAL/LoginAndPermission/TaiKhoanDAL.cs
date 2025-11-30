// Project: DAL
using System;
using System.Data.SqlClient;
using DTO;

namespace DAL.LoginAndPermission
{
    public class TaiKhoanDAL
    {
        public TaiKhoanDTO GetByUsernameAndPassword(string username, string passwordHash)
        {
            const string sql = @"
                SELECT TaiKhoanID, TenDangNhap, MatKhauHash, VaiTroID, TrangThai,
                       Email, NhanVienID, ISNULL(Khoa, 0) AS Khoa
                FROM TaiKhoanHeThong
                WHERE TenDangNhap = @u AND MatKhauHash = @p";

            using (SqlConnection conn = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", passwordHash);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new TaiKhoanDTO
                    {
                        TaiKhoanID = reader["TaiKhoanID"].ToString(),
                        TenDangNhap = reader["TenDangNhap"].ToString(),
                        MatKhauHash = reader["MatKhauHash"].ToString(),
                        VaiTroID = reader["VaiTroID"].ToString(),
                        TrangThai = reader["TrangThai"].ToString(),
                        Email = reader["Email"].ToString(),
                        NhanVienID = reader["NhanVienID"].ToString(),
                        Khoa = Convert.ToBoolean(reader["Khoa"])
                    };
                }
            }
        }

        public TaiKhoanDTO GetByUsername(string username)
        {
            const string sql = @"
                SELECT TaiKhoanID, TenDangNhap, MatKhauHash, VaiTroID, TrangThai,
                       Email, NhanVienID, ISNULL(Khoa, 0) AS Khoa
                FROM TaiKhoanHeThong
                WHERE TenDangNhap = @u";

            using (SqlConnection conn = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@u", username);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new TaiKhoanDTO
                    {
                        TaiKhoanID = reader["TaiKhoanID"].ToString(),
                        TenDangNhap = reader["TenDangNhap"].ToString(),
                        MatKhauHash = reader["MatKhauHash"].ToString(),
                        VaiTroID = reader["VaiTroID"].ToString(),
                        TrangThai = reader["TrangThai"].ToString(),
                        Email = reader["Email"].ToString(),
                        NhanVienID = reader["NhanVienID"].ToString(),
                        Khoa = Convert.ToBoolean(reader["Khoa"])
                    };
                }
            }
        }

      
    }
}
