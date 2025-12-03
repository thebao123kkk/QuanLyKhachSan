using BLL.LoginAndPermission;
using DAL;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Data.SqlClient;
namespace GUI
{

    public partial class Login : Window
    {
        private readonly LoginBLL _loginBll = new LoginBLL();
        public Login()
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;
        }

        //---------Load sự kiện---------
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DatabaseAccess.CanConnect())
            {
                MessageBox.Show(
                    "Không thể kết nối đến cơ sở dữ liệu.\nVui lòng cấu hình lại trước khi đăng nhập.",
                    "Lỗi kết nối",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                DbConfigWindow config = new DbConfigWindow();
                bool? result = config.ShowDialog();

                // Nếu vẫn không lưu hoặc thoát → đóng ứng dụng luôn
                if (result != true || !DatabaseAccess.CanConnect())
                {
                    Application.Current.Shutdown();
                    return;
                }
            }

            // Thiết lập ComboBox Vai trò
            RoleDAL roleDal = new RoleDAL();
            var dsRole = roleDal.GetAllRoles();

            RoleComboBox.Items.Clear();
            foreach (var r in dsRole)
            {
                RoleComboBox.Items.Add(new ComboBoxItem { Content = r });
            }
            
        }
        private void visibleChanged(object sender, RoutedEventArgs e)
        {
            passlabel.Visibility =
                string.IsNullOrEmpty(PasswordBox.Password)
                ? Visibility.Visible
                : Visibility.Collapsed;
            if (PasswordBox.Password.Length < 6)
            {
                PassError.Visibility = Visibility.Visible;
            }
            else
            {
                PassError.Visibility = Visibility.Collapsed;
            }
        }
        //---------View pass---------
        private bool isPasswordVisible = false;

        private void EyeButton_Click(object sender, RoutedEventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                // Hiện mật khẩu
                PasswordVisibleBox.Text = PasswordBox.Password;
                PasswordVisibleBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;

                // Icon mắt mở (Visible) - Chuẩn Material Design
                EyeIcon.Data = Geometry.Parse("M12,4.5C7,4.5 2.73,7.61 1,12c1.73,4.39 6,7.5 11,7.5s9.27-3.11 11-7.5C21.27,7.61 17,4.5 12,4.5z M12,17c-2.76,0-5-2.24-5-5s2.24-5 5-5s5,2.24 5,5S14.76,17 12,17z M12,9c-1.66,0-3,1.34-3,3s1.34,3 3,3s3-1.34 3-3S13.66,9 12,9z");
            }
            else
            {
                // Ẩn mật khẩu
                PasswordBox.Password = PasswordVisibleBox.Text;
                PasswordVisibleBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;

                // Icon mắt gạch chéo (Hidden) - Chuẩn Material Design
                EyeIcon.Data = Geometry.Parse("M11.83,9L15,12.17C15,12.11 15,12.06 15,12c0-1.66-1.34-3-3-3C11.94,9 11.89,9 11.83,9z M17.51,14.68l2.58,2.58c1.37-1.45 2.44-3.12 3.12-4.93c-1.84-4.9-6.38-8.33-11.71-8.33c-2.22,0-4.32,0.6-6.19,1.66l2.56,2.56C8.8,7.74 10.35,7.5 12,7.5c2.76,0 5,2.24 5,5C17,13.25 16.82,13.97 17.51,14.68z M2.81,2.81L1.39,4.22l2.27,2.27C2.18,8.23 1.15,10.27 1,12.5c1.84,4.9 6.38,8.33 11.71,8.33c2.39,0 4.63-0.65 6.58-1.79l2.49,2.49l1.41-1.41L2.81,2.81z M10.5,12.5c0,0.83 0.67,1.5 1.5,1.5c0.12,0 0.23-0.02 0.34-0.04L10.46,12.1C10.48,12.23 10.5,12.36 10.5,12.5z");
            }
        }

        //---------Sự kiện nút đăng nhập---------

        // Cho phép kéo thả cửa sổ (vì WindowStyle="None")
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Gọi BLL
            try
            {
                var result = _loginBll.DangNhap(username, password, role);

                if (!result.Success)
                {
                    ShowMessage(result.Message, Colors.Red);
                    return;
                }

                ShowMessage("Đăng nhập thành công!", Colors.Green);

                // Mở màn hình chính
                MainDashboard main = new MainDashboard();
                main.Show();

                this.Close();
            }
            catch (Exception ex)
            {
                ShowMessage("Có lỗi khi đăng nhập: " + ex.Message, Colors.Red);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Xử lý sự kiện Quên mật khẩu
        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            // Theo SRS 4.6: "Chức năng Quên mật khẩu qua email"
            MessageBox.Show("Vui lòng liên hệ Quản trị viên (Admin) để đặt lại mật khẩu hoặc kiểm tra email của bạn.",
                            "Quên mật khẩu", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Xử lý sự kiện mở cửa sổ Cấu hình DB
        private void DbConfigButton_Click(object sender, RoutedEventArgs e)
        {
            DbConfigWindow dbConfig = new DbConfigWindow();

            // Sử dụng ShowDialog để mở cửa sổ dạng modal (người dùng phải đóng nó trước khi quay lại login)
            dbConfig.ShowDialog();
        }

        // Hàm tiện ích hiển thị thông báo lỗi/thành công ngay trên form
        private void ShowMessage(string message, Color color)
        {
            MessageLabel.Text = message;
            MessageLabel.Foreground = new SolidColorBrush(color);
            MessageLabel.Visibility = Visibility.Visible;
        }
    }
}