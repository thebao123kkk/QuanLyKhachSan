using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL.LoginAndPermission
{
    public class QuyenDAL
    {
        public List<string> GetQuyenByTaiKhoan(string taiKhoanID)
        {
            var list = new List<string>();

            const string sql = @"
                SELECT QuyenID 
                FROM v_TaiKhoan_Quyen 
                WHERE TaiKhoanID = @id";

            using (SqlConnection conn = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", taiKhoanID);
                conn.Open();

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader["QuyenID"].ToString());
                }
            }

            return list;
        }
    }
}
