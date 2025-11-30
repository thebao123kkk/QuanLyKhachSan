using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public static class ThanhToanChiTietBLL
    {
        public static List<InvoiceItemDTO> LayHoaDonTheoMotPhong(string maDatChiTiet)
        {
            var items = new List<InvoiceItemDTO>();

            var phong = PhongDAL.GetRoomFullInfo(maDatChiTiet);
            if (phong != null)
            {
                items.Add(new InvoiceItemDTO
                {
                    Description = $"Tiền phòng – {phong.PhongID} ({phong.LoaiPhong}, {phong.SoDem} đêm)",
                    Quantity = 1,
                    Price = phong.ThanhTien ,
                    //Total = phong.ThanhTien
                });

                var dichVu = DichVuDAL.GetUsedServices(phong.MaDatChiTiet);

                foreach (var dv in dichVu)
                {
                    items.Add(new InvoiceItemDTO
                    {
                        Description = dv.TenDichVu,
                        Quantity = dv.SoLuong,
                        Price = dv.DonGiaTaiThoiDiem,
                      //  Total = dv.SoLuong * dv.DonGiaTaiThoiDiem
                    });
                }
            }

            return items;
        }


        public static List<InvoiceItemDTO> LayHoaDonTheoTatCaPhong(string tenKhach)
        {
            var items = new List<InvoiceItemDTO>();

            var dsPhong = PhongDAL.GetAllRoomsByCustomerName(tenKhach);
            var dsMaCT = new List<string>();

            foreach (var p in dsPhong)
            {
                dsMaCT.Add(p.MaDatChiTiet);

                items.Add(new InvoiceItemDTO
                {
                    Description = $"Tiền phòng – {p.PhongID} ({p.LoaiPhong}, {p.SoDem} đêm)",
                    Quantity = 1,
                    Price = p.ThanhTien,
                    //Total = p.ThanhTien
                });
            }

            var dsDichVu = DichVuDAL.GetServicesByListChiTiet(dsMaCT);

            foreach (var dv in dsDichVu)
            {
                items.Add(new InvoiceItemDTO
                {
                    Description = dv.TenDichVu,
                    Quantity = dv.SoLuong,
                    Price = dv.DonGiaTaiThoiDiem,
                    //Total = dv.SoLuong * dv.DonGiaTaiThoiDiem
                });
            }

            return items;
        }
    }
}
