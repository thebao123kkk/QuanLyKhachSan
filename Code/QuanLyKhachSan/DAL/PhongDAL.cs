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


        public static List<RoomFullChargeDTO> GetRoomsByBooking(string maDatTong)
        {
            List<RoomFullChargeDTO> list = new List<RoomFullChargeDTO>();

            string query = @"
        SELECT 
            dpct.MaDatChiTiet,
            dpct.SoLuongPhong,
            dpct.NgayNhan,
            dpct.NgayTra,
            lp.TenLoai AS LoaiPhong,
            lp.GiaCoBan
        FROM DatPhongChiTiet dpct
        JOIN DatPhongTong dpt ON dpct.MaDatTong = dpt.MaDatTong
        JOIN Phong p ON p.PhongID = dpt.PhongID
        JOIN LoaiPhongChiTiet lp ON lp.LoaiPhongID = p.LoaiPhongID
        WHERE dpct.MaDatTong = @MaDatTong
        ORDER BY dpct.MaDatChiTiet
    ";

            using (var conn = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaDatTong", maDatTong);

                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new RoomFullChargeDTO
                        {
                            MaDatChiTiet = rd["MaDatChiTiet"].ToString(),
                            LoaiPhong = rd["LoaiPhong"].ToString(),
                            SoLuongPhong = Convert.ToInt32(rd["SoLuongPhong"]),
                            GiaCoBan = Convert.ToDecimal(rd["GiaCoBan"]),
                            NgayNhan = Convert.ToDateTime(rd["NgayNhan"]),
                            NgayTra = Convert.ToDateTime(rd["NgayTra"])
                        });
                    }
                }
            }

            return list;
        }

        public static RoomFullChargeDTO GetRoomFullInfo(string maDatChiTiet)
        {
            RoomFullChargeDTO result = null;

            string query = @"
            SELECT 
                p.PhongID,
                lp.TenLoai,
                lp.GiaCoBan,
                dpct.SoLuongPhong,
                dpct.NgayNhan,
                dpct.NgayTra,
                dpct.ThanhTien,
                dpct.MaDatChiTiet
            FROM DatPhongChiTiet dpct
            JOIN DatPhongTong dpt ON dpct.MaDatTong = dpt.MaDatTong
            JOIN Phong p ON p.PhongID = dpt.PhongID
            JOIN LoaiPhongChiTiet lp ON lp.LoaiPhongID = p.LoaiPhongID
            WHERE dpct.MaDatChiTiet = @MaDatChiTiet
        ";

            using (var conn = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaDatChiTiet", maDatChiTiet);

                conn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        result = new RoomFullChargeDTO
                        {
                            MaDatChiTiet = rd["MaDatChiTiet"].ToString(),
                            PhongID = rd["PhongID"].ToString(),
                            LoaiPhong = rd["TenLoai"].ToString(),
                            GiaCoBan = Convert.ToDecimal(rd["GiaCoBan"]),
                            SoLuongPhong = Convert.ToInt32(rd["SoLuongPhong"]),
                            NgayNhan = Convert.ToDateTime(rd["NgayNhan"]),
                            NgayTra = Convert.ToDateTime(rd["NgayTra"]),
                            ThanhTien = Convert.ToDecimal(rd["ThanhTien"])
                        };
                    }
                }
            }

            return result;
        }


        // Lấy tất cả phòng theo tên khách
        public static List<RoomFullChargeDTO> GetAllRoomsByCustomerName(string tenKhach)
        {
            List<RoomFullChargeDTO> list = new List<RoomFullChargeDTO>();

            string query = @"
            SELECT
                dpct.MaDatChiTiet,
                dpct.SoLuongPhong,
                dpct.NgayNhan,
                dpct.NgayTra,
                lp.TenLoai AS LoaiPhong,
                lp.GiaCoBan,
                dpct.ThanhTien,
                p.PhongID
            FROM DatPhongTong dpt
            JOIN KhachHang kh ON kh.KhachHangID = dpt.KhachHangID
            JOIN DatPhongChiTiet dpct ON dpct.MaDatTong = dpt.MaDatTong
            JOIN Phong p ON p.PhongID = dpt.PhongID
            JOIN LoaiPhongChiTiet lp ON lp.LoaiPhongID = p.LoaiPhongID
            WHERE kh.HoTen = @TenKhach
        ";

            using (var conn = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TenKhach", tenKhach);

                conn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new RoomFullChargeDTO
                        {
                            MaDatChiTiet = rd["MaDatChiTiet"].ToString(),
                            PhongID = rd["PhongID"].ToString(),
                            LoaiPhong = rd["LoaiPhong"].ToString(),
                            GiaCoBan = Convert.ToDecimal(rd["GiaCoBan"]),
                            SoLuongPhong = Convert.ToInt32(rd["SoLuongPhong"]),
                            NgayNhan = Convert.ToDateTime(rd["NgayNhan"]),
                            NgayTra = Convert.ToDateTime(rd["NgayTra"]),
                            ThanhTien = Convert.ToDecimal(rd["ThanhTien"])
                        });
                    }
                }
            }

            return list;
        }



    }
}



