using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DashBoard
{
    public class HienThiDAL
    {
        public string[] GetUserNameAndRoleName(string username, string vaitroid)
        {
            const string sql = @"
                SELECT nv.HoTen, vt.TenVaiTro
                FROM NhanVien nv
                JOIN TaiKhoanHeThong tk ON nv.NhanVienID = tk.NhanVienID
                JOIN VaiTro vt ON vt.VaiTroID = tk.VaiTroID
                WHERE tk.TenDangNhap = @u AND vt.VaiTroID = @vaitroid";

            using (SqlConnection conn = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@vaitroid", vaitroid);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string hoTen = reader.GetString(0);     
                        string tenVaiTro = reader.GetString(1);
                        return new string[] { hoTen, tenVaiTro };
                    }
                }
            }

            return null; 
        }

    }
}
