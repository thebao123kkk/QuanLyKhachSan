using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using BLL;
using LiveCharts;
using LiveCharts.Wpf;

namespace GUI
{
    public partial class ReportsDashboard : Window, INotifyPropertyChanged
    {
        public ReportsDashboard()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private SeriesCollection _serviceSeries;
        public SeriesCollection ServiceSeries
        {
            get => _serviceSeries;
            set { _serviceSeries = value; OnPropertyChanged(nameof(ServiceSeries)); }
        }

        private string[] _serviceLabels;
        public string[] ServiceLabels
        {
            get => _serviceLabels;
            set { _serviceLabels = value; OnPropertyChanged(nameof(ServiceLabels)); }
        }

        private Func<double, string> _serviceFormatter;
        public Func<double, string> ServiceFormatter
        {
            get => _serviceFormatter;
            set { _serviceFormatter = value; OnPropertyChanged(nameof(ServiceFormatter)); }
        }

        private double _serviceMax;
        public double ServiceMax
        {
            get => _serviceMax;
            set { _serviceMax = value; OnPropertyChanged(nameof(ServiceMax)); }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadServiceChartData();
            TaiDuLieuCongSuatPhong();
            RevenueChart.AxisX.Clear();
            RevenueChart.AxisX.Add(new Axis
            {
                Title = "Ngày",
                Labels = new List<string>(),
                LabelsRotation = 0
            });

            RevenueChart.AxisY.Clear();
            RevenueChart.AxisY.Add(new Axis
            {
                Title = "Doanh thu (VNĐ)",
                LabelFormatter = val => val.ToString("N0") + " đ"
            });

        }

        private void LoadServiceChartData()
        {
            var data = ServiceReportBLL.GetTopService();
            if (data == null || data.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu dịch vụ.", "Thông báo");
                return;
            }

            // Gán nhãn tên dịch vụ
            ServiceLabels = data.Select(d => d.TenDichVu).ToArray();

            // Series: biểu diễn số lần
            ServiceSeries = new SeriesCollection
    {
        new ColumnSeries
        {
            Title = "Số lần",
            Values = new ChartValues<double>(data.Select(d => (double)d.TongSoLan)),
            DataLabels = true,
            LabelPoint = chartPoint =>
            {
                int index = (int)chartPoint.Key;
                var dv = data[index];
                return $"{dv.TongSoLan} lần";
            }
        }
    };

            // Tooltip sẽ tự lấy đúng tên dịch vụ + số lần
            ServiceFormatter = val => val.ToString("N0") + " lần";
            
            // Tính scale trục Y
            var maxCount = data.Max(d => (double)d.TongSoLan);
            var step = 10;
            ServiceMax = Math.Ceiling(maxCount / step) * step;
        }

        public SeriesCollection CongSuatSeries { get; set; }

        private void TaiDuLieuCongSuatPhong()
        {
            var data = ServiceReportBLL.GetCongSuatLoaiPhong();
            CongSuatSeries = new SeriesCollection();

            Func<ChartPoint, string> labelFunc = cp =>
                $"{cp.Participation:P1}";

            foreach (var item in data)
            {
                var series = new PieSeries
                {
                    Title = item.TenLoaiPhong,
                    Values = new ChartValues<double> { item.SoPhongDaDat },
                    DataLabels = true,
                    LabelPoint = labelFunc
                };
                CongSuatSeries.Add(series);
            }

            OccupancyChart.Series = CongSuatSeries;
        }

        public SeriesCollection RevenueSeries { get; set; }
        public List<string> RevenueLabels { get; set; }
        public Func<double, string> RevenueFormatter { get; set; }

        private void LoadDoanhThuChart()
        {
            DateTime? start = StartDatePicker.SelectedDate;
            DateTime? end = EndDatePicker.SelectedDate;

            var data = ServiceReportBLL.LayDoanhThuTheoNgay(start, end);
            //MessageBox.Show($"Tổng ngày: {data.Count}");
            RevenueLabels = data.Select(d => d.Ngay.ToString("dd/MM")).ToList();

            RevenueSeries = new SeriesCollection
    {
        new ColumnSeries
        {
            Title = "Doanh thu",
            Values = new ChartValues<double>(data.Select(d => (double)d.TongDoanhThu)),
            DataLabels = true,
            LabelPoint = point => $"{point.Y:N0} đ"
        }
    };

            RevenueFormatter = val => val.ToString("N0") + " đ";
            DataContext = this;
            RevenueChart.Series = RevenueSeries;
            RevenueChart.AxisX[0].LabelsRotation = 0;
            RevenueChart.AxisX[0].Labels = RevenueLabels;
            RevenueChart.AxisY[0].LabelFormatter = RevenueFormatter;

        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            LoadDoanhThuChart();
            //MessageBox.Show("OK");
        }

        private void ExportExcelButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? start = StartDatePicker.SelectedDate;
            DateTime? end = EndDatePicker.SelectedDate;
            var data = ServiceReportBLL.LayDoanhThuTheoNgay(start, end);
            BaoCaoDoanhThuBLL.XuatExcelDoanhThu(data);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainDashboard dashboard = new MainDashboard();
            dashboard.Show();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
