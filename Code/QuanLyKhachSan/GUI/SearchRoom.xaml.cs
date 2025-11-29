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
    /// Interaction logic for SearchRoom.xaml
    /// </summary>
    public partial class SearchRoom : Window
    {
        public SearchRoom()
        {
            InitializeComponent();
        }
        // Kéo thả cửa sổ (vì WindowStyle=None)
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
            LoadData();
        }

        private void LoadData()
        {
            string keyword = SearchTermTextBox.Text.Trim();
            var ds = DatPhongBLL.LayPhongDaDatTheoKhach(keyword);
            ResultsDataGrid.ItemsSource = ds;
        }


        // Đóng cửa sổ
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainDashboard dashboard = new MainDashboard();
            dashboard.Show();
        }

        // Nút Tìm kiếm
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string keyword = SearchTermTextBox.Text.Trim();
            string criteria = (SearchCriteriaComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            var ds = DatPhongBLL.SearchBooking(criteria, keyword);

            ResultsDataGrid.ItemsSource = ds;
        }


        // Chech in
        private void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResultsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Hãy chọn phòng để Checkout.");
                return;
            }

            DatPhongViewDTO selected = (DatPhongViewDTO)ResultsDataGrid.SelectedItem;

            if (DatPhongBLL.CheckoutPhong(selected.PhongID))
            {
                LoadData();
                MessageBox.Show("Checkin thành công!.");

                // Reload lại
                string keyword = SearchTermTextBox.Text.Trim();
                ResultsDataGrid.ItemsSource = DatPhongBLL.LayPhongDaDatTheoKhach(keyword);
            }
            else
            {
                MessageBox.Show("Checkin thất bại.");
            }
        }

        // Khi chọn một hàng trong DataGrid
        private void ResultsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ResultsDataGrid.SelectedItem != null)
            {
                ViewDetailsButton.IsEnabled = true;
            }
            else
            {
                ViewDetailsButton.IsEnabled = false;
            }
        }

        // Xem chi tiết phòng
        private void ViewDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResultsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Bạn chưa chọn phòng.");
                return;
            }
            var selected = (DatPhongViewDTO)ResultsDataGrid.SelectedItem;

            RoomDetails detail = new RoomDetails(selected);
            detail.Show();
            this.Close();
        }

    }
}
