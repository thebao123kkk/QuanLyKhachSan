// Project: BLL
using DAL;
using DAL.LoginAndPermission;
using DTO;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BLL.LoginAndPermission
{
    public class LoginBLL
    {
        private readonly TaiKhoanDAL _taiKhoanDal = new TaiKhoanDAL();
        private readonly RoleDAL _roleDal = new RoleDAL();
        private readonly LogBLL _logBLL = new LogBLL();
        //DTO lưu trạng thái đăng nhập
        public class LoginResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public TaiKhoanDTO User { get; set; }

        }
        public LoginResult Fail(string msg)
        {
            return new LoginResult { Success = false, Message = msg };
        }
        //Hàm băm MD5
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

        public LoginResult DangNhap(string username, string password, string selectedRole)
        {
            // 1. Kiểm tra input
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu."
                };
            }


            // 2. Lấy tài khoản theo username
            TaiKhoanDTO tk = _taiKhoanDal.GetByUsername(username);
            if (tk == null)
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Sai tên đăng nhập hoặc mật khẩu."
                };
            }

            // 3. Kiểm tra trạng thái khóa
            if (tk.Khoa)
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Tài khoản đã bị khóa. Vui lòng liên hệ Quản lý."
                };
            }

            // 4. Hash mật khẩu và so sánh
            string inputHash = ToMD5(password);
            if (!string.Equals(inputHash, tk.MatKhauHash, StringComparison.OrdinalIgnoreCase))
            {
                // TODO: tăng số lần sai, nếu >= 5 thì gọi DAL khóa tài khoản (đúng SRS).
                return new LoginResult
                {
                    Success = false,
                    Message = "Sai tên đăng nhập hoặc mật khẩu."
                };
            }

            // 5. Kiểm tra Role khớp với DB
            string selectedRoleID = _roleDal.GetRoleIDByName(selectedRole);
            if (selectedRoleID == null)
                return Fail("Vai trò không tồn tại trong hệ thống.");

            // ⭐ SO SÁNH ROLE ID
            if (!string.Equals(selectedRoleID, tk.VaiTroID, StringComparison.OrdinalIgnoreCase))
            {
                return Fail($"Sai vai trò. Vui lòng chọn lại");
            }

            // 6. Kiểm tra trạng thái hoạt động (KHÔNG ÁP DỤNG CHO ADMIN)
            if (!string.Equals(tk.TenDangNhap, "admin", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.Equals(tk.TrangThai, "Hoạt động", StringComparison.OrdinalIgnoreCase))
                {
                    return Fail("Tài khoản không còn hoạt động. Vui lòng liên hệ Quản lý.");
                }
            }



            // 7. Thành công
            SessionInfo.StartSession(tk);

            // 8. TỰ ĐỘNG GHI LOG TẠI ĐÂY
            _logBLL.GhiThaoTac("Đăng nhập",
                $"{tk.TenDangNhap} đã đăng nhập vào hệ thống.");

            return new LoginResult
            {
                Success = true,
                Message = "Đăng nhập thành công!",
                User = tk
            };
        }
    }
}
