using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class KhachHangDAL
    {
        public static List<KhachHangDTO> LayTatCaKhachHang()
        {
            var list = new List<KhachHangDTO>();
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM KhachHang";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new KhachHangDTO
                    {
                        KhachHangID = reader["KhachHangID"].ToString(),
                        HoTen = reader["HoTen"].ToString(),
                        SDT = reader["SDT"].ToString(),
                        Email = reader["Email"].ToString(),
                        CongTy = reader["CongTy"].ToString(),
                        MST = reader["MST"].ToString(),
                        DiaChi = reader["DiaChi"].ToString()
                    });
                }
            }
            return list;
        }

        public static KhachHangDTO LayThongTinKhach(string khachHangID)
        {
            KhachHangDTO dto = null;

            string query = @"
            SELECT KhachHangID, HoTen, SDT, Email, CongTy, MST, DiaChi, NgayTao
            FROM KhachHang
            WHERE KhachHangID = @ID
        ";

            using (SqlConnection conn = DatabaseAccess.GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ID", khachHangID);

                conn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        dto = new KhachHangDTO
                        {
                            KhachHangID = rd["KhachHangID"].ToString(),
                            HoTen = rd["HoTen"].ToString(),
                            SDT = rd["SDT"].ToString(),
                            Email = rd["Email"].ToString(),
                            CongTy = rd["CongTy"].ToString(),
                            MST = rd["MST"].ToString(),
                            DiaChi = rd["DiaChi"].ToString(),
                            NgayTao = Convert.ToDateTime(rd["NgayTao"])
                        };
                    }
                }
            }

            return dto;
        }

        public static string LayEmailKhach(string sdt)
        {
            string email = null;

            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                string query = "SELECT Email FROM KhachHang WHERE SDT = @sdt";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sdt", sdt);

                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null)
                    email = result.ToString();
            }

            return email;
        }

    }
}
