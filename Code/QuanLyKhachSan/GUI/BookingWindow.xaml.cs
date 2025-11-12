using System;
using System.Collections.Generic;
using System.Globalization;
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
using BLL;
using DTO;

namespace GUI
{
    /// <summary>
    /// Interaction logic for KiemTraTinhTrangPhong.xaml
    /// </summary>
    public partial class BookingWindow : Window
    {
        private List<PhongDTO> allRooms = new List<PhongDTO>();
        private List<dynamic> selectedRooms = new List<dynamic>();
        public BookingWindow()
        {
            InitializeComponent();
            LoadLoaiPhong();
            LoadTang();
            LoadDanhSachPhong();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        // Closes the window
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainDashboard dashboard = new MainDashboard();
            dashboard.Show();
        }

        //Bỏ vô hiệu hóa nhập thông tin khách hàng mới khi checkbox được chọn
        private void chkKhachHangMoi_Checked(object sender, RoutedEventArgs e)
        {
            SetThongTinKhachHangEnabled(true);
        }

        private void chkKhachHangMoi_Unchecked(object sender, RoutedEventArgs e)
        {
            SetThongTinKhachHangEnabled(false);
        }

        private void SetThongTinKhachHangEnabled(bool enable)
        {
            txtTenKH.IsEnabled = enable;
            txtSoDienThoai.IsEnabled = enable;
            txtEmail.IsEnabled = enable;
            txtDiaChi.IsEnabled = enable;
            txtTenCongTy.IsEnabled = enable;
            txtMaSoThue.IsEnabled = enable;
        }
        //Load danh sách phòng khi cửa sổ được tải
        private void LoadDanhSachPhong()
        {
            var dsPhong = PhongBLL.LayDanhSachPhong();
            allRooms = dsPhong;
            DataGridPhong.ItemsSource = dsPhong;
        }
        
        /// <summary>
        /// LOC PHÒNG THEO LOẠI VÀ TẦNG
        /// </summary>
        private void LoadLoaiPhong()
        {
            var list = LoaiPhongBLL.LayDanhSachLoaiPhong();
            list.Insert(0, "Tất cả");
            cbLoaiPhong.ItemsSource = list;
            cbLoaiPhong.SelectedIndex = 0;
        }

        private void LoadTang()
        {
            cbTang.Items.Add("Tất cả");
            cbTang.Items.Add("Tầng 1");
            cbTang.Items.Add("Tầng 2");
            cbTang.Items.Add("Tầng 3");
            cbTang.Items.Add("Tầng 4");
            cbTang.SelectedIndex = 0;
        }
        
        private void FilterRooms()
        {
            var filtered = allRooms.AsEnumerable();

            // Lọc theo loại phòng
            if (cbLoaiPhong.SelectedIndex > 0)
            {
                string selectedLoai = cbLoaiPhong.SelectedItem.ToString();
                filtered = filtered.Where(p => p.TenLoai.Equals(selectedLoai, StringComparison.OrdinalIgnoreCase));
            }

            // Lọc theo tầng
            if (cbTang.SelectedIndex > 0)
            {
                int tang = cbTang.SelectedIndex; // Tầng 1 => index 1, Tầng 2 => index 2
                filtered = filtered.Where(p =>
                    !string.IsNullOrEmpty(p.SoPhong) &&
                    p.SoPhong.Length > 0 &&
                    p.SoPhong.StartsWith(tang.ToString())
                );
            }

            DataGridPhong.ItemsSource = filtered.ToList();
        }

        private void cbLoaiPhong_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (IsLoaded) FilterRooms();
        }

        private void cbTang_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (IsLoaded) FilterRooms();
        }
        private void BtnKiemTraPhong_Click(object sender, RoutedEventArgs e)
        {
            if (IsLoaded) FilterRooms();
        }
        private void BtnThemPhong_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Thêm phòng được nhấn!");
            if (sender is Button btn && btn.DataContext is PhongDTO phong)
            {
                if (dpNgayNhan.SelectedDate == null || dpNgayTra.SelectedDate == null)
                {
                    MessageBox.Show("Vui lòng chọn ngày nhận và ngày trả phòng.");
                    return;
                }

                DateTime ngayNhan = dpNgayNhan.SelectedDate.Value;
                DateTime ngayTra = dpNgayTra.SelectedDate.Value;

                if (ngayTra <= ngayNhan)
                {
                    MessageBox.Show("Ngày trả phải sau ngày nhận.");
                    return;
                }

                if (selectedRooms.Any(r => r.RoomId == phong.PhongID))
                {
                    MessageBox.Show("Phòng này đã được chọn, không thể chọn trùng!",
                                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Số ngày thuê
                int soNgay = (ngayTra - ngayNhan).Days;

                // Tính thành tiền
                decimal thanhTien = phong.GiaCoBan * soNgay;

                // Thêm vào danh sách phòng đã chọn
                selectedRooms.Add(new
                {
                    RoomId = phong.PhongID,
                    RoomType = phong.TenLoai,
                    Price = phong.GiaCoBan,
                    Total = thanhTien
                });

                // Cập nhật DataGrid
                dgSelectedRooms.ItemsSource = null;
                dgSelectedRooms.ItemsSource = selectedRooms;

                // Cập nhật tổng tiền
                decimal tongTien = selectedRooms.Sum(r => (decimal)r.Total);
                tbTongTien.Text = $"{tongTien:N0} VNĐ";
            }
        }

        private void BtnXoaPhong_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext != null)
            {
                var row = btn.DataContext;
                selectedRooms.Remove(row);
                dgSelectedRooms.ItemsSource = null;
                dgSelectedRooms.ItemsSource = selectedRooms;

                decimal tongTien = selectedRooms.Sum(r => (decimal)r.Total);
                tbTongTien.Text = $"{tongTien:N0} VNĐ";
            }
        }


    }

}
