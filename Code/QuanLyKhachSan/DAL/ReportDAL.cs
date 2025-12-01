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
        public static List<CongSuatPhongDTO> LayCongSuatTheoLoaiPhong()
        {
            List<CongSuatPhongDTO> list = new List<CongSuatPhongDTO>();
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();
                string sql = @"
            WITH Tong AS (
                SELECT lp.TenLoai AS TenLoaiPhong,
                       COUNT(p.PhongID) AS SoPhong,
                       SUM(CASE WHEN dpt.MaDatTong IS NOT NULL THEN 1 ELSE 0 END) AS SoPhongDaDat
                FROM Phong p
                JOIN LoaiPhongChiTiet lp ON p.LoaiPhongID = lp.LoaiPhongID
                LEFT JOIN DatPhongTong dpt ON dpt.PhongID = p.PhongID AND dpt.TrangThai != N'Hủy'
                GROUP BY lp.TenLoai
            )
            SELECT *,
                   CAST(1.0 * SoPhongDaDat / NULLIF((SELECT SUM(SoPhongDaDat) FROM Tong), 0) * 100 AS DECIMAL(5,2)) AS TiLeGopLapDay
            FROM Tong
            ORDER BY TiLeGopLapDay DESC;";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new CongSuatPhongDTO
                        {
                            TenLoaiPhong = reader["TenLoaiPhong"].ToString(),
                            SoPhong = Convert.ToInt32(reader["SoPhong"]),
                            SoPhongDaDat = Convert.ToInt32(reader["SoPhongDaDat"]),
                            TiLeGopLapDay = Convert.ToDouble(reader["TiLeGopLapDay"])
                        });
                    }
                }
            }
            return list;
        }

        public static List<DoanhThuNgayDTO> LayDoanhThuTheoNgay(DateTime? start, DateTime? end)
        {
            List<DoanhThuNgayDTO> list = new List<DoanhThuNgayDTO>();
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();
                string sql = @"
            SELECT CAST(NgayLap AS DATE) AS Ngay, SUM(DaThu) AS TongDoanhThu
            FROM HoaDonThanhToan
            WHERE (@start IS NULL OR NgayLap >= @start)
              AND (@end IS NULL OR NgayLap <= @end)
            GROUP BY CAST(NgayLap AS DATE)
            ORDER BY Ngay";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@start", (object)start ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@end", (object)end ?? DBNull.Value);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new DoanhThuNgayDTO
                        {
                            Ngay = reader.GetDateTime(0),
                            TongDoanhThu = reader.GetDecimal(1)
                        });
                    }
                }
            }
            return list;
        }


    }
}
