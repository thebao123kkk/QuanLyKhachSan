using DAL;
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
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Window
    {
        public AdminDashboard()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
        }

        //----Load sự kiện----
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

        // --- Logic Cửa Sổ Chính ---

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainDashboard dashboard = new MainDashboard();
            dashboard.Show();
        }

        // --- TAB 1: QUẢN LÝ NHÂN VIÊN (SRS 4.6) ---

        private void LoadUsersGrid()
        {
            // Dữ liệu giả lập cho danh sách nhân viên (đã cập nhật đầy đủ trường)

        }

        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ClearFormButton_Click(object sender, RoutedEventArgs e)
        {
            // Làm mới các trường nhập liệu
            NhanVienIDTextBox.Clear();
            HoTenTextBox.Clear();
            ChucVuTextBox.Clear();
            DienThoaiTextBox.Clear();
            EmailTextBox.Clear();
            DiaChiTextBox.Clear();
            PasswordTextBox.Clear();
            GioiTinhComboBox.SelectedIndex = -1;
            RoleComboBox.SelectedIndex = -1;
            NgaySinhDatePicker.SelectedDate = null;

            UsersDataGrid.SelectedItem = null;

            // Cho phép nhập Mã NV khi thêm mới
            NhanVienIDTextBox.IsReadOnly = false;
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            // Logic: Lấy dữ liệu từ tất cả các TextBox, ComboBox, DatePicker

            LoadUsersGrid(); // Tải lại danh sách
            ClearFormButton_Click(sender, e); // Làm mới form
        }

        private void UpdateUserButton_Click(object sender, RoutedEventArgs e)
        {

            LoadUsersGrid();
            ClearFormButton_Click(sender, e);
        }

        private void LockUserButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UnlockUserButton_Click(object sender, RoutedEventArgs e)
        {

        }

        // --- TAB 2: QUẢN LÝ PHÊ DUYỆT (SRS 4.2.2) ---
        // (Không thay đổi logic)

        private void LoadApprovalRequestsGrid()
        {
            // Dữ liệu giả lập cho các yêu cầu cần phê duyệt

        }

        private void ApproveButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
