using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI
{
    public partial class DbConfigWindow : Window
    {
        private const string ConnName = "QuanLyKhachSanDB"; // DÙNG CỐ ĐỊNH 1 TÊN

        public DbConfigWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        // ---------------------------------------------------------
        // 1) Load config hiện tại từ App.config
        // ---------------------------------------------------------
        private void LoadSettings()
        {
            try
            {
                var connString = ConfigurationManager.ConnectionStrings[ConnName]?.ConnectionString;

                if (!string.IsNullOrEmpty(connString))
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connString);

                    ServerTextBox.Text = builder.DataSource;
                    DatabaseTextBox.Text = builder.InitialCatalog;

                    if (builder.IntegratedSecurity)
                    {
                        AuthModeComboBox.SelectedIndex = 0;   // Windows Auth
                    }
                    else
                    {
                        AuthModeComboBox.SelectedIndex = 1;   // SQL Auth
                        UsernameTextBox.Text = builder.UserID;
                    }
                }
            }
            catch { }
        }

        // ---------------------------------------------------------
        // 2) Xây dựng connection string
        // ---------------------------------------------------------
        private string BuildConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = ServerTextBox.Text,
                InitialCatalog = DatabaseTextBox.Text,
                ConnectTimeout = 5
            };

            if (AuthModeComboBox.SelectedIndex == 0)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.IntegratedSecurity = false;
                builder.UserID = UsernameTextBox.Text;
                builder.Password = PasswordBox.Password;
            }

            return builder.ConnectionString;
        }

        // ---------------------------------------------------------
        // 3) Thử kết nối
        // ---------------------------------------------------------
        private void TestConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connString = BuildConnectionString();

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                }

                MessageBox.Show("Kết nối thành công!", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kết nối thất bại:\n" + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ---------------------------------------------------------
        // 4) Lưu vào App.config đúng chuẩn
        // ---------------------------------------------------------
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connString = BuildConnectionString();

                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // Xóa cũ – thêm mới
                if (config.ConnectionStrings.ConnectionStrings[ConnName] != null)
                    config.ConnectionStrings.ConnectionStrings.Remove(ConnName);

                config.ConnectionStrings.ConnectionStrings.Add(
                    new ConnectionStringSettings(ConnName, connString, "System.Data.SqlClient")
                );

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");

                MessageBox.Show(
                    "Đã lưu cấu hình thành công!\nVui lòng khởi động lại ứng dụng.",
                    "Thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                this.DialogResult = true;
                this.Close();
                //// --------------- 🎯 TỰ ĐỘNG RESTART CHƯƠNG TRÌNH ----------------
                //string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

                //// Mở lại ứng dụng
                //System.Diagnostics.Process.Start(exePath);

                //// Tắt ứng dụng hiện tại
                //Application.Current.Shutdown();
                //// -
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu cấu hình:\n" + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ---------------------------------------------------------
        // UI
        // ---------------------------------------------------------
        private void AuthModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SqlAuthPanel == null) return;
            SqlAuthPanel.Visibility = (AuthModeComboBox.SelectedIndex == 1) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
