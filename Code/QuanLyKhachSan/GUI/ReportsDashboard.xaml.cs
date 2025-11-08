using LiveCharts;
using LiveCharts.Wpf;
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
    /// Interaction logic for ReportsDashboard.xaml
    /// </summary>
    public partial class ReportsDashboard : Window
    {
        public ReportsDashboard()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        // Nút đóng cửa sổ
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainDashboard dashboard = new MainDashboard();
            dashboard.Show();
        }


        // --- Logic cho Tab 1: Báo Cáo Doanh Thu ---

        private void LoadRevenueChartData()
        {
           
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            // Logic: Lọc dữ liệu CSDL dựa trên StartDatePicker.SelectedDate và EndDatePicker.SelectedDate
            // Cập nhật lại RevenueSeries và RevenueLabels
            MessageBox.Show("Đã lọc báo cáo!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportExcelButton_Click(object sender, RoutedEventArgs e)
        {
            // Logic: Xuất dữ liệu ra Excel (SRS 4.5)
            MessageBox.Show("Đang xuất file Excel...", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // --- Logic cho Tab 2: Báo Cáo Công Suất ---

        private void LoadOccupancyChartData()
        {
        }

        private void PieChart_DataClick(object sender, ChartPoint chartPoint)
        {
            MessageBox.Show($"Bạn đã chọn: {chartPoint.SeriesView.Title} - {chartPoint.Y} phòng.", "Thông Tin", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // --- Logic cho Tab 3: Báo Cáo Dịch Vụ ---

        private void LoadServiceChartData()
        {
        }

        // --- Logic cho Tab 4: Hiệu Suất Nhân Viên ---

        private void LoadStaffGridData()
        {

        }
    }
}
