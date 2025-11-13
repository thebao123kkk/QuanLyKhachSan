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
    }
}
