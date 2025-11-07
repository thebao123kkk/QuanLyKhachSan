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
    /// Interaction logic for MainDashboard.xaml
    /// </summary>
    public partial class MainDashboard : Window
    {
        public MainDashboard()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Hiển thị thông tin chào mừng
            //WelcomeTextBlock.Text = $"Xin chào, {currentUsername}! (Vai trò: {currentUserRole})";

            // Áp dụng phân quyền (quan trọng)
            //ApplyRolePermissions(currentUserRole);

            // Tải các số liệu thống kê nhanh
            LoadDashboardStats();
        }

        private void LoadDashboardStats()
        {
            // Logic: Gọi BLL/DAO để lấy các số liệu thống kê nhanh trong ngày



        }


        private void ApplyRolePermissions(string role)
        {
            // Mặc định ẩn các nút quản trị và thống kê
            ReportsButton.Visibility = Visibility.Collapsed;
            AdminButton.Visibility = Visibility.Collapsed;
            StatsPanel.Visibility = Visibility.Collapsed;

            // Mặc định ẩn các nút nghiệp vụ
            BookingButton.Visibility = Visibility.Collapsed; // (MỚI)
            SearchRoomButton.Visibility = Visibility.Collapsed;
            CheckoutButton.Visibility = Visibility.Collapsed;
            HousekeepingButton.Visibility = Visibility.Collapsed;

            // Phân quyền
            switch (role.ToLower())
            {
                case "quản lý":
                    // Quản lý thấy tất cả
                    StatsPanel.Visibility = Visibility.Visible;
                    BookingButton.Visibility = Visibility.Visible; // (MỚI)
                    SearchRoomButton.Visibility = Visibility.Visible;
                    CheckoutButton.Visibility = Visibility.Visible;
                    HousekeepingButton.Visibility = Visibility.Visible;
                    ReportsButton.Visibility = Visibility.Visible;
                    AdminButton.Visibility = Visibility.Visible;
                    break;

                case "lễ tân":
                    // Lễ tân thấy nghiệp vụ của lễ tân và thống kê nhanh
                    StatsPanel.Visibility = Visibility.Visible;
                    BookingButton.Visibility = Visibility.Visible; // (MỚI)
                    SearchRoomButton.Visibility = Visibility.Visible;
                    CheckoutButton.Visibility = Visibility.Visible;
                    break;

                case "buồng phòng":
                    // Buồng phòng chỉ thấy nghiệp vụ buồng phòng
                    HousekeepingButton.Visibility = Visibility.Visible;
                    break;

                default:
                    // Vai trò lạ
                    MessageBox.Show("Vai trò không xác định. Không có quyền truy cập.", "Lỗi Phân Quyền", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close(); // Đóng dashboard nếu không có quyền
                    break;
            }
        }


        // --- Logic Cửa Sổ Cơ Bản ---

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Đóng ứng dụng khi đóng cửa sổ chính
            Application.Current.Shutdown();
        }


        // --- Xử Lý Mở Các Cửa Sổ Chức Năng ---

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();

            // Đóng cửa sổ Dashboard này
            this.Close();
        }

        private void BookingButton_Click(object sender, RoutedEventArgs e)
        {
            BookingWindow bookingWindow = new BookingWindow();
            bookingWindow.Show();
        }

        private void SearchRoomButton_Click(object sender, RoutedEventArgs e)
        {
            SearchRoom searchRoom = new SearchRoom();
            searchRoom.Show(); // Dùng .Show() để có thể mở nhiều cửa sổ
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            Paiding checkout = new Paiding();
            checkout.Show();
        }

        private void HousekeepingButton_Click(object sender, RoutedEventArgs e)
        {
            HousekeepingDashboard housekeeping = new HousekeepingDashboard();
            housekeeping.Show();
        }

        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {
            ReportsDashboard reports = new ReportsDashboard();
            reports.Show();
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            AdminDashboard admin = new AdminDashboard();
            admin.Show();
        }
    }
}
