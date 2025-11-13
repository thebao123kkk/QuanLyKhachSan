using System;
using System.Configuration; // Cần thêm reference System.Configuration
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI
{
    /// <summary>
    /// Interaction logic for DbConfigWindow.xaml
    /// </summary>
    public partial class DbConfigWindow : Window
    {
        public DbConfigWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        /// <summary>
        /// Tải cài đặt hiện tại từ App.config (nếu có)
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                var connString = ConfigurationManager.ConnectionStrings["QuanLyKhachSanDB"]?.ConnectionString;
                if (!string.IsNullOrEmpty(connString))
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connString);
                    ServerTextBox.Text = builder.DataSource;
                    DatabaseTextBox.Text = builder.InitialCatalog;

                    if (builder.IntegratedSecurity)
                    {
                        AuthModeComboBox.SelectedIndex = 0; // Windows Auth
                    }
                    else
                    {
                        AuthModeComboBox.SelectedIndex = 1; // SQL Auth
                        UsernameTextBox.Text = builder.UserID;
                        // PasswordBox.Password = builder.Password; // Thường không lưu ngược lại pass
                    }
                }
            }
            catch (Exception ex)
            {
                // Bỏ qua nếu chưa có file config
                Console.WriteLine("Không tìm thấy file config cũ: " + ex.Message);
            }
        }

        /// <summary>
        /// Xây dựng chuỗi kết nối dựa trên thông tin nhập vào
        /// </summary>
        private string BuildConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = ServerTextBox.Text;
            builder.InitialCatalog = DatabaseTextBox.Text;

            if (AuthModeComboBox.SelectedIndex == 0) // Windows Authentication
            {
                builder.IntegratedSecurity = true;
            }
            else // SQL Server Authentication
            {
                builder.IntegratedSecurity = false;
                builder.UserID = UsernameTextBox.Text;
                builder.Password = PasswordBox.Password;
            }

            builder.ConnectTimeout = 5; // Đặt timeout ngắn (5s) để test
            return builder.ConnectionString;
        }

        /// <summary>
        /// Ẩn/hiện panel User/Pass dựa trên chế độ xác thực
        /// </summary>
        private void AuthModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SqlAuthPanel == null) return;

            if (AuthModeComboBox.SelectedIndex == 1) // SQL Server Auth
            {
                SqlAuthPanel.Visibility = Visibility.Visible;
            }
            else
            {
                SqlAuthPanel.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Thử kết nối đến CSDL với cài đặt hiện tại
        /// </summary>
        private void TestConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            string connString = BuildConnectionString();
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    MessageBox.Show("Kết nối thành công!", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kết nối thất bại:\n{ex.Message}", "Lỗi Kết Nối", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Lưu chuỗi kết nối vào file App.config và đóng cửa sổ
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connString = BuildConnectionString();

                // Mở file App.config (hoặc Web.config)
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // Xóa (nếu có) và thêm mới connection string
                config.ConnectionStrings.ConnectionStrings.Remove("QuanLyKhachSanDB");
                config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("QuanLyKhachSanDB", connString, "System.Data.SqlClient"));

                // Lưu thay đổi
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");

                MessageBox.Show("Đã lưu cấu hình kết nối thành công!\nỨng dụng có thể cần khởi động lại.", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu file cấu hình:\n{ex.Message}", "Lỗi Lưu Config", MessageBoxButton.OK, MessageBoxImage.Error);
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
            this.DialogResult = false;
            this.Close();
        }
    }
}