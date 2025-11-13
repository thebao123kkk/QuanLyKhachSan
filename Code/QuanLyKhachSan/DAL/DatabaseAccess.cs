using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    //public class SqlConnectionData
    //{
    //    public static SqlConnection Connect()
    //    {
    //        // Chuỗi kết nối đến SQL Server (nhớ đổi khi đổi máy)
    //        string strcon = "Data Source=.\\SQLEXPRESS;Initial Catalog=QuanLyKhachSan;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
    //        SqlConnection conn = new SqlConnection(strcon);
    //        return conn;
    //    }
    //}
    //public class DatabaseAccess
    //{
    //    public static string ConnectionString { get; internal set; }

    //}
    public static class DatabaseAccess
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager
                .ConnectionStrings["QuanLyKhachSanDB"]
                ?.ConnectionString;
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
    }
}
