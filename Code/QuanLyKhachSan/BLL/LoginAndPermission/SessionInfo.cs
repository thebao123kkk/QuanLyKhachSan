using System;

namespace BLL.LoginAndPermission
{
    public static class SessionInfo
    {
        // ===== THÔNG TIN NGƯỜI DÙNG =====
        public static string TaiKhoanID { get; private set; }
        public static string TenDangNhap { get; private set; }
        public static string HoTen { get; private set; }
        public static string Email { get; private set; }
        public static string VaiTroID { get; private set; }
        public static string NhanVienID { get; private set; }

        // ===== THÔNG TIN PHIÊN ĐĂNG NHẬP =====
        public static DateTime? ThoiGianDangNhap { get; private set; }
        public static DateTime? ThoiGianDangXuat { get; private set; }

        public static string TenMayTinh { get; private set; }
        public static string TenUngDung { get; private set; } = "QuanLyKhachSan";


        public static void StartSession(DTO.TaiKhoanDTO user)
        {
            if (user == null) return;

            TaiKhoanID = user.TaiKhoanID;
            TenDangNhap = user.TenDangNhap;
            Email = user.Email;
            VaiTroID = user.VaiTroID;
            NhanVienID = user.NhanVienID;

            ThoiGianDangNhap = DateTime.Now;
            TenMayTinh = Environment.MachineName;
        }

        /// Gọi khi người dùng đăng xuất hoặc tắt ứng dụng.
        public static void EndSession()
        {
            ThoiGianDangXuat = DateTime.Now;
        }

        /// Trả về chuỗi mô tả phiên – dùng để ghi log.
        public static string ToLogString()
        {
            return
                $"[{TenDangNhap}] - Vai trò: {VaiTroID} - " +
                $"Máy: {TenMayTinh} - Ứng dụng: {TenUngDung} - " +
                $"Login: {ThoiGianDangNhap?.ToString("yyyy-MM-dd HH:mm:ss")}";
        }

        /// Xóa sạch session (nếu cần).
        public static void Clear()
        {
            TaiKhoanID = null;
            TenDangNhap = null;
            Email = null;
            VaiTroID = null;
            NhanVienID = null;

            ThoiGianDangNhap = null;
            ThoiGianDangXuat = null;
            TenMayTinh = null;
        }
    }
}
