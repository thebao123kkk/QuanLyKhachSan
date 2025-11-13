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
using DAL;
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
        private List<KhachHangDTO> goiYKhachHang = new List<KhachHangDTO>();

        public BookingWindow()
        {
            InitializeComponent();
            LoadLoaiPhong();
            LoadTang();
            LoadDanhSachPhongTrong();
            goiYKhachHang = KhachHangBLL.LayTatCaKhachHang();

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

        private void LoadDanhSachPhongTrong()
        {
            var dsPhong = PhongBLL.LayDanhSachPhongTrong()
                .Where(p => p.TrangThai == "Trống")
                .ToList();

            allRooms = dsPhong;

            DataGridPhong.ItemsSource = null;
            DataGridPhong.ItemsSource = allRooms;
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
                int tang = cbTang.SelectedIndex; 
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
        //THÊM PHÒNG VÀO DANH SÁCH ĐẶT PHÒNG
        private void BtnThemPhong_Click(object sender, RoutedEventArgs e)
        {
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
                PhongBLL.UpdateTrangThaiPhong(phong.PhongID, "Đang thuê");
                // Thêm vào danh sách phòng đã chọn
                selectedRooms.Add(new
                {
                    RoomId = phong.PhongID,
                    RoomType = phong.TenLoai,
                    Price = phong.GiaCoBan,
                    Total = thanhTien,
                    LoaiPhongID = phong.LoaiPhongID
                });

                // Cập nhật DataGrid
                dgSelectedRooms.ItemsSource = null;
                dgSelectedRooms.ItemsSource = selectedRooms;

                // Cập nhật tổng tiền
                decimal tongTien = selectedRooms.Sum(r => (decimal)r.Total);
                tbTongTien.Text = $"{tongTien:N0} VNĐ";
              //
              LoadDanhSachPhongTrong(); 
            }
        }

        private void BtnXoaPhong_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext != null)
            {
                dynamic row = btn.DataContext;
                PhongBLL.UpdateTrangThaiPhong(row.RoomId, "Trống");
                selectedRooms.Remove(row);
                dgSelectedRooms.ItemsSource = null;
                dgSelectedRooms.ItemsSource = selectedRooms;

                decimal tongTien = selectedRooms.Sum(r => (decimal)r.Total);
                tbTongTien.Text = $"{tongTien:N0} VNĐ";
                LoadDanhSachPhongTrong();
            }
        }
        //GỢI Ý KHÁCH HÀNG KHI NHẬP SỐ ĐIỆN THOẠI HOẶC EMAIL
        private void txtSearchCustomer_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txtSearchCustomer.Text.Trim();

            txtSearch_Suggest.Text = ""; // Xóa gợi ý cũ
            txtSearchCustomer.Tag = null;

            if (string.IsNullOrWhiteSpace(input))
                return;

            // Tìm khách hàng theo SDT hoặc Email bắt đầu giống input
            var match = goiYKhachHang
                .FirstOrDefault(kh =>
                    (kh.SDT != null && kh.SDT.StartsWith(input, StringComparison.OrdinalIgnoreCase)) ||
                    (kh.Email != null && kh.Email.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                );

            if (match == null) return;

            string full = match.SDT.StartsWith(input) ? match.SDT : match.Email;

            if (full.Length <= input.Length) return;

            // Lấy phần gợi ý
            string suggest = full.Substring(input.Length);

            // Hiển thị chữ mờ
            txtSearch_Suggest.Inlines.Clear();
            txtSearch_Suggest.Inlines.Add(new Run(input)
            {
                Foreground = Brushes.Transparent
            });
            txtSearch_Suggest.Inlines.Add(new Run(suggest)
            {
                Foreground = new SolidColorBrush(Color.FromArgb(140, 0, 0, 0)) // chữ mờ
            });

            // Lưu khách hàng để khi nhấn Tab -> tự fill
            txtSearchCustomer.Tag = match;
        }

        private void txtSearchCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Tab || e.Key == Key.Enter) && txtSearchCustomer.Tag is KhachHangDTO kh)
            {
                // Nếu đang gõ email, fill email
                if (txtSearchCustomer.Text.Contains("@") || txtSearchCustomer.Text.Any(char.IsLetter))
                {
                    txtSearchCustomer.Text = kh.Email;
                }
                // Nếu đang gõ số, fill số điện thoại
                else
                {
                    txtSearchCustomer.Text = kh.SDT;
                }

                txtSearch_Suggest.Text = ""; // clear ghost text

                // Fill thông tin
                txtTenKH.Text = kh.HoTen;
                txtSoDienThoai.Text = kh.SDT;
                txtEmail.Text = kh.Email;
                txtDiaChi.Text = kh.DiaChi;
                txtTenCongTy.Text = kh.CongTy;
                txtMaSoThue.Text = kh.MST;

                chkNewCustomer.IsChecked = false;

                e.Handled = true;
            }
        }
        //INsert ĐẶT PHÒNG KHI NHẤN NÚT ĐẶT PHÒNG
        private void BtnDatPhong_Click(object sender, RoutedEventArgs e)
        {
            // KIỂM TRA SỐ NGƯỜI
            if (string.IsNullOrWhiteSpace(txtNguoiLon.Text) ||
                !int.TryParse(txtNguoiLon.Text, out int nguoiLon) ||
                nguoiLon < 1)
            {
                MessageBox.Show("Số người lớn phải ≥ 1", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTreEm.Text) ||
                !int.TryParse(txtTreEm.Text, out int treEm) ||
                treEm < 0)
            {
                MessageBox.Show("Số trẻ em phải ≥ 0", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var kh = new KhachHangDTO
            {
                HoTen = txtTenKH.Text,
                SDT = txtSoDienThoai.Text,
                Email = txtEmail.Text,
                CongTy = txtTenCongTy.Text,
                MST = txtMaSoThue.Text,
                DiaChi = txtDiaChi.Text
            };

            string khId = BookingDAL.InsertKhachHang(kh);
            foreach (var r in selectedRooms)
            {
                PhongBLL.UpdateTrangThaiPhong((string)r.RoomId, "Đang thuê");
                LoadDanhSachPhongTrong();
            }
            string maDatTong = BookingDAL.InsertDatPhongTong(
                khId,
                kh.HoTen,
                kh.SDT,
                !string.IsNullOrEmpty(kh.MST),
            Convert.ToDecimal(txtTienCoc.Text),
                txtGhiChu.Text,
                "NV01",
                selectedRooms[0].RoomId
            );
            int soNgay = (dpNgayTra.SelectedDate.Value - dpNgayNhan.SelectedDate.Value).Days;
            decimal tienChuaVAT = selectedRooms.Sum(x => (decimal)x.Total);
            decimal tienSauVAT = tienChuaVAT * 1.08m;

            BookingDAL.InsertDatPhongChiTiet(
                maDatTong,
                dpNgayNhan.SelectedDate.Value,
                dpNgayTra.SelectedDate.Value,
                int.Parse(txtNguoiLon.Text),
                int.Parse(txtTreEm.Text),
                selectedRooms.Count,
                8,
                tienSauVAT,
                txtGhiChu.Text
            );
            ResetFormDatPhong();
            MessageBox.Show("Đặt phòng thành công!", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ResetFormDatPhong()
        {
            // Reset chọn phòng
            txtSearchCustomer.Clear();
            txtTenKH.Clear();
            txtSoDienThoai.Clear();
            txtEmail.Clear();
            txtDiaChi.Clear();
            txtTenCongTy.Clear();
            txtMaSoThue.Clear();
            selectedRooms.Clear();
            dgSelectedRooms.ItemsSource = null;

            // Reset số người
            txtNguoiLon.Text = "1";
            txtTreEm.Text = "0";

            // Reset đặt cọc
            txtTienCoc.Text = "0";

            // Reset ghi chú
            txtGhiChu.Text = "";

            // Reset ngày
            dpNgayNhan.SelectedDate = DateTime.Today;
            dpNgayTra.SelectedDate = null;

            // Reset tổng tiền
            tbTongTien.Text = "0 VNĐ";

            // Load lại phòng trống
            LoadDanhSachPhongTrong();
        }

    }
}
