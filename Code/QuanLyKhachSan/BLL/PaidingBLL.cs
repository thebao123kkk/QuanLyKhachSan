using System.Collections.Generic;
using DAL;
using DTO;

public static class PaidingBLL
{
    // Load 1 phòng
    public static List<InvoiceItemDTO> LayHoaDonTheoMotPhong(string maDatChiTiet)
    {
        var items = new List<InvoiceItemDTO>();

        var room = PhongDAL.GetRoomFullInfo(maDatChiTiet);
        if (room != null)
        {
            items.Add(new InvoiceItemDTO
            {
                Description = $"Tiền phòng – {room.PhongID} ({room.LoaiPhong}, {room.SoDem} đêm)",
                Quantity = 1,
                Price = room.ThanhTien ,
              //  Total = room.ThanhTien
            });

            var services = DichVuDAL.GetUsedServices(room.MaDatChiTiet);

            foreach (var s in services)
            {
                items.Add(new InvoiceItemDTO
                {
                    Description = s.TenDichVu,
                    Quantity = s.SoLuong,
                    Price = s.DonGiaTaiThoiDiem,
                   // Total = s.SoLuong * s.DonGiaTaiThoiDiem
                });
            }
        }

        return items;
    }


    // Load tất cả phòng của khách theo tên
    public static List<InvoiceItemDTO> LayHoaDonTheoTatCaPhong(string tenKhach)
    {
        var items = new List<InvoiceItemDTO>();

        var rooms = PhongDAL.GetAllRoomsByCustomerName(tenKhach);
        List<string> listMaCT = new List<string>();

        foreach (var r in rooms)
        {
            listMaCT.Add(r.MaDatChiTiet);

            items.Add(new InvoiceItemDTO
            {
                Description = $"Tiền phòng – {r.PhongID} ({r.LoaiPhong}, {r.SoDem} đêm)",
                Quantity = 1,
                Price = r.ThanhTien ,
             //   Total = r.ThanhTien
            });
        }

        var services = DichVuDAL.GetServicesByListChiTiet(listMaCT);

        foreach (var s in services)
        {
            items.Add(new InvoiceItemDTO
            {
                Description = s.TenDichVu,
                Quantity = s.SoLuong,
                Price = s.DonGiaTaiThoiDiem,
               // Total = s.SoLuong * s.DonGiaTaiThoiDiem
            });
        }

        return items;
    }
}
