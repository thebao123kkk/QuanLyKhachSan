using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class NVDAL
    {
        public static string LayTenNhanVien(string nhanVienID)
        {
            string sql = @"SELECT HoTen FROM NhanVien WHERE NhanVienID = @id";
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", nhanVienID);
                conn.Open();

                object result = cmd.ExecuteScalar();
                return result == null ? "" : result.ToString();
            }
        }

    }
}
