using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public static class MaGiamGiaDAL
    {
        public static MaGiamGiaDTO LayMaGiamGia(string ma)
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();
                string sql = @"SELECT MGGID, TuNgay, DenNgay, PhanTramGiamGia
                           FROM MaGiamGia
                           WHERE MGGID = @ma";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ma", ma);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new MaGiamGiaDTO
                        {
                            MGGID = reader.GetString(0),
                            TuNgay = reader.GetDateTime(1),
                            DenNgay = reader.GetDateTime(2),
                            PhanTramGiamGia = reader.GetInt32(3)
                        };
                    }
                }
            }

            return null; // không có mã
        }
    }

}
