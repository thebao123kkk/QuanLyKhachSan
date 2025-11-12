using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class PhongDAL
    {
        public static List<PhongDTO> GetDanhSachPhong()
        {
            List<PhongDTO> dsPhong = new List<PhongDTO>();

            using (SqlConnection conn = SqlConnectionData.Connect())
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
                        lp.GiaCoBan
                    FROM Phong p
                    JOIN LoaiPhongChiTiet lp ON p.LoaiPhongID = lp.LoaiPhongID";

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
                            GiaCoBan = (decimal)reader["GiaCoBan"]
                        };
                        dsPhong.Add(phong);
                    }

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
                using (SqlConnection conn = SqlConnectionData.Connect())
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
    }
}



