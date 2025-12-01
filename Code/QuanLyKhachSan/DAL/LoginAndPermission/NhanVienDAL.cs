using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
namespace DAL.LoginAndPermission
{
    public class NhanVienDAL
    {
        // 1) Get All Employees
        public List<NhanVienDTO> GetAll()
        {
            List<NhanVienDTO> list = new List<NhanVienDTO>();

            string query = "SELECT nv.*, vt.TenVaiTro\r\nFROM NhanVien nv\r\nJOIN VaiTro vt ON nv.VaiTroID = vt.VaiTroID\r\nORDER BY nv.NgayTao DESC\r\n";

            using (SqlConnection con = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(new NhanVienDTO
                    {
                        NhanVienID = rd["NhanVienID"].ToString(),
                        HoTen = rd["HoTen"].ToString(),
                        ChucVu = rd["ChucVu"].ToString(),
                        GioiTinh = rd["GioiTinh"].ToString(),
                        NgaySinh = rd["NgaySinh"] == DBNull.Value ? null : (DateTime?)rd["NgaySinh"],
                        DienThoai = rd["DienThoai"].ToString(),
                        Email = rd["Email"].ToString(),
                        DiaChi = rd["DiaChi"].ToString(),
                        VaiTroID = rd["VaiTroID"].ToString(),
                        TrangThai = rd["TrangThai"].ToString()
                    });

                }
            }

            return list;
        }

        // 2) INSERT
        public bool Insert(NhanVienDTO nv)
        {
            string query = @"
            INSERT INTO NhanVien
            (NhanVienID, HoTen, ChucVu, GioiTinh, NgaySinh,
             DienThoai, Email, DiaChi, VaiTroID, TrangThai, NgayTao)
            VALUES
            (@ID, @HoTen, @ChucVu, @GioiTinh, @NgaySinh,
             @DienThoai, @Email, @DiaChi, @VaiTroID, N'Đang làm', GETDATE())
        ";

            using (SqlConnection con = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ID", nv.NhanVienID);
                cmd.Parameters.AddWithValue("@HoTen", nv.HoTen);
                cmd.Parameters.AddWithValue("@ChucVu", nv.ChucVu);
                cmd.Parameters.AddWithValue("@GioiTinh", nv.GioiTinh);
                cmd.Parameters.AddWithValue("@NgaySinh", nv.NgaySinh);
                cmd.Parameters.AddWithValue("@DienThoai", nv.DienThoai);
                cmd.Parameters.AddWithValue("@Email", nv.Email);
                cmd.Parameters.AddWithValue("@DiaChi", nv.DiaChi);
                cmd.Parameters.AddWithValue("@VaiTroID", nv.VaiTroID);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 3) UPDATE
        public bool Update(NhanVienDTO nv)
        {
            string query = @"
            UPDATE NhanVien
            SET HoTen=@HoTen,
                ChucVu=@ChucVu,
                GioiTinh=@GioiTinh,
                NgaySinh=@NgaySinh,
                DienThoai=@DienThoai,
                Email=@Email,
                DiaChi=@DiaChi,
                VaiTroID=@VaiTroID
            WHERE NhanVienID=@ID
        ";

            using (SqlConnection con = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ID", nv.NhanVienID);
                cmd.Parameters.AddWithValue("@HoTen", nv.HoTen);
                cmd.Parameters.AddWithValue("@ChucVu", nv.ChucVu);
                cmd.Parameters.AddWithValue("@GioiTinh", nv.GioiTinh);
                cmd.Parameters.AddWithValue("@NgaySinh", nv.NgaySinh);
                cmd.Parameters.AddWithValue("@DienThoai", nv.DienThoai);
                cmd.Parameters.AddWithValue("@Email", nv.Email);
                cmd.Parameters.AddWithValue("@DiaChi", nv.DiaChi);
                cmd.Parameters.AddWithValue("@VaiTroID", nv.VaiTroID);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 4) Update trạng thái nhân viên
        public bool UpdateStatus(string id, string status)
        {
            string query = @"
            UPDATE NhanVien
            SET TrangThai=@TrangThai
            WHERE NhanVienID=@ID
        ";

            using (SqlConnection con = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@TrangThai", status);
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 5) Kiểm tra email trùng
        public bool CheckEmailExists(string email)
        {
            string query = "SELECT COUNT(*) FROM NhanVien WHERE Email=@Email";

            using (SqlConnection con = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Email", email);

                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        // 6) Kiểm tra Nhân viên ID tồn tại?
        public bool CheckIDExists(string id)
        {
            string query = "SELECT COUNT(*) FROM NhanVien WHERE NhanVienID=@ID";

            using (SqlConnection con = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        // 7) Lấy trạng thái nhân viên
        public string GetStatus(string id)
        {
            string query = "SELECT TrangThai FROM NhanVien WHERE NhanVienID=@ID";

            using (SqlConnection con = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                object result = cmd.ExecuteScalar();

                return result?.ToString() ?? "";
            }
        }

        // 8) Đếm tổng số nhân viên (để sinh mã NV)
        public int GetTotalEmployees()
        {
            string query = "SELECT COUNT(*) FROM NhanVien";

            using (SqlConnection con = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
