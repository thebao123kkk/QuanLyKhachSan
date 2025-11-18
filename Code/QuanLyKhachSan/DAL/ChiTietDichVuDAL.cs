using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class ChiTietDichVuDAL
    {
        public static bool LuuChiTietDichVu(string maDatChiTiet, List<DichVuPhongDTO> ds)
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                foreach (var item in ds)
                {
                    string newId = "CTDV" + Guid.NewGuid().ToString("N").Substring(0, 6);

                    string sql = @"
                INSERT INTO ChiTietDichVu
                (MaChiTietDV, MaDatChiTiet, DichVuID, SoLuong, DonGiaTaiThoiDiem, NgaySuDung)
                VALUES (@id, @mact, @dvd, @sl, @gia, GETDATE())";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", newId);
                    cmd.Parameters.AddWithValue("@mact", maDatChiTiet);
                    cmd.Parameters.AddWithValue("@dvd", item.DichVuID);
                    cmd.Parameters.AddWithValue("@sl", item.SoLuong);
                    cmd.Parameters.AddWithValue("@gia", item.DonGia);

                    cmd.ExecuteNonQuery();
                }

                return true;
            }
        }

        public static List<ChiTietDichVuDTO> LoadChiTietDichVu(string maDatChiTiet)
        {
            List<ChiTietDichVuDTO> list = new List<ChiTietDichVuDTO>();

            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                string sql = @"
            SELECT ct.MaChiTietDV, ct.MaDatChiTiet, ct.DichVuID, dv.TenDichVu,
                   ct.SoLuong, ct.DonGiaTaiThoiDiem, ct.NgaySuDung
            FROM ChiTietDichVu ct
            JOIN DichVuPhong dv ON dv.DichVuID = ct.DichVuID
            WHERE ct.MaDatChiTiet = @id
        ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", maDatChiTiet);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(new ChiTietDichVuDTO
                    {
                        MaChiTietDV = rd["MaChiTietDV"].ToString(),
                        MaDatChiTiet = rd["MaDatChiTiet"].ToString(),
                        DichVuID = rd["DichVuID"].ToString(),
                        TenDichVu = rd["TenDichVu"].ToString(),
                        SoLuong = Convert.ToDecimal(rd["SoLuong"]),
                        DonGiaTaiThoiDiem = Convert.ToDecimal(rd["DonGiaTaiThoiDiem"]),
                        NgaySuDung = Convert.ToDateTime(rd["NgaySuDung"])
                    });
                }
                return list;
            }
        }


    }
}
