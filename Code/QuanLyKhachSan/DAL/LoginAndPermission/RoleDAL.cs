using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class RoleDAL
    {
        public List<string> GetAllRoles()
        {
            List<string> roles = new List<string>();
            string sql = "SELECT TenVaiTro FROM VaiTro";

            using (SqlConnection conn = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    roles.Add(reader["TenVaiTro"].ToString());
                }
            }
            return roles;
        }

        public bool CheckRoleExists(string roleName)
        {
            string sql = "SELECT COUNT(*) FROM VaiTro WHERE TenVaiTro = @r";

            using (SqlConnection conn = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@r", roleName);
                conn.Open();

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public string GetRoleIDByName(string roleName)
        {
            const string sql = "SELECT VaiTroID FROM VaiTro WHERE TenVaiTro = @name";

            using (SqlConnection conn = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@name", roleName);
                conn.Open();

                object result = cmd.ExecuteScalar();
                return result?.ToString();
            }
        }

    }
}
