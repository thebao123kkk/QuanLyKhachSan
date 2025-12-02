using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DashBoardDAL
    {
        public (int soDangO, int tongPhong) GetPhongDangO()
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                int soDangO = 0, tongPhong = 0;

                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Phong WHERE TrangThai = N'Đã nhận';", conn))
                {
                    soDangO = (int)cmd.ExecuteScalar();
                }

                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Phong;", conn))
                {
                    tongPhong = (int)cmd.ExecuteScalar();
                }

                return (soDangO, tongPhong);
            }
        }

        public int GetCheckIns(DateTime today)
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM DatPhongChiTiet WHERE NgayNhan = @today;", conn))
                {
                    cmd.Parameters.AddWithValue("@today", today);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public int GetCheckOuts(DateTime today)
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM DatPhongChiTiet WHERE NgayTra = @today;", conn))
                {
                    cmd.Parameters.AddWithValue("@today", today);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public int GetPhongBan()
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Phong WHERE TrangThai = N'Bẩn';", conn))
                {
                    return (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
