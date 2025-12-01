using DAL.LoginAndPermission;
using DTO;
using System;

namespace BLL.LoginAndPermission
{
    public class AuthService
    {
        private readonly QuyenDAL _quyenDal = new QuyenDAL();

        public void LoadPermissions(string taiKhoanID, string vaiTroID)
        {
            PermissionContext.TaiKhoanID = taiKhoanID;
            PermissionContext.VaiTroID = vaiTroID;

            // Load danh sách quyền
            PermissionContext.QuyenIDs = _quyenDal.GetQuyenByTaiKhoan(taiKhoanID);
        }

        public bool Has(string quyenID)
        {
            return PermissionContext.QuyenIDs.Contains(quyenID);
        }

        public void Require(string quyenID)
        {
            if (!Has(quyenID))
                throw new UnauthorizedAccessException($"Bạn không có quyền {quyenID} để thực hiện hành động này.");
        }
    }
}
