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
        }

        // Đóng cửa sổ
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Nút Tìm kiếm
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string keyword = SearchTermTextBox.Text.Trim();
            string criteria = (SearchCriteriaComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // TODO: gọi hàm tìm kiếm phòng từ database
            MessageBox.Show($"Tìm kiếm theo: {criteria}\nTừ khóa: {keyword}", "Thông báo");
        }

        // Khi chọn một hàng trong DataGrid
        private void ResultsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
           
        }

        // Xem chi tiết phòng
        private void ViewDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResultsDataGrid.SelectedItem != null)
            {
                MessageBox.Show("Xem chi tiết phòng đã chọn.", "Thông tin");
            }
        }

        // Đặt phòng
        private void BookRoomButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResultsDataGrid.SelectedItem != null)
            {
                MessageBox.Show("Mở form đặt phòng cho phòng đã chọn.", "Đặt phòng");
            }
        } 
    }
}
