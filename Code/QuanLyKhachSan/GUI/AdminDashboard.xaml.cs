using BLL.LoginAndPermission;
using DAL;
using DAL.LoginAndPermission;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private string CurrentNhanVienID;

        public AdminDashboard()
        {
            InitializeComponent();
            LoadRoleComboBox();
            LoadUsersGrid();
        }

        //----Load sự kiện----
        private void LoadRoleComboBox()
        {
            RoleDAL roleDal = new RoleDAL();
            var list = roleDal.GetAllRolesDetail();

            RoleComboBox.Items.Clear();
            foreach (var r in list)
            {
                RoleComboBox.Items.Add(new ComboBoxItem
                {
                    Content = r.TenVaiTro,
                    Tag = r.VaiTroID
                });
            }
        }

        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = RoleComboBox.SelectedItem as ComboBoxItem;
            if (selected == null) return;

            // Lấy tên vai trò = content
            string tenVaiTro = selected.Content.ToString();

            // Set vào textbox chức vụ
            ChucVuTextBox.Text = tenVaiTro;
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
            NhanVienBLL bll = new NhanVienBLL();
            UsersDataGrid.ItemsSource = bll.GetAll();

        }

        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = UsersDataGrid.SelectedItem as NhanVienDTO;
            if (selected == null) return;

            // Load dữ liệu vào form
            CurrentNhanVienID = selected.NhanVienID;
            ChucVuTextBox.Text = selected.ChucVu;
            HoTenTextBox.Text = selected.HoTen;
            DienThoaiTextBox.Text = selected.DienThoai;
            EmailTextBox.Text = selected.Email;
            DiaChiTextBox.Text = selected.DiaChi;

            // Date
            NgaySinhDatePicker.SelectedDate = selected.NgaySinh;

            // --- ComboBox: Giới tính ---
            var itemGT = GioiTinhComboBox.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(i => i.Content.ToString() == selected.GioiTinh);

            GioiTinhComboBox.SelectedItem = itemGT;

            // --- ComboBox: Vai trò ---
            foreach (ComboBoxItem item in RoleComboBox.Items)
            {
                if (item.Tag?.ToString() == selected.VaiTroID)
                {
                    RoleComboBox.SelectedItem = item;
                    break;
                }
            }
            RoleComboBox.IsEnabled = false; // Khóa không cho thay đổi vai trò

        }

        private void ClearFormButton_Click(object sender, RoutedEventArgs e)
        {
            // Làm mới các trường nhập liệu
            ChucVuTextBox.Clear();
            HoTenTextBox.Clear();
            DienThoaiTextBox.Clear();
            EmailTextBox.Clear();
            DiaChiTextBox.Clear();
            PasswordTextBox.Clear();
            GioiTinhComboBox.SelectedIndex = -1;
            RoleComboBox.SelectedIndex = -1;
            RoleComboBox.IsEnabled = true;
            NgaySinhDatePicker.SelectedDate = null;

            UsersDataGrid.SelectedItem = null;

        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRole = RoleComboBox.SelectedItem as ComboBoxItem;
            string roleID = selectedRole?.Tag?.ToString();
            NhanVienDTO nv = new NhanVienDTO
            {
                ChucVu = ChucVuTextBox.Text,
                HoTen = HoTenTextBox.Text,
                GioiTinh = GioiTinhComboBox.Text,
                NgaySinh = NgaySinhDatePicker.SelectedDate,
                DienThoai = DienThoaiTextBox.Text,
                Email = EmailTextBox.Text,
                DiaChi = DiaChiTextBox.Text,
                VaiTroID = roleID,
            };

            NhanVienBLL bll = new NhanVienBLL();
            string result = bll.Add(nv);

            if (result == "SUCCESS")
            {
                MessageBox.Show("Thêm nhân viên thành công!");
            }
            else
            {
                MessageBox.Show(result); 
            }

            LoadUsersGrid(); // Tải lại danh sách
            ClearFormButton_Click(sender, e); // Làm mới form
        }


        private void UpdateUserButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRole = RoleComboBox.SelectedItem as ComboBoxItem;
            string roleID = selectedRole?.Tag?.ToString();
            NhanVienDTO nv = new NhanVienDTO
            {
                NhanVienID = CurrentNhanVienID,
                ChucVu = ChucVuTextBox.Text,
                HoTen = HoTenTextBox.Text,
                GioiTinh = GioiTinhComboBox.Text,
                NgaySinh = NgaySinhDatePicker.SelectedDate,
                DienThoai = DienThoaiTextBox.Text,
                Email = EmailTextBox.Text,
                DiaChi = DiaChiTextBox.Text,
                VaiTroID = roleID,
            };

            NhanVienBLL bll = new NhanVienBLL();
            string result = bll.Update(nv);

            if (result == "SUCCESS")
            {
                MessageBox.Show("Cập nhật nhân viên thành công!");
            }
            else
            {
                MessageBox.Show(result);
            }

            LoadUsersGrid();
            ClearFormButton_Click(sender, e);
        }

        private void LockUserButton_Click(object sender, RoutedEventArgs e)
        {
            var nv = UsersDataGrid.SelectedItem as NhanVienDTO;
            if (nv == null) return;

            NhanVienBLL bll = new NhanVienBLL();
            bll.Lock(nv.NhanVienID);

            LoadUsersGrid();
        }

        private void UnlockUserButton_Click(object sender, RoutedEventArgs e)
        {
            var nv = UsersDataGrid.SelectedItem as NhanVienDTO;
            if (nv == null) return;

            NhanVienBLL bll = new NhanVienBLL();
            bll.Unlock(nv.NhanVienID);

            LoadUsersGrid();
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

        // --- TAB 2: NHẬP DỮ LIỆU TỪ EXCEL ---
        private void ImportExcelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveImportButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
