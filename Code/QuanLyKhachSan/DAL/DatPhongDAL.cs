using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class DatPhongDAL
    {
        // Lấy danh sách phòng khách đã đặt
        public static List<DatPhongViewDTO> LayPhongDaDatTheoKhach(string tenKhach)
        {
            List<DatPhongViewDTO> list = new List<DatPhongViewDTO>();

            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT 
                        dpt.MaDatTong,
                        dpct.MaDatChiTiet,
                        dpt.MaCode,
                        kh.HoTen,
                        p.PhongID,
                        p.SoPhong,
                        lp.TenLoai,
                        dpct.NgayNhan,
                        dpct.NgayTra,
                        dpct.SoLuongPhong,
                        p.TrangThai
                    FROM DatPhongTong dpt
                    JOIN KhachHang kh ON kh.KhachHangID = dpt.KhachHangID
                    JOIN Phong p ON p.PhongID = dpt.PhongID
                    JOIN LoaiPhongChiTiet lp ON lp.LoaiPhongID = p.LoaiPhongID
                    JOIN DatPhongChiTiet dpct ON dpct.MaDatTong = dpt.MaDatTong
                    WHERE kh.HoTen LIKE '%' + @TenKhach + '%'
                ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@TenKhach", tenKhach);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(new DatPhongViewDTO
                    {
                        MaDatTong = rd["MaDatTong"].ToString(),
                        MaDatChiTiet = rd["MaDatChiTiet"].ToString(),
                        MaCode = rd["MaCode"].ToString(),
                        TenKhach = rd["HoTen"].ToString(),
                        PhongID = rd["PhongID"].ToString(),
                        SoPhong = rd["SoPhong"].ToString(),
                        LoaiPhong = rd["TenLoai"].ToString(),
                        NgayNhan = Convert.ToDateTime(rd["NgayNhan"]),
                        NgayTra = Convert.ToDateTime(rd["NgayTra"]),
                        SoLuongPhong = Convert.ToInt32(rd["SoLuongPhong"]),
                        TrangThai = rd["TrangThai"].ToString()
                    });
                }
                rd.Close();
            }

            return list;
        }


        // Checkout → đổi trạng thái phòng thành "Đã nhận"
        public static bool CheckoutPhong(string phongID)
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                string sql = "UPDATE Phong SET TrangThai = N'Đã nhận' WHERE PhongID = @PhongID";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@PhongID", phongID);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static List<DatPhongViewDTO> SearchBooking(string criteria, string keyword)
        {
            List<DatPhongViewDTO> list = new List<DatPhongViewDTO>();

            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                string field = "";

                // ÁNH XẠ TIÊU CHÍ TÌM KIẾM
                switch (criteria)
                {
                    case "Tên khách hàng":
                        field = "kh.HoTen";
                        break;

                    case "Số điện thoại":
                        field = "kh.SDT";
                        break;

                    case "Email":
                        field = "kh.Email";
                        break;

                    default:
                        field = "kh.HoTen"; // Mặc định tìm theo tên
                        break;
                }

                string sql = $@"
            SELECT 
                dpt.MaDatTong,
                dpct.MaDatChiTiet,
                dpt.MaCode,
                kh.HoTen,
                p.PhongID,
                p.SoPhong,
                lp.TenLoai,
                dpct.NgayNhan,
                dpct.NgayTra,
                dpct.SoLuongPhong,
                p.TrangThai
            FROM DatPhongTong dpt
            JOIN KhachHang kh ON kh.KhachHangID = dpt.KhachHangID
            JOIN Phong p ON p.PhongID = dpt.PhongID
            JOIN LoaiPhongChiTiet lp ON lp.LoaiPhongID = p.LoaiPhongID
            JOIN DatPhongChiTiet dpct ON dpct.MaDatTong = dpt.MaDatTong
            WHERE {field} LIKE '%' + @keyword + '%'
        ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@keyword", keyword);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(new DatPhongViewDTO
                    {
                        MaDatTong = rd["MaDatTong"].ToString(),
                        MaDatChiTiet = rd["MaDatChiTiet"].ToString(),
                        MaCode = rd["MaCode"].ToString(),
                        TenKhach = rd["HoTen"].ToString(),
                        PhongID = rd["PhongID"].ToString(),
                        SoPhong = rd["SoPhong"].ToString(),
                        LoaiPhong = rd["TenLoai"].ToString(),
                        NgayNhan = Convert.ToDateTime(rd["NgayNhan"]),
                        NgayTra = Convert.ToDateTime(rd["NgayTra"]),
                        SoLuongPhong = Convert.ToInt32(rd["SoLuongPhong"]),
                        TrangThai = rd["TrangThai"].ToString()
                    });
                }

                rd.Close();
            }

            return list;
        }

        public static decimal LayTienPhong(string maDatChiTiet)
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();
                string sql = @"SELECT ThanhTien FROM DatPhongChiTiet WHERE MaDatChiTiet = @id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", maDatChiTiet);

                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0;
            }
        }

        public static decimal LayTienCoc(string maDatTong)
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                string sql = @"
            SELECT TongTienCoc
            FROM DatPhongTong
            WHERE MaDatTong = @id
        ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", maDatTong);

                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0;
            }
        }

        public static bool KiemTraPhongTrungLich(string phongID, DateTime newCheckoutDate, DateTime ngayNhanHienTai, string maDatChiTietHienTai)
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                string sql = @"
            SELECT COUNT(*)
            FROM DatPhongChiTiet dpct
            JOIN DatPhongTong dpt ON dpct.MaDatTong = dpt.MaDatTong
            WHERE dpt.PhongID = @phongID
              AND dpct.MaDatChiTiet <> @currentID
              AND dpct.NgayNhan < @newCheckoutDate
              AND dpct.NgayTra > @ngayNhanHienTai
        ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@phongID", phongID);
                cmd.Parameters.AddWithValue("@newCheckoutDate", newCheckoutDate);
                cmd.Parameters.AddWithValue("@ngayNhanHienTai", ngayNhanHienTai);
                cmd.Parameters.AddWithValue("@currentID", maDatChiTietHienTai);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
        public static bool GiaHanPhong(string maDatChiTiet, DateTime newCheckoutDate, decimal tienPhongMoi)
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                string sql = @"
            UPDATE DatPhongChiTiet
            SET NgayTra = @newDate,
                ThanhTien = @newFee
            WHERE MaDatChiTiet = @id
        ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@newDate", newCheckoutDate);
                cmd.Parameters.AddWithValue("@newFee", tienPhongMoi);
                cmd.Parameters.AddWithValue("@id", maDatChiTiet);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static decimal LayGiaPhongHienTai(string phongID)
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                string sql = @"
            SELECT lp.GiaCoBan
            FROM Phong p
            JOIN LoaiPhongChiTiet lp ON lp.LoaiPhongID = p.LoaiPhongID
            WHERE p.PhongID = @id
        ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", phongID);

                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0;
            }
        }





    }
}
