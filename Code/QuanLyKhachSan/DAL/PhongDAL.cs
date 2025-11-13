using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO;

namespace DAL
{
    public class PhongDAL
    {
        public static List<PhongDTO> GetDanhSachPhongTrong()
        {
            List<PhongDTO> dsPhong = new List<PhongDTO>();

            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                try
                {
                    conn.Open();
                    string sql = @"
                    SELECT 
                        p.PhongID,
                        p.SoPhong,
                        p.LoaiPhongID,
                        lp.TenLoai,
                        lp.SucChua,
                        lp.GiaCoBan,
                        p.TrangThai
                    FROM Phong p
                    JOIN LoaiPhongChiTiet lp ON p.LoaiPhongID = lp.LoaiPhongID
                    WHERE p.TrangThai = N'Trống'";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PhongDTO phong = new PhongDTO
                        {
                            PhongID = reader["PhongID"].ToString(),
                            SoPhong = reader["SoPhong"].ToString(),
                            LoaiPhongID = reader["LoaiPhongID"].ToString(),
                            TenLoai = reader["TenLoai"].ToString(),
                            SucChua = (int)reader["SucChua"],
                            GiaCoBan = (decimal)reader["GiaCoBan"],
                            TrangThai = reader["TrangThai"].ToString()

                        };
                        dsPhong.Add(phong);

                    }
                    //MessageBox.Show("SQL load " + dsPhong.Count + " phòng trống", "DAL Debug");

                    return dsPhong;
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi (ví dụ: ghi log)
                    throw new Exception("Lỗi khi lấy danh sách phòng: " + ex.Message);
                }
            }
        }

        public class LoaiPhongDAL
        {
            public static List<string> GetTenLoaiPhong()
            {
                List<string> list = new List<string>();
                using (SqlConnection conn = DatabaseAccess.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT DISTINCT TenLoai FROM LoaiPhongChiTiet";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(reader["TenLoai"].ToString());
                    }
                }
                return list;
            }
        }

        public static void UpdateTrangThaiPhong(string phongId, string trangThai)
        {
            using (var conn = DatabaseAccess.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "UPDATE Phong SET TrangThai = @TrangThai WHERE PhongID = @PhongID", conn);
                cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                cmd.Parameters.AddWithValue("@PhongID", phongId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}



