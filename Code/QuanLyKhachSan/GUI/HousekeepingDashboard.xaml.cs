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
    /// Interaction logic for HousekeepingDashboard.xaml
    /// </summary>
    public partial class HousekeepingDashboard : Window
    {
        public HousekeepingDashboard()
        {
            InitializeComponent();
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
        }

        // --- Tải Dữ Liệu và Lọc ---

        private void LoadRoomsGrid()
        {
            // Lọc danh sách dựa trên ComboBox
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (allRooms != null) // Đảm bảo dữ liệu đã được khởi tạo
            //{
            //    LoadRoomsGrid();
            //}
        }

        private void RoomsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        // --- Xử lý các nghiệp vụ Buồng phòng ---

        private void MarkCleanButton_Click(object sender, RoutedEventArgs e)
        {
            // (SRS 4.3: Luồng Bẩn -> Sạch)
        }

        private void ConfirmCheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            // (SRS 4.2.1: Ràng buộc check-out)
        }

        private void ReportMaintenanceButton_Click(object sender, RoutedEventArgs e)
        {
            // (SRS 4.3: Trạng thái Đang sửa chữa/bảo trì)

                LoadRoomsGrid();
        }
    }
}
