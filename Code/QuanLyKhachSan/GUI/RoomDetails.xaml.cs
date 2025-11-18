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
using DTO;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RoomDetails : Window
    {
        public DatPhongViewDTO BookingInfo { get; set; }
        public RoomDetails()
        {
            InitializeComponent();
        }

        public RoomDetails(DatPhongViewDTO info)
        {
            InitializeComponent();
            BookingInfo = info;
            LoadRoomDetails();
            LoadServices();
            LoadUsedServices();
            LoadPaymentInfo();
        }

        private void LoadRoomDetails()
        {
            if (BookingInfo == null) return;

            // Gán tiêu đề: Phòng + Loại phòng
            RoomTitleText.Text = $"Phòng {BookingInfo.SoPhong} - {BookingInfo.LoaiPhong}";

            // Gán tên khách
            GuestNameText.Text = BookingInfo.TenKhach;

            // Tính số đêm
            int soDem = (BookingInfo.NgayTra - BookingInfo.NgayNhan).Days;

            StayDurationText.Text =
                $"{BookingInfo.NgayNhan:dd/MM/yyyy} - {BookingInfo.NgayTra:dd/MM/yyyy} ({soDem} đêm)";

            // Trạng thái phòng
            RoomStatusText.Text = BookingInfo.TrangThai;

            // Tô màu trạng thái
            RoomStatusText.Foreground = (BookingInfo.TrangThai == "Đã nhận")
                ? new SolidColorBrush(Color.FromRgb(40, 167, 69))  // xanh lá
                : new SolidColorBrush(Color.FromRgb(220, 53, 69)); // đỏ
        }
        private List<DichVuPhongDTO> availableServices = new List<DichVuPhongDTO>();
        private List<DichVuPhongDTO> usedServices = new List<DichVuPhongDTO>();
        private List<DichVuPhongDTO> databaseServices = new List<DichVuPhongDTO>(); // load từ DB
        private List<DichVuPhongDTO> addedServices = new List<DichVuPhongDTO>();     // chỉ dịch vụ user mới thêm


        // Mô phỏng tải danh sách dịch vụ đã dùng
        private void LoadServices()
        {
            availableServices = DichVuBLL.LayDichVuHoatDong();

            ServiceComboBox.Items.Clear();

            foreach (var dv in availableServices)
            {
                ServiceComboBox.Items.Add($"{dv.TenDichVu} ({dv.DonVi})");
            }

            if (ServiceComboBox.Items.Count > 0)
                ServiceComboBox.SelectedIndex = 0;

        }

        private void LoadUsedServices()
        {
            databaseServices.Clear();

            var ds = ChiTietDichVuBLL.LoadChiTietDichVu(BookingInfo.MaDatChiTiet);

            foreach (var x in ds)
            {
                databaseServices.Add(new DichVuPhongDTO
                {
                    DichVuID = x.DichVuID,
                    TenDichVu = x.TenDichVu,
                    DonGia = x.DonGiaTaiThoiDiem,
                    DonVi = "",
                    SoLuong = x.SoLuong
                });
            }

            RefreshServiceGrid();
        }


        private void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServiceComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Hãy chọn dịch vụ.");
                return;
            }

            if (!decimal.TryParse(ServiceQuantityTextBox.Text, out decimal quantity) || quantity <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ.");
                return;
            }

            var dv = availableServices[ServiceComboBox.SelectedIndex];

            DichVuPhongDTO item = new DichVuPhongDTO
            {
                DichVuID = dv.DichVuID,
                TenDichVu = dv.TenDichVu,
                DonVi = dv.DonVi,
                DonGia = dv.DonGia,
                HieuLuc = dv.HieuLuc,
                SoLuong = quantity
            };

            addedServices.Add(item);

            RefreshServiceGrid();
        }
        private void RefreshServiceGrid()
        {
            var unitedList = databaseServices.Concat(addedServices).ToList();

            ServicesDataGrid.ItemsSource = null;
            ServicesDataGrid.ItemsSource = unitedList;

            UpdateServiceFee(unitedList);
        }

        private void UpdateServiceFee(List<DichVuPhongDTO> unitedList)
        {
            tienDichVu = unitedList.Sum(x => x.ThanhTien);
            ServiceFeeText.Text = $"{tienDichVu:N0} VNĐ";
            //MessageBox.Show($"Dịch vụ (cập nhật): {tienDichVu:N0} VNĐ", "Debug", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //Tổng tiền 
        private decimal tienPhong = 0;
        private decimal tienCoc = 0;
        private decimal tienDichVu = 0;

        private void LoadPaymentInfo()
        {
            // 1. Tiền phòng
            tienPhong = DatPhongBLL.LayTienPhong(BookingInfo.MaDatChiTiet);
            RoomFeeText.Text = $"{tienPhong:N0} VNĐ";

            // 2. Tiền cọc
            tienCoc = DatPhongBLL.LayTienCoc(BookingInfo.MaDatTong);
            PaidText.Text = $"{tienCoc:N0} VNĐ";

            // 3. TIỀN DỊCH VỤ (đúng)
            tienDichVu =
                databaseServices.Sum(x => x.ThanhTien) +
                addedServices.Sum(x => x.ThanhTien);

            ServiceFeeText.Text = $"{tienDichVu:N0} VNĐ";
            //MessageBox.Show($"Dịch vụ: {tienDichVu:N0} VNĐ", "Debug", MessageBoxButton.OK, MessageBoxImage.Information);
            // 4. Tổng tạm tính 
            decimal tong = tienPhong + tienDichVu - tienCoc;
            TotalDueText.Text = $"{tong:N0} VNĐ";
        }



        // Cho phép kéo thả cửa sổ
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        // Nút đóng cửa sổ
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainDashboard dashboard = new MainDashboard();
            dashboard.Show();
        }

        // Hàm giả lập lấy giá dịch vụ
        private decimal GetMockServicePrice(string serviceName)
        {
            return 0;
        }

        // Xử lý sự kiện nút "Kiểm Tra Khả Dụng" (SRS 4.2.2 / AC-FD4)
        private void CheckAvailabilityButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Đang kiểm tra khả dụng...", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
            if (!NewCheckoutDatePicker.SelectedDate.HasValue)
            {
                AvailabilityText.Text = "Vui lòng chọn ngày trả phòng mới.";
                AvailabilityText.Foreground = Brushes.Red;
                return;
            }

            DateTime newCheckout = NewCheckoutDatePicker.SelectedDate.Value;
            DateTime oldCheckin = BookingInfo.NgayNhan;
            DateTime oldCheckout = BookingInfo.NgayTra;

            if (newCheckout <= oldCheckout)
            {
                AvailabilityText.Text = "Ngày trả mới phải lớn hơn ngày trả hiện tại.";
                AvailabilityText.Foreground = Brushes.Red;
                return;
            }

            // 1) KIỂM TRA TRÙNG LỊCH PHÒNG
            bool isConflict = DatPhongBLL.KiemTraPhongTrungLich(
                BookingInfo.PhongID,
                newCheckout,
                oldCheckin,
                BookingInfo.MaDatChiTiet
            );

            if (isConflict)
            {
                AvailabilityText.Text = "Không thể gia hạn: trùng với lịch đặt phòng của khách khác.";
                AvailabilityText.Foreground = Brushes.Red;
                return;
            }

            // 2) XÁC NHẬN GIA HẠN
            var result = MessageBox.Show(
                $"Xác nhận gia hạn đến ngày {newCheckout:dd/MM/yyyy}?",
                "Xác nhận gia hạn phòng",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.No)
            {
                AvailabilityText.Text = "Bạn đã hủy gia hạn.";
                AvailabilityText.Foreground = Brushes.Gray;
                return;
            }

            // 3) TÍNH TIỀN PHÒNG MỚI
            // Lấy giá cơ bản hiện tại từ LoaiPhongChiTiet
            decimal giaCoBan = DatPhongBLL.LayGiaPhongHienTai(BookingInfo.PhongID);

            // Số đêm cũ và mới
            int soDemCu = (oldCheckout - oldCheckin).Days;
            int soDemMoi = (newCheckout - oldCheckin).Days;

            // Số đêm gia hạn thêm
            int themDem = soDemMoi - soDemCu;

            // Tiền phòng cũ
            decimal tienPhongCu = DatPhongBLL.LayTienPhong(BookingInfo.MaDatChiTiet);

            // Tiền gia hạn thêm
            decimal tienGiaHanThem = giaCoBan * themDem;

            // Tổng tiền phòng mới
            decimal tienPhongMoi = tienPhongCu + tienGiaHanThem;

            // 4) UPDATE DB: cập nhật ngày trả & tiền phòng mới
            bool updated = DatPhongBLL.GiaHanPhong(
                BookingInfo.MaDatChiTiet,
                newCheckout,
                tienPhongMoi
            );

            if (!updated)
            {
                AvailabilityText.Text = "Lỗi khi lưu gia hạn!";
                AvailabilityText.Foreground = Brushes.Red;
                return;
            }

            // 5) CẬP NHẬT UI SAU KHI GIA HẠN
            BookingInfo.NgayTra = newCheckout; // cập nhật lại model

            RoomFeeText.Text = $"{tienPhongMoi:N0} VNĐ";   // cập nhật tiền phòng mới

            LoadPaymentInfo();  // tính lại tổng tiền (phòng + dịch vụ – cọc)

            AvailabilityText.Text = "Gia hạn thành công!";
            AvailabilityText.Foreground = Brushes.Green;
        }



        // Hàm giả lập kiểm tra tính khả dụng (AC-FD4)
        private bool CheckRoomAvailability(DateTime newDate)
        {
            // Giả lập: Luôn trống nếu ngày trong tương lai gần
            // Giả lập: Bị trùng nếu chọn ngày 30/10/2025
            if (newDate.Day == 30 && newDate.Month == 10) return false;

            return newDate > DateTime.Now;
        }

        // Cập nhật tổng tiền
        private void UpdateTotalDue()
        {
            
        }


        // Nút lưu thay đổi (cho gia hạn, dịch vụ)
        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            // LƯU DỊCH VỤ MỚI VÀO DB
            bool ok = ChiTietDichVuBLL.LuuChiTietDichVu(BookingInfo.MaDatChiTiet, addedServices);

            if (!ok)
            {
                MessageBox.Show("Lưu dịch vụ thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Đã lưu thay đổi thành công!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);

            // Load lại dịch vụ từ DB sau khi lưu
            addedServices.Clear();
            LoadUsedServices();
            MessageBox.Show("Đã lưu thay đổi thành công!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close(); // Đóng cửa sổ sau khi lưu
        }

        // Nút chuyển đến màn hình thanh toán
        private void GoToCheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Logic mở cửa sổ Thanh Toán (Checkout)

            MessageBox.Show("Chuyển đến màn hình Thanh Toán...", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }
}
