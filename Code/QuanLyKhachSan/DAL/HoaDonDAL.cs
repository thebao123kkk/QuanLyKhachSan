using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

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

        public static string TaoMaHoaDon(string maCodeBooking)
        {
            // 1. Lấy ngày in hóa đơn (MMDD)
            string ngayHoaDon = DateTime.Now.ToString("MMdd");

            // 2. Lấy phần MMDD từ mã booking "BKYYYYMMDD_xxxx"
            //    ví dụ: BK20251125_0024 → lấy "1125"
            string ngayBooking = maCodeBooking.Substring(6, 4);

            // 3. Lấy 4 số cuối của mã booking
            string last4 = maCodeBooking.Substring(maCodeBooking.Length - 4);

            // Ghép mã cuối cùng:
            string time = DateTime.Now.ToString("HHmmss");
            return $"HD{ngayHoaDon}_{ngayBooking}_{last4}_{time}";
        }

        public static void LuuHoaDon(HoaDonDTO dto)
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                string sql = @"
            INSERT INTO HoaDonThanhToan (MaHoaDon, DaThu, ConLai, NgayLap, MGGID, NhanVienID)
            VALUES (@MaHoaDon, @DaThu, @ConLai, @NgayLap, @MGGID, @NhanVienID);";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaHoaDon", dto.MaHoaDon);
                    cmd.Parameters.AddWithValue("@DaThu", dto.TienCoc + dto.SoTienThanhToanThem);
                    cmd.Parameters.AddWithValue("@ConLai", dto.ConLai);
                    cmd.Parameters.AddWithValue("@NgayLap", DateTime.Now);

                    if (dto.MGGID == null)
                        cmd.Parameters.AddWithValue("@MGGID", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@MGGID", dto.MGGID);
                    cmd.Parameters.AddWithValue("@NhanVienID", dto.NhanVienID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static decimal LayTongDaThuTheoMaDatTong(string maDatTong)
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                string sql = @"
                SELECT ISNULL(SUM(h.DaThu), 0)
                FROM HoaDonThanhToan h
                JOIN CTHD c ON h.MaHoaDon = c.MaHoaDon
                WHERE c.MaDatChiTiet IN (
                    SELECT MaDatChiTiet 
                    FROM DatPhongChiTiet 
                    WHERE MaDatTong = @MaDatTong
                )";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDatTong", maDatTong);
                    object result = cmd.ExecuteScalar();
                    return Convert.ToDecimal(result);
                }
            }

        }

        public static decimal LayConLaiGanNhat(string maDatTong)
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                string sql = @"
            SELECT TOP 1 ConLai
            FROM HoaDonThanhToan H
            JOIN CTHD C ON H.MaHoaDon = C.MaHoaDon
            JOIN DatPhongChiTiet dpct ON dpct.MaDatChiTiet = C.MaDatChiTiet
            WHERE dpct.MaDatTong = @MaDatTong
            ORDER BY H.NgayLap DESC";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDatTong", maDatTong);
                    var result = cmd.ExecuteScalar();
                    return result == null ? 0 : (decimal)result;
                }
            }
        }

    }

}
