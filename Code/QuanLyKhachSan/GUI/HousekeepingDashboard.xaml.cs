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
using BLL;
using DTO;

namespace GUI
{
    /// <summary>
    /// Interaction logic for HousekeepingDashboard.xaml
    /// </summary>
    public partial class HousekeepingDashboard : Window
    {
        public HousekeepingDashboard()
        {
            InitializeComponent();
            LoadRoomsGrid();
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

        // --- Tải Dữ Liệu và Lọc ---
        private List<PhongDTO> allRooms = new List<PhongDTO>();

        private void LoadRoomsGrid()
        {
            // Lấy dữ liệu gốc từ DB
            allRooms = PhongBLL.LayTatCaPhong();

            string filter = (LocTrangThaicb.SelectedItem as ComboBoxItem)?.Content.ToString();
            IEnumerable<PhongDTO> filtered = allRooms;

            switch (filter)
            {
                case "Chỉ xem phòng Bẩn":
                    filtered = allRooms.Where(p => p.TrangThai == "Bẩn");
                    break;

                case "Chỉ xem phòng Đang ở":
                    // Đang ở = Đã nhận trong DB
                    filtered = allRooms.Where(p => p.TrangThai == "Đã nhận");
                    break;

                case "Chỉ xem phòng Sạch":
                    // Sạch = Trống + Chờ nhận phòng
                    filtered = allRooms.Where(p =>
                        p.TrangThai == "Trống" || p.TrangThai == "Chờ nhận phòng");
                    break;
            }

            // Map trạng thái hiển thị: Đã nhận -> Đang ở
            var displayList = filtered
                .Select(p => new PhongDTO
                {
                    PhongID = p.PhongID,
                    SoPhong = p.SoPhong,
                    LoaiPhongID = p.LoaiPhongID,
                    TenLoai = p.TenLoai,
                    SucChua = p.SucChua,
                    GiaCoBan = p.GiaCoBan,
                    GhiChu = p.GhiChu,
                    TrangThai = (p.TrangThai == "Đã nhận") ? "Đang ở" : p.TrangThai
                })
                .ToList();

            PhongDataGrid.ItemsSource = displayList;
        }


        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
                LoadRoomsGrid();
        }

        private PhongDTO selectedRoom = null;
        private void PhongDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedRoom = PhongDataGrid.SelectedItem as PhongDTO;

            if (selectedRoom == null)
            {
                ActionsPanel.IsEnabled = false;
                SelectedRoomTextBlock.Text = "Vui lòng chọn một phòng";
                TrangThaiPhongDuocChon.Text = "Trạng thái: ";
                CapNhatTrangThaibtn.IsEnabled = false;
                return;
            }

            ActionsPanel.IsEnabled = true;

            // Hiển thị mã phòng
            SelectedRoomTextBlock.Text = $"Phòng {selectedRoom.PhongID}";

            // Hiển thị trạng thái
            string trangThai = selectedRoom.TrangThai == "Đã nhận" ? "Đang ở" : selectedRoom.TrangThai;
            TrangThaiPhongDuocChon.Text = $"Trạng thái: {trangThai}";

            // Cập nhật mode của nút dựa trên trạng thái
            if (trangThai == "Bẩn")
            {
                CapNhatTrangThaibtn.Content = "Hoàn thành Dọn (Sạch)";
                CapNhatTrangThaibtn.IsEnabled = true;
            }
            else if (trangThai == "Đang ở")
            {
                CapNhatTrangThaibtn.Content = "Hoàn thành kiểm phòng";
                CapNhatTrangThaibtn.IsEnabled = true;
            }
            else
            {
                CapNhatTrangThaibtn.IsEnabled = false;
            }
        }

        // --- Xử lý các nghiệp vụ Buồng phòng 
        private void CapNhatTrangThaibtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRoom == null) return;

            string trangThaiHienThi = selectedRoom.TrangThai == "Đã nhận" ? "Đang ở" : selectedRoom.TrangThai;

            // ------------------------
            // TRƯỜNG HỢP 1: PHÒNG BẨN
            // ------------------------
            if (trangThaiHienThi == "Bẩn")
            {
                var result = MessageBox.Show(
                    "Phòng đã được dọn sẵn sàng cho khách vào?",
                    "Xác nhận",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    PhongBLL.UpdateTrangThaiPhong(selectedRoom.PhongID, "Trống");

                    MessageBox.Show("Phòng đã được cập nhật sang trạng thái Sạch.", "Thành công");
                    LoadRoomsGrid();
                }

                return;
            }

            // ------------------------
            // TRƯỜNG HỢP 2: ĐANG Ở (ĐÃ NHẬN)
            // ------------------------
            if (trangThaiHienThi == "Đang ở")
            {
                var result = MessageBox.Show(
                    "Hãy cập nhật ghi chú (nếu có) trước khi xác nhận kiểm phòng.",
                    "Xác nhận",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Information
                );

                if (result == MessageBoxResult.OK)
                {
                    // Phòng đang ở → kiểm phòng xong → chuyển sang Bẩn
                    PhongBLL.UpdateTrangThaiPhong(selectedRoom.PhongID, "Bẩn");

                    MessageBox.Show("Trạng thái phòng đã được chuyển sang Bẩn.", "Thành công");
                    LoadRoomsGrid();
                }

                return;
            }
        }

        private void ReportMaintenanceButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRoom == null)
            {
                MessageBox.Show("Hãy chọn một phòng trước khi gửi báo cáo.");
                return;
            }

            string noiDung = BaoCaoBaoTritb.Text.Trim();

            if (string.IsNullOrEmpty(noiDung))
            {
                MessageBox.Show("Vui lòng nhập nội dung báo cáo.");
                return;
            }

            PhongBLL.UpdateGhiChuPhong(selectedRoom.PhongID, noiDung);

            MessageBox.Show("Báo cáo bảo trì đã được lưu vào ghi chú phòng.", "Thành công",
                MessageBoxButton.OK, MessageBoxImage.Information);

            BaoCaoBaoTritb.Clear();
            LoadRoomsGrid();
        }
    }
}
