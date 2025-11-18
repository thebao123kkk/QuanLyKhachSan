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
    }
}
