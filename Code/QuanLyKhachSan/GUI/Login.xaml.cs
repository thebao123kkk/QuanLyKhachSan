using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Hide previous message
            MessageLabel.Visibility = Visibility.Collapsed;

            // Get user input
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // --- Validation Logic ---
            // In a real app, this would call a business logic layer (BLL).
            bool isValid = false;
            string welcomeMessage = "";

            if (username.ToLower() == "manager" && password == "123" && role == "Quản lý")
            {
                isValid = true;
                welcomeMessage = "Đăng nhập thành công! Chuyển đến trang Quản trị...";
            }
            else if (username.ToLower() == "employee" && password == "123" && role == "Nhân viên")
            {
                isValid = true;
                welcomeMessage = "Đăng nhập thành công! Chào mừng nhân viên...";
            }

            if (isValid)
            {
                // Show success message
                MessageLabel.Text = welcomeMessage;
                MessageLabel.Foreground = new SolidColorBrush(Colors.Green);
                MessageLabel.Visibility = Visibility.Visible;

                // Simulate navigating to another window/page
                await Task.Delay(2000);

                // For example, open the main application window and close this one
                // var mainAppWindow = new MainApplicationWindow(role);
                // mainAppWindow.Show();
                // this.Close(); 
                MessageBox.Show($"Đã chuyển hướng với vai trò: {role}", "Thông báo");
            }
            else
            {
                // Show error message
                MessageLabel.Text = "Tên đăng nhập, mật khẩu hoặc vai trò không chính xác!";
                MessageLabel.Foreground = new SolidColorBrush(Colors.Red);
                MessageLabel.Visibility = Visibility.Visible;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the application
            Application.Current.Shutdown();
        }
    }
}