using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class DichVuDAL
    {
        public static List<DichVuPhongDTO> LayDichVuHoatDong()
        {
            List<DichVuPhongDTO> list = new List<DichVuPhongDTO>();

            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT DichVuID, TenDichVu, DonGia, DonVi, HieuLuc
                    FROM DichVuPhong
                    WHERE HieuLuc = 1
                ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(new DichVuPhongDTO
                    {
                        DichVuID = rd["DichVuID"].ToString(),
                        TenDichVu = rd["TenDichVu"].ToString(),
                        DonGia = Convert.ToDecimal(rd["DonGia"]),
                        DonVi = rd["DonVi"].ToString(),
                        HieuLuc = Convert.ToBoolean(rd["HieuLuc"])
                    });
                }
            }

            return list;
        }

        public static List<ServiceUsedDTO> GetUsedServices(string maDatChiTiet)
        {
            var list = new List<ServiceUsedDTO>();

            string query = @"
            SELECT 
                dv.TenDichVu, 
                ctdv.SoLuong, 
                ctdv.DonGiaTaiThoiDiem
            FROM ChiTietDichVu ctdv
            JOIN DichVuPhong dv ON dv.DichVuID = ctdv.DichVuID
            WHERE ctdv.MaDatChiTiet = @MaDatChiTiet
            ORDER BY ctdv.NgaySuDung
        ";

            using (var conn = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaDatChiTiet", maDatChiTiet);

                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new ServiceUsedDTO
                        {
                            TenDichVu = rd["TenDichVu"].ToString(),
                            SoLuong = Convert.ToDecimal(rd["SoLuong"]),
                            DonGiaTaiThoiDiem = Convert.ToDecimal(rd["DonGiaTaiThoiDiem"])
                        });
                    }
                }
            }

            return list;
        }

        public static List<ServiceUsedDTO> GetServicesByListChiTiet(List<string> listMaCT)
        {
            var list = new List<ServiceUsedDTO>();

            string paramNames = string.Join(",", listMaCT.Select((x, i) => "@p" + i));
            string query = $@"
            SELECT dv.TenDichVu, ctdv.SoLuong, ctdv.DonGiaTaiThoiDiem
            FROM ChiTietDichVu ctdv
            JOIN DichVuPhong dv ON dv.DichVuID = ctdv.DichVuID
            WHERE ctdv.MaDatChiTiet IN ({paramNames})
        ";

            using (var conn = DatabaseAccess.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                for (int i = 0; i < listMaCT.Count; i++)
                    cmd.Parameters.AddWithValue("@p" + i, listMaCT[i]);

                conn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new ServiceUsedDTO
                        {
                            TenDichVu = rd["TenDichVu"].ToString(),
                            SoLuong = Convert.ToDecimal(rd["SoLuong"]),
                            DonGiaTaiThoiDiem = Convert.ToDecimal(rd["DonGiaTaiThoiDiem"])
                        });
                    }
                }
            }

            return list;
        }
    }
}
