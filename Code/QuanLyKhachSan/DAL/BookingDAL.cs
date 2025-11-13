using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class BookingDAL
    {
        public static string InsertKhachHang(KhachHangDTO kh)
        {
            using (var conn = DatabaseAccess.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_InsertKhachHang", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HoTen", kh.HoTen);
                cmd.Parameters.AddWithValue("@SDT", kh.SDT);
                cmd.Parameters.AddWithValue("@Email", kh.Email);
                cmd.Parameters.AddWithValue("@CongTy", kh.CongTy);
                cmd.Parameters.AddWithValue("@MST", kh.MST);
                cmd.Parameters.AddWithValue("@DiaChi", kh.DiaChi);

                SqlParameter output = new SqlParameter("@KhachHangID", SqlDbType.VarChar, 50)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(output);

                cmd.ExecuteNonQuery();
                return output.Value.ToString();
            }
        }

        public static string InsertPhong(string soPhong, string loaiPhongID)
        {
            using (var conn = DatabaseAccess.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_InsertPhong", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SoPhong", soPhong);
                cmd.Parameters.AddWithValue("@LoaiPhongID", loaiPhongID);

                SqlParameter output = new SqlParameter("@PhongID", SqlDbType.VarChar, 50)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(output);

                cmd.ExecuteNonQuery();
                return output.Value.ToString();
            }
        }

        public static string InsertDatPhongTong(
            string khachHangId,
            string tenDaiDien,
            string sdtDaiDien,
            bool laDoan,
            decimal tongTienCoc,
            string ghiChu,
            string nhanVienId,
            string phongId)
        {
            using (var conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_InsertDatPhongTong", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@KhachHangID", khachHangId);
                cmd.Parameters.AddWithValue("@TenDaiDien", tenDaiDien);
                cmd.Parameters.AddWithValue("@SDTDaiDien", sdtDaiDien);
                cmd.Parameters.AddWithValue("@LaDoan", laDoan);
                cmd.Parameters.AddWithValue("@TongTienCoc", tongTienCoc);
                cmd.Parameters.AddWithValue("@GhiChu", ghiChu);
                cmd.Parameters.AddWithValue("@NhanVienID", nhanVienId);
                cmd.Parameters.AddWithValue("@PhongID", phongId);

                SqlParameter outParam = new SqlParameter("@MaDatTong", SqlDbType.VarChar, 50)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outParam);

                cmd.ExecuteNonQuery();
                return outParam.Value.ToString();
            }
        }
        public static string InsertDatPhongChiTiet(
            string maDatTong,
            DateTime ngayNhan,
            DateTime ngayTra,
            int nguoiLon,
            int treEm,
            int soLuongPhong,
            decimal vat,
            decimal thanhTien,
            string ghiChu)
        {
            using (var conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_InsertDatPhongChiTiet", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaDatTong", maDatTong);
                cmd.Parameters.AddWithValue("@NgayNhan", ngayNhan);
                cmd.Parameters.AddWithValue("@NgayTra", ngayTra);
                cmd.Parameters.AddWithValue("@NguoiLon", nguoiLon);
                cmd.Parameters.AddWithValue("@TreEm", treEm);
                cmd.Parameters.AddWithValue("@SoLuongPhong", soLuongPhong);
                cmd.Parameters.AddWithValue("@VAT", vat);
                cmd.Parameters.AddWithValue("@ThanhTien", thanhTien);
                cmd.Parameters.AddWithValue("@GhiChu", ghiChu);

                SqlParameter outParam = new SqlParameter("@MaDatChiTiet", SqlDbType.VarChar, 50)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outParam);

                cmd.ExecuteNonQuery();
                return outParam.Value.ToString();
            }
        }

        
    }
}

