using System;
using System.Configuration;
using System.Data.SqlClient;

namespace DAL
{
    public static class DatabaseAccess
    {
        private const string ConnName = "QuanLyKhachSanDB"; // DÙNG CỐ ĐỊNH 1 TÊN

        /// <summary>
        /// Đọc chuỗi kết nối từ App.config
        /// </summary>
        public static string GetConnectionString()
        {
            return ConfigurationManager
                .ConnectionStrings[ConnName]
                ?.ConnectionString;
        }

        /// <summary>
        /// Tạo SqlConnection từ chuỗi cấu hình DB
        /// </summary>
        public static SqlConnection GetConnection()
        {
            string cs = GetConnectionString();

            if (string.IsNullOrWhiteSpace(cs))
                throw new Exception("Không tìm thấy cấu hình cơ sở dữ liệu (connection string trống).");

            return new SqlConnection(cs);
        }

        /// <summary>
        /// Kiểm tra xem có kết nối được DB hay không
        /// </summary>
        public static bool CanConnect()
        {
            try
            {
                string cs = GetConnectionString();

                if (string.IsNullOrWhiteSpace(cs))
                    return false;

                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
