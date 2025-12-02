//using DTO;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DAL
//{
//    public class SqlConnectionData
//    {
//        public static SqlConnection Connect()
//        {
//            // Chuỗi kết nối đến SQL Server (nhớ đổi khi đổi máy)
//            string strcon = 
//                //"Data Source=.\\SQLEXPRESS;Initial Catalog=QuanLyKhachSan;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
//                "Server=localhost\\SQLEXPRESS01;Database=QuanLyKhachSan;Trusted_Connection=True;";

//            SqlConnection conn = new SqlConnection(strcon);
//            return conn;
//        }
//    }
//    //public class DatabaseAccess
//    //{
//    //    public static string ConnectionString { get; internal set; }

//    //}
//    public static class DatabaseAccess
//    {
//        public static string GetConnectionString()
//        {
//            return ConfigurationManager
//                .ConnectionStrings["QuanLyKhachSan"]
//                ?.ConnectionString;
//        }

//        public static SqlConnection GetConnection()
//        {
//            return new SqlConnection(GetConnectionString());
//        }
//    }
//}


using System;
using System.Data.SqlClient;

namespace DAL
{
    public  class DatabaseAccess
    {
        // Chuỗi kết nối được mã hóa cứng
        private const string ConnectionString =
            "Server=localhost\\SQLEXPRESS;Database=QuanLyKhachSan;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;";
        // Hàm tạo và mở kết nối
        public  static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);

            // Cố gắng mở kết nối và xử lý ngoại lệ nếu thất bại
            try
            {
                return conn;
            }
            catch (SqlException ex)
            {
                // Thay vì chỉ trả về, nên log lỗi hoặc ném lại ngoại lệ với thông báo rõ ràng hơn
                throw new Exception("Lỗi kết nối cơ sở dữ liệu: Vui lòng kiểm tra Server Name và trạng thái SQL Server.", ex);
            }
        }
    }
}