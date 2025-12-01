using DAL;
using DAL.LoginAndPermission;
using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BLL.LoginAndPermission
{

    public class NhanVienBLL
    {
        private readonly NhanVienDAL dal = new NhanVienDAL();
        private readonly RoleDAL roleDal = new RoleDAL();

        // 1) Lấy danh sách nhân viên
        public List<NhanVienDTO> GetAll()
        {
            return dal.GetAll();
        }

        // 2) Thêm nhân viên (logic chính)
        public string Add(NhanVienDTO nv)
        {
            // --- Validate 1: ID tự sinh nếu trống ---
            if (string.IsNullOrWhiteSpace(nv.NhanVienID))
            {
                nv.NhanVienID = GenerateEmployeeID();
            }

            // --- Validate 2: Họ tên bắt buộc ---
            if (string.IsNullOrWhiteSpace(nv.HoTen))
                return "Họ tên không được để trống.";

            // --- Validate 3: Email hợp lệ ---
            if (!IsValidEmail(nv.Email))
                return "Email không hợp lệ.";

            // --- Validate 4: SĐT bắt buộc & hợp lệ ---
            if (!IsValidPhone(nv.DienThoai))
                return "Số điện thoại không hợp lệ.";

            // --- Validate 5: Ngày sinh phải < ngày hiện tại ---
            if (nv.NgaySinh >= DateTime.Now)
                return "Ngày sinh không hợp lệ.";

            // --- Validate 6: Vai trò phải tồn tại ---
            if (!IsRoleExist(nv.VaiTroID))
                return "Vai trò không tồn tại trong hệ thống.";

            // --- Validate 7: Email không được trùng ---
            if (dal.CheckEmailExists(nv.Email))
                return "Email đã tồn tại trong hệ thống.";

            // --- Gọi DAL ---
            bool ok = dal.Insert(nv);
            return ok ? "SUCCESS" : "Lỗi thêm nhân viên vào database.";
        }

        // 3) Cập nhật nhân viên
        public static string ToMD5(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (var b in hashBytes)
                    sb.Append(b.ToString("X2"));

                return sb.ToString();
            }
        }
        public bool UpdatePassword(string nvID, string newPassword)
        {
            string hash = ToMD5(newPassword);

            string sql = @"UPDATE TaiKhoanHeThong 
                   SET MatKhauHash = @mk 
                   WHERE NhanVienID = @id";

            using (SqlConnection con = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@mk", hash);
                cmd.Parameters.AddWithValue("@id", nvID);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public string Update(NhanVienDTO nv)
        {
            // Kiểm tra nhân viên có tồn tại không
            if (!dal.CheckIDExists(nv.NhanVienID))
                return "Không tìm thấy nhân viên cần cập nhật.";

            // Nếu có mật khẩu mới → cập nhật trước
            if (!string.IsNullOrWhiteSpace(nv.newMk))
            {
                bool okPass = UpdatePassword(nv.NhanVienID, nv.newMk);
                if (!okPass) return "Không thể cập nhật mật khẩu mới.";
            }

            // Nếu bị khóa → không cho sửa
            if (dal.GetStatus(nv.NhanVienID) == "Khóa")
                return "Tài khoản đang bị khóa — không thể chỉnh sửa.";

            // Validate như khi Add
            if (string.IsNullOrWhiteSpace(nv.HoTen))
                return "Họ tên không được để trống.";

            if (!IsValidEmail(nv.Email))
                return "Email không hợp lệ.";

            if (!IsValidPhone(nv.DienThoai))
                return "Số điện thoại không hợp lệ.";

            if (!IsRoleExist(nv.VaiTroID))
                return "Vai trò không tồn tại.";

            bool ok = dal.Update(nv);
            return ok ? "SUCCESS" : "Lỗi cập nhật nhân viên.";
        }

        // 4) Khóa nhân viên
        public string Lock(string id)
        {
            if (!dal.CheckIDExists(id))
                return "Không tìm thấy nhân viên.";

            bool ok = dal.UpdateStatus(id, "Khóa");
            return ok ? "SUCCESS" : "Lỗi khóa nhân viên.";
        }

        // 5) Mở khóa nhân viên
        public string Unlock(string id)
        {
            if (!dal.CheckIDExists(id))
                return "Không tìm thấy nhân viên.";

            bool ok = dal.UpdateStatus(id, "Hoạt động");
            return ok ? "SUCCESS" : "Lỗi mở khóa.";
        }

        // 6) Các hàm nghiệp vụ hỗ trợ

        private bool IsValidPhone(string phone)
        {
            return phone != null &&
                   phone.Length >= 9 &&
                   phone.Length <= 12 &&
                   phone.All(char.IsDigit);
        }

        private bool IsValidEmail(string email)
        {
            return email != null && email.Contains("@") && email.Contains(".");
        }

        private bool IsRoleExist(string vtID)
        {
            var allRoles = roleDal.GetAllRolesDetail(); 
            return allRoles.Any(r => r.VaiTroID == vtID);
        }


        private string GenerateEmployeeID()
        {
            int count = dal.GetTotalEmployees() + 1;
            return "NV" + count.ToString("0000");
        }
    }

}
