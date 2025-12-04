using BLL;
using BLL.LoginAndPermission;
using DAL;
using DAL.DashBoard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

    public partial class MainDashboard : Window
    {
        HienThiDAL hienThiDAL = new HienThiDAL();
        public MainDashboard()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            ApplyRolePermissions();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Hiển thị welcome text với tên người dùng và vai trò
            string[] textUandR = hienThiDAL.GetUserNameAndRoleName(SessionInfo.TenDangNhap, SessionInfo.VaiTroID);
            WelcomeTextBlock.Text = $"Xin chào, {textUandR[0]} - Vai trò: {textUandR[1]}";


            LoadDashboardStats();
        }



        private void LoadDashboardStats()
        {
            DashBoardBLL bll = new DashBoardBLL();
            DateTime today = DateTime.Today;

            // 1. Phòng đang ở
            var phong = bll.LayPhongDangO();
            PhongTrangThaiDaNhan.Text = $"{phong.soDangO} / {phong.tongPhong}";

            // 2. Check-in hôm nay
            int checkIns = bll.LaySoCheckIns(today);
            StatCheckIns.Text = checkIns.ToString();

            // 3. Check-out hôm nay
            int checkOuts = bll.LaySoCheckOuts(today);
            StatCheckOuts.Text = checkOuts.ToString();

            // 4. Phòng bẩn
            int soPhongBan = bll.LaySoPhongBan();
            StatDirtyRooms.Text = soPhongBan.ToString();
        }


        private void ApplyRolePermissions()
        {
            // Reset tất cả về Disable
            StatsPanel.IsEnabled = false;
            BookingButton.IsEnabled = false;
            SearchRoomButton.IsEnabled = false;
            CheckoutButton.IsEnabled = false;
            HousekeepingButton.IsEnabled = false;
            ReportsButton.IsEnabled = false;
            AdminButton.IsEnabled = false;

            string[] textUandR = hienThiDAL.GetUserNameAndRoleName(SessionInfo.TenDangNhap, SessionInfo.VaiTroID);

            string role = textUandR[1].ToLower();

            // 1️ QUẢN LÝ
            if (role == "quản lý")
            {
                ReportsButton.IsEnabled = true;   // mở
                AdminButton.IsEnabled = true;     // mở
                return;
            }

            // 2️ LỄ TÂN
            if (role == "lễ tân")
            {
                StatsPanel.IsEnabled = true;
                BookingButton.IsEnabled = true;
                SearchRoomButton.IsEnabled = true;
                CheckoutButton.IsEnabled = true;

                return;
            }

            // 3️ BUỒNG PHÒNG
            if (role == "buồng phòng")
            {
                HousekeepingButton.IsEnabled = true;
                return;
            }

            // Vai trò không hợp lệ
            MessageBox.Show("Không xác định được vai trò người dùng.",
                "Lỗi phân quyền", MessageBoxButton.OK, MessageBoxImage.Error);

            this.Close();
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
            LogBLL log = new LogBLL();
            log.GhiThaoTac("Đăng xuất",
                $"{SessionInfo.TenDangNhap} đã đăng xuất khỏi hệ thống.");

            // 2️⃣ Kết thúc session (ghi thời gian)
            SessionInfo.EndSession();
            Login loginWindow = new Login();
            loginWindow.Show();

            // Đóng cửa sổ Dashboard này
            this.Close();
        }

        private void BookingButton_Click(object sender, RoutedEventArgs e)
        {
            BookingWindow bookingWindow = new BookingWindow();
            bookingWindow.Show();
            this.Hide();
        }

        private void SearchRoomButton_Click(object sender, RoutedEventArgs e)
        {
            SearchRoom searchRoom = new SearchRoom();
            searchRoom.Show(); // Dùng .Show() để có thể mở nhiều cửa sổ
            this.Hide();

        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            Paiding checkout = new Paiding();
            checkout.Show();
            this.Hide();

        }

        private void HousekeepingButton_Click(object sender, RoutedEventArgs e)
        {
            HousekeepingDashboard housekeeping = new HousekeepingDashboard();
            housekeeping.Show();
            this.Hide();

        }

        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {
            ReportsDashboard reports = new ReportsDashboard();
            reports.Show();
            this.Hide();

        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            AdminDashboard admin = new AdminDashboard();
            admin.Show();
            this.Hide();

        }
    }
}
