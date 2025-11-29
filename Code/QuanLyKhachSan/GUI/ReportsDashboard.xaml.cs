using BLL;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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
            set
            {
                _serviceSeries = value;
                OnPropertyChanged(nameof(ServiceSeries));
            }
        }

        private string[] _serviceLabels;
        public string[] ServiceLabels
        {
            get => _serviceLabels;
            set
            {
                _serviceLabels = value;
                OnPropertyChanged(nameof(ServiceLabels));
            }
        }

        private Func<double, string> _serviceFormatter;
        public Func<double, string> ServiceFormatter
        {
            get => _serviceFormatter;
            set
            {
                _serviceFormatter = value;
                OnPropertyChanged(nameof(ServiceFormatter));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadServiceChartData();
        }

        private void LoadServiceChartData()
        {
            var data = ServiceReportBLL.GetTopService();

            if (data.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu dịch vụ.", "Thông báo");
                return;
            }

            // Debug
            string debugText = string.Join("\n", data.Select(d => $"{d.TenDichVu}: {d.TongSoLan} lần, {d.DoanhThu:N0} đ"));
            MessageBox.Show(debugText);

            ServiceLabels = data.Select(d => d.TenDichVu).ToArray();

            ServiceSeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Số lần",
                    Values = new ChartValues<double>(data.Select(d => (double)d.TongSoLan)),
                    DataLabels = true,
                    LabelPoint = val => val.X.ToString("N0") + " lần"
                },
                new ColumnSeries
                {
                    Title = "Doanh thu",
                    Values = new ChartValues<double>(data.Select(d => (double)d.DoanhThu)),
                    DataLabels = true,
                    LabelPoint = val => val.X.ToString("N0") + " đ"
                }
            };

            ServiceFormatter = val => val.ToString("N0");
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
            {
                DragMove();
            }
        }
    }
}
