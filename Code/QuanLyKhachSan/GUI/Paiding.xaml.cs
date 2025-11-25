using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BLL;
using DAL;
using DTO;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Paiding.xaml
    /// </summary>
    public partial class Paiding : Window
    {
        private DatPhongViewDTO booking;
        public Paiding()
        {
            InitializeComponent();
        }

        public Paiding(DatPhongViewDTO bookingInfo)
        {
            InitializeComponent();
            booking = bookingInfo;
            LoadPaidingInfo();
        }
        private void FinalizeCheckoutButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrintVatButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrintProformaButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RecordPaymentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void ApplyDiscountButton_Click(object sender, RoutedEventArgs e)
        {

        }

        // Nút đóng cửa sổ
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainDashboard dashboard = new MainDashboard();
            dashboard.Show();
        }

        private void LoadPaidingInfo()
        {
            // Phòng + khách
            TitleRoomCustomerText.Text = $"Phòng {booking.SoPhong} - {booking.TenKhach}";

            // Booking ID
            BookingIdText.Text = $"Booking #{booking.MaCode}";

            // Tính số đêm
            int soDem = (booking.NgayTra - booking.NgayNhan).Days;

            // Khoảng thời gian lưu trú
            StayDurationText.Text =
                $"Lưu trú: {booking.NgayNhan:dd/MM/yyyy} - {booking.NgayTra:dd/MM/yyyy} ({soDem} đêm)";

            LoadInvoiceDetails();
        }

        private void LoadInvoiceDetails()
        {
            InvoiceDetailsGrid.ItemsSource =
                PaidingBLL.LayHoaDonTheoMotPhong(booking.MaDatChiTiet);
            // 2) Lấy lại dữ liệu tiền phòng & dịch vụ (để tính tổng)
            var room = PhongDAL.GetRoomFullInfo(booking.MaDatChiTiet);
            var dsPhong = new List<RoomFullChargeDTO> { room };

            var dsDichVu = DichVuDAL.GetUsedServices(room.MaDatChiTiet);

            // 3) Lấy tiền cọc (DatPhongTong)
            decimal tienCoc = DatPhongTongDAL.LayTienCoc(booking.MaDatTong);

            // 4) Lấy tiền đã thu (HoaDonThanhToan)

            decimal daThu = HoaDonDAL.LayDaThuTheoMaDatChiTiet(room.MaDatChiTiet);

            // 5) Tính tổng
            var tongKet = ThanhToanTongHopBLL.TinhTongKetThanhToan(
                dsPhong,
                dsDichVu,
                tienCoc,
                daThu
            );

            // 6) Gán UI
            TongTienPhong.Text = tongKet.TongTienPhong.ToString("N0");
            TongTienDichVu.Text = tongKet.TongTienDichVu.ToString("N0");
            PhiVAT.Text = tongKet.VAT.ToString("N0");
            DaThanhToan.Text = tongKet.DaThanhToan.ToString("N0");
            ConLai.Text = tongKet.ConLai.ToString("N0");
        }

        //private void HienAllPhongDangO_Checked(object sender, RoutedEventArgs e)
        //{

        //    InvoiceDetailsGrid.ItemsSource =
        //        PaidingBLL.LayHoaDonTheoTatCaPhong(booking.TenKhach);
        //    // 2) Lấy lại dữ liệu tiền phòng & dịch vụ (để tính tổng)
        //    var room = PhongDAL.GetRoomFullInfo(booking.MaDatChiTiet);
        //    var dsPhong = new List<RoomFullChargeDTO> { room };
        //    var dsMaCT = dsPhong.Select(p => p.MaDatChiTiet).ToList();
        //    var dsDichVu = DichVuDAL.GetUsedServices(room.MaDatChiTiet);

        //    // 3) Lấy tiền cọc (DatPhongTong)
        //    decimal tienCoc = DatPhongTongDAL.LayTienCoc(booking.MaDatTong);

        //    // 4) Lấy tiền đã thu (HoaDonThanhToan)
        //    decimal daThu = HoaDonDAL.LayDaThuTheoMaDatChiTiet(room.MaDatChiTiet);

        //    // 5) Tính tổng
        //    var tongKet = ThanhToanTongHopBLL.TinhTongKetThanhToan(
        //        dsPhong,
        //        dsDichVu,
        //        tienCoc,
        //        daThu
        //    );
        //    CapNhatUIGiaTri(tongKet);
        //    // 6) Gán UI
        //    TongTienPhong.Text = tongKet.TongTienPhong.ToString("N0");
        //    TongTienDichVu.Text = tongKet.TongTienDichVu.ToString("N0");
        //    PhiVAT.Text = tongKet.VAT.ToString("N0");
        //    DaThanhToan.Text = tongKet.DaThanhToan.ToString("N0");
        //    ConLai.Text = tongKet.ConLai.ToString("N0");

        //}
        private void HienAllPhongDangO_Checked(object sender, RoutedEventArgs e)
        {
            // Load nhiều phòng
            var dsPhong = PhongDAL.GetAllRoomsByCustomerName(booking.TenKhach);
            var dsMaCT = dsPhong.Select(p => p.MaDatChiTiet).ToList();
            var dsDichVu = DichVuDAL.GetServicesByListChiTiet(dsMaCT);
            var room = PhongDAL.GetRoomFullInfo(booking.MaDatChiTiet);
            decimal tienCoc = DatPhongTongDAL.LayTienCoc(booking.MaDatTong);
            decimal daThu = HoaDonDAL.LayDaThuTheoMaDatChiTiet(room.MaDatChiTiet);

            // Tính tổng nhiều phòng
            var tongKet = ThanhToanTongHopBLL.TinhTongKetThanhToan(
                dsPhong, dsDichVu, tienCoc, daThu
            );

            CapNhatUIGiaTri(tongKet);

            // Load vào DataGrid
            InvoiceDetailsGrid.ItemsSource = ThanhToanChiTietBLL.LayHoaDonTheoTatCaPhong(booking.TenKhach);
        }

        private void HienAllPhongDangO_Unchecked(object sender, RoutedEventArgs e)
        {
            var room = PhongDAL.GetRoomFullInfo(booking.MaDatChiTiet);
            var dsPhong = new List<RoomFullChargeDTO> { room };
            var dsDichVu = DichVuDAL.GetUsedServices(room.MaDatChiTiet);

            decimal tienCoc = DatPhongTongDAL.LayTienCoc(booking.MaDatTong);
            decimal daThu = HoaDonDAL.LayDaThuTheoMaDatChiTiet(room.MaDatChiTiet);

            // Tính tổng lại về 1 phòng
            var tongKet = ThanhToanTongHopBLL.TinhTongKetThanhToan(
                dsPhong, dsDichVu, tienCoc, daThu
            );

            CapNhatUIGiaTri(tongKet);

            // Load lại DataGrid
            InvoiceDetailsGrid.ItemsSource = ThanhToanChiTietBLL.LayHoaDonTheoMotPhong(booking.MaDatChiTiet);
        }

        private void CapNhatUIGiaTri(TongKetThanhToanDTO tongKet)
        {
            TongTienPhong.Text = tongKet.TongTienPhong.ToString("N0");
            TongTienDichVu.Text = tongKet.TongTienDichVu.ToString("N0");
            GiamGia.Text = tongKet.GiamGia.ToString("N0");
            PhiVAT.Text = tongKet.VAT.ToString("N0");
            DaThanhToan.Text = tongKet.DaThanhToan.ToString("N0");
            ConLai.Text = tongKet.ConLai.ToString("N0");
        }


    }
}
