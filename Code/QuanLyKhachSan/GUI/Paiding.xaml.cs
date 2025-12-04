using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
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
using BLL.LoginAndPermission;
using DAL;
using DTO;

namespace GUI
{
    public partial class Paiding : Window
    {
        private DatPhongViewDTO booking;
        private readonly LogBLL _log = new LogBLL();

        public Paiding()
        {
            InitializeComponent();
        }

        public Paiding(DatPhongViewDTO bookingInfo)
        {
            InitializeComponent();
            booking = bookingInfo;
            LoadPaidingInfo();
            if (booking.TrangThai == "Hoàn tất TT")
            {
                MessageBox.Show("Đơn phòng này đã thanh toán hoàn tất. Không thể ghi nhận thêm.",
                                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                GhiNhanThanhToanButton.IsEnabled = false;
                SoTienThanhToantb.IsEnabled = false;
                ApMaGiamGiabtn.IsEnabled = false;
            }
        }
        private void FinalizeCheckoutButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrintVatButton_Click(object sender, RoutedEventArgs e)
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
        private int phanTramGiamGia = 0;

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
                daThu, phanTramGiamGia
            );

            // 6) Gán UI
            TongTienPhong.Text = tongKet.TongTienPhong.ToString("N0");
            TongTienDichVu.Text = tongKet.TongTienDichVu.ToString("N0");
            PhiVAT.Text = tongKet.VAT.ToString("N0");
            DaThanhToan.Text = tongKet.DaThanhToan.ToString("N0");
            ConLai.Text = tongKet.ConLai.ToString("N0");
        }

        
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
                dsPhong, dsDichVu, tienCoc, daThu, phanTramGiamGia
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
                dsPhong, dsDichVu, tienCoc, daThu, phanTramGiamGia
            );

            CapNhatUIGiaTri(tongKet);

            // Load lại DataGrid
            InvoiceDetailsGrid.ItemsSource = ThanhToanChiTietBLL.LayHoaDonTheoMotPhong(booking.MaDatChiTiet);
        }

        private void ApMaGiamGiaBtn_Click(object sender, RoutedEventArgs e)
        {
            string code = txtMaGiamGia.Text.Trim();
            var mgg = MaGiamGiaBLL.KiemTraMa(code);

            if (mgg == null)
            {
                MessageBox.Show("Mã giảm giá không hợp lệ hoặc hết hạn.");
                return;
            }

            // LƯU GIẢM GIÁ LẠI
            phanTramGiamGia = mgg.PhanTramGiamGia;

            // XÁC ĐỊNH ĐANG CHỌN 1 PHÒNG HAY TẤT CẢ PHÒNG
            // 1) Lấy thông tin phòng chính
            var roomChinh = PhongDAL.GetRoomFullInfo(booking.MaDatChiTiet);

            // 2) Danh sách phòng & dịch vụ sẽ dùng để tính
            List<RoomFullChargeDTO> dsPhong;
            List<ServiceUsedDTO> dsDichVu;

            // 3) Kiểm tra ALL PHÒNG
            if (AllPhong.IsChecked == true)
            {
                // Lấy tất cả phòng theo tên khách
                dsPhong = PhongDAL.GetAllRoomsByCustomerName(booking.TenKhach);

                // Lấy list mã đặt chi tiết của tất cả phòng
                var dsMaCT = dsPhong.Select(p => p.MaDatChiTiet).ToList();

                // Lấy tất cả dịch vụ của tất cả phòng
                dsDichVu = DichVuDAL.GetServicesByListChiTiet(dsMaCT);
            }
            else
            {
                // Chỉ 1 phòng
                dsPhong = new List<RoomFullChargeDTO> { roomChinh };

                // Dịch vụ của phòng chính
                dsDichVu = DichVuDAL.GetUsedServices(roomChinh.MaDatChiTiet);
            }

            decimal tienCoc = DatPhongTongDAL.LayTienCoc(booking.MaDatTong);

            // DÙ ALL PHÒNG → CHỈ CẦN LẤY đã thu CỦA booking chính
            decimal daThu = HoaDonDAL.LayDaThuTheoMaDatChiTiet(booking.MaDatChiTiet);

            // TÍNH TỔNG SAU GIẢM
            var tongKet = ThanhToanTongHopBLL.TinhTongKetThanhToan(
                dsPhong,
                dsDichVu,
                tienCoc,
                daThu,
                phanTramGiamGia
            );

            CapNhatUIGiaTri(tongKet);

            MessageBox.Show($"Áp dụng mã giảm giá {phanTramGiamGia}% thành công!");
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


        private void PrintProformaButton_Click(object sender, RoutedEventArgs e)
        {
            var dto = new PhieuTamTinhDTO
            {
                Booking = booking
            };

            string khID = DatPhongTongDAL.LayKhachHangID(booking.MaDatTong);
            var khach = KhachHangDAL.LayThongTinKhach(khID);

            // Tự tạo mã TMP theo công thức bạn yêu cầu
            dto.SoPhieu = $"TMP-{DateTime.Now:yyMMdd}-{new Random().Next(100, 999)}";

            dto.TenKhach = khach.HoTen;
            dto.SDT = khach.SDT;
            dto.DiaChi = khach.DiaChi;
            dto.Phong = booking.PhongID;
            dto.LoaiPhong = booking.LoaiPhong;
            dto.NgayDen = booking.NgayNhan;
            dto.NgayDi = booking.NgayTra;
            dto.NhanVien = "Lê Minh Tuấn";
            decimal tienCoc = DatPhongTongDAL.LayTienCoc(booking.MaDatTong);
            dto.DaDatCoc = tienCoc;

            //dto.NhanVien = GlobalInfo.NhanVienDangNhap;

            // Lấy chi tiết từ datagrid
            dto.ChiTiet = InvoiceDetailsGrid.Items
                .Cast<InvoiceItemDTO>()
                .ToList();

            // Tổng tiền hàng
            dto.TongTienHang = dto.ChiTiet.Sum(x => x.Total);

            // GIẢM GIÁ
            decimal tienGiam = 0;
            if (phanTramGiamGia > 0)
            {
                tienGiam = dto.TongTienHang * phanTramGiamGia / 100m;
            }
            dto.PhanTramGiamGia = phanTramGiamGia;


            dto.TienGiamGia = tienGiam;      // PHẢI THÊM TRƯỜNG NÀY TRONG DTO
            dto.PhanTramGiamGia = phanTramGiamGia;

            dto.VAT = (dto.TongTienHang - tienGiam) * 0.08m;

            dto.ConLai = dto.TongTienHang - tienGiam + dto.VAT - dto.DaDatCoc;



            PhieuTamTinhBLL.InPhieuTamTinh(dto);
            _log.GhiThaoTac(
                "In phiếu tạm tính",
                $"{SessionInfo.TenDangNhap} đã in TMP cho phòng {booking.PhongID} (Booking #{booking.MaCode})"
            );

        }

        private async void GhiNhanThanhToan_Click(object sender, RoutedEventArgs e)
        {
            string trangThai = DatPhongTongDAL.LayTrangThai(booking.MaDatTong);

            if (trangThai == "Hoàn tất TT")
            {
                MessageBox.Show("Đơn này đã thanh toán hoàn tất. Không thể ghi nhận thêm.",
                                "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // -------------------------------
            // 1) XÁC ĐỊNH DANH SÁCH PHÒNG + DỊCH VỤ
            // -------------------------------
            List<RoomFullChargeDTO> dsPhong;
            List<ServiceUsedDTO> dsDichVu;

            if (AllPhong.IsChecked == true)
            {
                // LẤY TẤT CẢ PHÒNG CỦA KHÁCH NÀY
                dsPhong = PhongDAL.GetAllRoomsByCustomerName(booking.TenKhach);

                var dsMaCT = dsPhong.Select(p => p.MaDatChiTiet).ToList();

                // LẤY TOÀN BỘ DỊCH VỤ CỦA TẤT CẢ PHÒNG
                dsDichVu = DichVuDAL.GetServicesByListChiTiet(dsMaCT);
            }
            else
            {
                // CHỈ 1 PHÒNG
                var room = PhongDAL.GetRoomFullInfo(booking.MaDatChiTiet);
                dsPhong = new List<RoomFullChargeDTO> { room };
                dsDichVu = DichVuDAL.GetUsedServices(room.MaDatChiTiet);
            }

            // -------------------------------
            // 2) LẤY TIỀN CỌC VÀ ĐÃ THU (CỦA BOOKING GỐC)
            // -------------------------------
            decimal tienCoc = DatPhongTongDAL.LayTienCoc(booking.MaDatTong);
            decimal daThu = HoaDonDAL.LayDaThuTheoMaDatChiTiet(booking.MaDatChiTiet);

            // -------------------------------
            // 3) TÍNH TỔNG KẾT (CÓ GIẢM GIÁ)
            // -------------------------------
            var tongKet = ThanhToanTongHopBLL.TinhTongKetThanhToan(
                dsPhong,
                dsDichVu,
                tienCoc,
                daThu,
                phanTramGiamGia           // GỬI GIẢM GIÁ VÔ BLL
            );

            // -------------------------------
            // 4) LẤY SỐ TIỀN THANH TOÁN THÊM
            // -------------------------------
            decimal soTienThanhToanThem = 0;
            decimal.TryParse(
                SoTienThanhToantb.Text,
                NumberStyles.AllowThousands,
                CultureInfo.GetCultureInfo("vi-VN"),
                out soTienThanhToanThem
            );

            // 2. Lấy số còn lại từ tongKet
            decimal soConLai = tongKet.ConLai;

            // 3. Kiểm tra đúng số tiền
            if (soTienThanhToanThem < soConLai)
            {
                MessageBox.Show(
                    $"Khách chưa thanh toán đủ.\nCòn thiếu: {(soConLai - soTienThanhToanThem):N0}đ",
                    "Thiếu tiền",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            if (soTienThanhToanThem > soConLai)
            {
                MessageBox.Show(
                    $"Số tiền nhập vượt quá số cần thanh toán.\nCòn lại chỉ là: {soConLai:N0}đ",
                    "Sai số tiền",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }


            // -------------------------------
            // 5) LẤY THÔNG TIN KHÁCH
            // -------------------------------
            string khID = DatPhongTongDAL.LayKhachHangID(booking.MaDatTong);
            var khach = KhachHangDAL.LayThongTinKhach(khID);

            // -------------------------------
            // 6) GÁN VÀO DTO HÓA ĐƠN
            // -------------------------------
            var dto = new HoaDonDTO
            {
                Booking = booking,

                TenKhach = khach.HoTen,
                DiaChi = khach.DiaChi,
                SDT = khach.SDT,

                Phong = booking.PhongID,
                NgayDen = booking.NgayNhan,
                NgayDi = booking.NgayTra,

                NhanVien = NVBLL.LayTenNhanVien(SessionInfo.NhanVienID),
                NhanVienID = SessionInfo.NhanVienID,
                ChiTiet = InvoiceDetailsGrid.Items.Cast<InvoiceItemDTO>().ToList(),

                // GIÁ TRỊ TỪ TỔNG KẾT
                TongTienHang = tongKet.TongTienPhong + tongKet.TongTienDichVu,
                GiamGia = tongKet.GiamGia,
                PhanTramGiamGia = phanTramGiamGia,
                VAT = tongKet.VAT,

                TienCoc = tienCoc,
                SoTienThanhToanThem = soTienThanhToanThem,

                ConLai = tongKet.ConLai - soTienThanhToanThem
            };

            // -------------------------
            // 7. SINH MÃ HOÁ ĐƠN
            // -------------------------
            dto.MaHoaDon = HoaDonDAL.TaoMaHoaDon(booking.MaCode);

            // -------------------------
            // 8. LƯU HÓA ĐƠN
            // -------------------------
            HoaDonDAL.LuuHoaDon(dto);

            // -------------------------
            // 9. CẬP NHẬT TRẠNG THÁI
            // -------------------------
            if (dto.ConLai <= 0)
            {
                // Phòng → Bẩn
                PhongDAL.UpdateTrangThaiPhong(booking.PhongID, "Bẩn");

                // Đặt phòng tổng → Đã hoàn tất thanh toán
                
                DatPhongTongDAL.CapNhatTrangThaiDatPhongTong(booking.MaDatTong);
            }

            //HoaDonBLL.InHoaDon(dto);
            // 1) IN HÓA ĐƠN → TRẢ VỀ ĐƯỜNG DẪN PDF
            string pdfPath = HoaDonBLL.InHoaDon(dto);

            if (string.IsNullOrEmpty(pdfPath))
            {
                MessageBox.Show("Không thể gửi email vì hóa đơn chưa được lưu.");
                return;
            }

            // 2) EMAIL HTML
            string emailBody = $@"
                <h2>💳 XÁC NHẬN THANH TOÁN</h2>
                <p>Chào <b>{dto.TenKhach}</b>,</p>
                <p>Bạn đã thanh toán thành công cho Booking <b>{dto.Booking.MaCode}</b>.</p>

                <p><b>Tóm tắt:</b></p>
                <ul>
                    <li>Phòng: {dto.Phong}</li>
                    <li>Đã thanh toán thêm: {dto.SoTienThanhToanThem:N0} VNĐ</li>
                    <li>Còn lại: {dto.ConLai:N0} VNĐ</li>
                </ul>

                <p>Hóa đơn chi tiết được đính kèm trong file PDF.</p>
                <p>Trân trọng!</p>
                ";

            // 3) GỬI EMAIL KÈM FILE PDF
            string customerEmail = KhachHangBLL.LayEmailKhach(dto.SDT);

            if (string.IsNullOrEmpty(customerEmail))
            {
                MessageBox.Show("Không tìm thấy email của khách hàng.");
                return;
            }

            await EmailService.SendEmailWithAttachmentAsync(
                customerEmail,
                $"Hóa đơn thanh toán – Booking #{dto.Booking.MaCode}",
                emailBody,
                pdfPath
            );

            MessageBox.Show("Đã gửi hóa đơn PDF đến email khách!", "Email");


            MessageBox.Show("Thanh toán thành công!", "Thông báo");
            _log.GhiThaoTac(
                "Ghi nhận thanh toán",
                $"{SessionInfo.TenDangNhap} đã thanh toán {soTienThanhToanThem:N0}đ cho Booking #{booking.MaCode}"
            );

            LoadTongKet();

        }

        private void LoadTongKet()
        {
            // 1. Lấy danh sách phòng & dịch vụ (đoạn của cậu giữ nguyên)
            // Lấy 1 phòng chính
            var room = PhongDAL.GetRoomFullInfo(booking.MaDatChiTiet);
            var dsPhong = new List<RoomFullChargeDTO> { room };
            var dsDichVu = DichVuDAL.GetUsedServices(room.MaDatChiTiet);

            // Nếu chọn tất cả phòng → thay thế dsPhong & dsDichVu
            if (AllPhong.IsChecked == true)
            {
                dsPhong = PhongDAL.GetAllRoomsByCustomerName(booking.TenKhach);

                var dsMaCT = dsPhong.Select(p => p.MaDatChiTiet).ToList();
                dsDichVu = DichVuDAL.GetServicesByListChiTiet(dsMaCT);
            }

            // 2. Tiền cọc
            decimal tienCoc = DatPhongTongDAL.LayTienCoc(booking.MaDatTong);

            // 3. ĐÃ THU TRƯỚC ĐÓ (gom tất cả lần thanh toán trước)
            decimal daThuTruocDo = HoaDonBLL.LayTongDaThuTheoMaDatTong(booking.MaDatTong);

            // 4. Tính tổng (đã có giảm giá phanTramGiamGia)
            var tongKet = ThanhToanTongHopBLL.TinhTongKetThanhToan(
                dsPhong,
                dsDichVu,
                tienCoc,
                daThuTruocDo,
                phanTramGiamGia
            );

            // 5. Cập nhật UI
            CapNhatUIGiaTri(tongKet);

            // Nếu đã hết tiền → khóa nút ghi nhận thanh toán
            if (tongKet.ConLai <= 0)
            {
                GhiNhanThanhToanButton.IsEnabled = false;
            }
        }

        private void SoTienThanhToantb_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null) return;

            string raw = tb.Text.Replace(".", "").Replace(",", "");
            if (string.IsNullOrEmpty(raw)) return;

            // Nếu chưa phải số thì bỏ qua
            if (!decimal.TryParse(raw, out decimal value)) return;

            int caretPos = tb.SelectionStart; // vị trí con trỏ trước khi format

            tb.Text = value.ToString("#,##0", new CultureInfo("vi-VN"));

            // Cập nhật vị trí con trỏ = cuối text
            tb.SelectionStart = tb.Text.Length;
        }

        //private void OnlyNumber(object sender, TextCompositionEventArgs e)
        //{

        //    e.Handled = !char.IsDigit(e.Text, 0);
        //    MessageBox.Show("hãy nhập số hợp lệ", "Thông báo");
        
        private void SoTienThanhToantb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }

        private void SoTienThanhToantb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space ||
                e.Key == Key.OemMinus ||
                e.Key == Key.Subtract ||
                e.Key == Key.OemPlus)
            {
                e.Handled = true;
            }
        }
        private void SoTienThanhToantb_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pasted = (string)e.DataObject.GetData(typeof(string));

                if (!Regex.IsMatch(pasted, @"^[0-9]+$"))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
        
    }
}
