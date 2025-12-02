using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class QuanLyDAL
    {
        // Kiểm tra phòng đã tồn tại hay chưa
        public static bool PhongExists(string phongID)
        {
            string query = "SELECT COUNT(*) FROM Phong WHERE PhongID = @id";
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", phongID);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return (count > 0);
            }
        }


        // Thêm mới phòng
        public static void InsertPhong(string phongID, string soPhong, string loaiPhongID, string trangThai, string ghiChu)
        {
            string query = @"INSERT INTO Phong (PhongID, SoPhong, LoaiPhongID, TrangThai, GhiChu)
                     VALUES (@id, @soPhong, @loaiID, @trangThai, @ghiChu)";
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", phongID);
                cmd.Parameters.AddWithValue("@soPhong", soPhong);
                cmd.Parameters.AddWithValue("@loaiID", loaiPhongID);
                cmd.Parameters.AddWithValue("@trangThai", trangThai);
                cmd.Parameters.AddWithValue("@ghiChu", ghiChu ?? (object)DBNull.Value);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        // Cập nhật thông tin phòng
        public static void UpdatePhong(string phongID, string soPhong, string loaiPhongID, string trangThai, string ghiChu)
        {
            string query = @"UPDATE Phong SET SoPhong = @soPhong, LoaiPhongID = @loaiID, TrangThai = @trangThai, GhiChu = @ghiChu
                     WHERE PhongID = @id";
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@soPhong", soPhong);
                cmd.Parameters.AddWithValue("@loaiID", loaiPhongID);
                cmd.Parameters.AddWithValue("@trangThai", trangThai);
                cmd.Parameters.AddWithValue("@ghiChu", ghiChu ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@id", phongID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Kiểm tra Loại phòng đã tồn tại chưa
        public static bool LoaiPhongExists(string loaiPhongID)
        {
            string query = "SELECT COUNT(*) FROM LoaiPhongChiTiet WHERE LoaiPhongID = @id";
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", loaiPhongID);
                conn.Open();
                // ExecuteScalar() trả về tổng số hàng thỏa mãn:contentReference[oaicite:0]{index=0}
                int count = (int)cmd.ExecuteScalar();
                return (count > 0);
            }
        }

        // Thêm mới loại phòng
        public static void InsertLoaiPhong(string loaiPhongID, string nhomLoaiID, string tenLoai, string moTa, int sucChua, decimal giaCoBan)
        {
            string query = "INSERT INTO LoaiPhongChiTiet (LoaiPhongID, NhomLoaiID, TenLoai, MoTa, SucChua, GiaCoBan) " +
                           "VALUES (@id, @nhom, @ten, @mota, @suc, @gia)";
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", loaiPhongID);
                cmd.Parameters.AddWithValue("@nhom", nhomLoaiID);
                cmd.Parameters.AddWithValue("@ten", tenLoai);
                cmd.Parameters.AddWithValue("@mota", moTa);
                cmd.Parameters.AddWithValue("@suc", sucChua);
                cmd.Parameters.AddWithValue("@gia", giaCoBan);
                conn.Open();
                // ExecuteNonQuery() thực thi INSERT/UPDATE/DELETE:contentReference[oaicite:1]{index=1}
                cmd.ExecuteNonQuery();
            }
        }

        // Cập nhật loại phòng
        public static void UpdateLoaiPhong(string loaiPhongID, string nhomLoaiID, string tenLoai, string moTa, int sucChua, decimal giaCoBan)
        {
            string query = "UPDATE LoaiPhongChiTiet " +
                           "SET NhomLoaiID = @nhom, TenLoai = @ten, MoTa = @mota, SucChua = @suc, GiaCoBan = @gia " +
                           "WHERE LoaiPhongID = @id";
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nhom", nhomLoaiID);
                cmd.Parameters.AddWithValue("@ten", tenLoai);
                cmd.Parameters.AddWithValue("@mota", moTa);
                cmd.Parameters.AddWithValue("@suc", sucChua);
                cmd.Parameters.AddWithValue("@gia", giaCoBan);
                cmd.Parameters.AddWithValue("@id", loaiPhongID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // --- Nhóm loại phòng ---
        public static bool NhomLoaiPhongExists(string nhomLoaiID)
        {
            string query = "SELECT COUNT(*) FROM NhomLoaiPhong WHERE NhomLoaiID = @id";
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", nhomLoaiID);
                conn.Open();
                return ((int)cmd.ExecuteScalar() > 0);
            }
        }
        public static void InsertNhomLoaiPhong(string nhomLoaiID, string tenNhom, string moTa)
        {
            string query = "INSERT INTO NhomLoaiPhong (NhomLoaiID, TenNhom, MoTa) " +
                           "VALUES (@id, @ten, @mota)";
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", nhomLoaiID);
                cmd.Parameters.AddWithValue("@ten", tenNhom);
                cmd.Parameters.AddWithValue("@mota", moTa);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public static void UpdateNhomLoaiPhong(string nhomLoaiID, string tenNhom, string moTa)
        {
            string query = "UPDATE NhomLoaiPhong SET TenNhom = @ten, MoTa = @mota WHERE NhomLoaiID = @id";
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ten", tenNhom);
                cmd.Parameters.AddWithValue("@mota", moTa);
                cmd.Parameters.AddWithValue("@id", nhomLoaiID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // --- Dịch vụ phòng ---
        public static bool DichVuExists(string dichVuID)
        {
            string query = "SELECT COUNT(*) FROM DichVuPhong WHERE DichVuID = @id";
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", dichVuID);
                conn.Open();
                return ((int)cmd.ExecuteScalar() > 0);
            }
        }
        public static void InsertDichVu(string dichVuID, string tenDichVu, decimal donGia, string donVi, bool hieuLuc)
        {
            string query = "INSERT INTO DichVuPhong (DichVuID, TenDichVu, DonGia, DonVi, HieuLuc) " +
                           "VALUES (@id, @ten, @gia, @donvi, @hieu)";
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", dichVuID);
                cmd.Parameters.AddWithValue("@ten", tenDichVu);
                cmd.Parameters.AddWithValue("@gia", donGia);
                cmd.Parameters.AddWithValue("@donvi", donVi);
                cmd.Parameters.AddWithValue("@hieu", hieuLuc);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public static void UpdateDichVu(string dichVuID, string tenDichVu, decimal donGia, string donVi, bool hieuLuc)
        {
            string query = "UPDATE DichVuPhong SET TenDichVu = @ten, DonGia = @gia, DonVi = @donvi, HieuLuc = @hieu " +
                           "WHERE DichVuID = @id";
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ten", tenDichVu);
                cmd.Parameters.AddWithValue("@gia", donGia);
                cmd.Parameters.AddWithValue("@donvi", donVi);
                cmd.Parameters.AddWithValue("@hieu", hieuLuc);
                cmd.Parameters.AddWithValue("@id", dichVuID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // --- Mã giảm giá ---
        public static bool MaGiamGiaExists(string magiamgiaID)
        {
            string query = "SELECT COUNT(*) FROM MaGiamGia WHERE MGGID = @id";
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", magiamgiaID);
                conn.Open();
                return ((int)cmd.ExecuteScalar() > 0);
            }
        }
        public static void InsertMaGiamGia(string magiamgiaID, DateTime tuNgay, DateTime denNgay, int phanTram)
        {
            string query = "INSERT INTO MaGiamGia (MGGID, TuNgay, DenNgay, PhanTramGiamGia) " +
                           "VALUES (@id, @tu, @den, @ptg)";
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", magiamgiaID);
                cmd.Parameters.AddWithValue("@tu", tuNgay);
                cmd.Parameters.AddWithValue("@den", denNgay);
                cmd.Parameters.AddWithValue("@ptg", phanTram);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateMaGiamGia(string magiamgiaID, DateTime tuNgay, DateTime denNgay, int phanTram)
        {
            string query = "UPDATE MaGiamGia SET TuNgay = @tu, DenNgay = @den, PhanTramGiamGia = @ptg " +
                           "WHERE MGGID = @id";
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@tu", tuNgay);
                cmd.Parameters.AddWithValue("@den", denNgay);
                cmd.Parameters.AddWithValue("@ptg", phanTram);
                cmd.Parameters.AddWithValue("@id", magiamgiaID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        


    }
}
