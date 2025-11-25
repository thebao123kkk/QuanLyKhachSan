using BLL.LoginAndPermission;
using DAL;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
            // Thiết lập ComboBox Vai trò
            RoleDAL roleDal = new RoleDAL();
            var dsRole = roleDal.GetAllRoles();

            RoleComboBox.Items.Clear();
            foreach (var r in dsRole)
            {
                RoleComboBox.Items.Add(new ComboBoxItem { Content = r });
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