using BLL.LoginAndPermission;
using DAL;

namespace BLL
{
    public class LogBLL
    {
        private readonly LogDAL _dal = new LogDAL();

        public void GhiDangNhap()
        {
            _dal.GhiLog(
                SessionInfo.TaiKhoanID,
                SessionInfo.TenDangNhap,
                "Đăng nhập",
                $"{SessionInfo.TenDangNhap} đã đăng nhập vào hệ thống."
            );
        }

        public void GhiDangXuat()
        {
            _dal.GhiLog(
                SessionInfo.TaiKhoanID,
                SessionInfo.TenDangNhap,
                "Đăng xuất",
                $"{SessionInfo.TenDangNhap} đã đăng xuất khỏi hệ thống."
            );
        }

        public void GhiThaoTac(string hanhDong, string noiDung)
        {
            _dal.GhiLog(
                SessionInfo.TaiKhoanID,
                SessionInfo.TenDangNhap,
                hanhDong,
                noiDung
            );
        }
    }
}
