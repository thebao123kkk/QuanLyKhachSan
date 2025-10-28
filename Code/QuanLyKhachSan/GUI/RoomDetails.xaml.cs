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

namespace GUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RoomDetails : Window
    {
        public RoomDetails()
        {
            InitializeComponent();
        }

        private void LoadRoomDetails()
        {
            // Trong ứng dụng thực tế, bạn sẽ tải dữ liệu này từ CSDL dựa trên ID phòng/booking

            UpdateTotalDue();
        }

        // Mô phỏng tải danh sách dịch vụ đã dùng
        private void LoadUsedServices()
        {
            // Trong ứng dụng thực tế, tải từ CSDL

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
        }

        // Xử lý sự kiện nút "Thêm Dịch Vụ" (SRS 4.2.3)
        private void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            // Logic để thêm dịch vụ
           
        }

        // Hàm giả lập lấy giá dịch vụ
        private decimal GetMockServicePrice(string serviceName)
        {
            return 0;
        }

        // Xử lý sự kiện nút "Kiểm Tra Khả Dụng" (SRS 4.2.2 / AC-FD4)
        private void CheckAvailabilityButton_Click(object sender, RoutedEventArgs e)
        {
            
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
            // Logic lưu tất cả thay đổi (dịch vụ mới, ngày gia hạn) vào CSDL
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
