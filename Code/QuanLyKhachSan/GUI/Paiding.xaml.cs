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
    /// Interaction logic for Paiding.xaml
    /// </summary>
    public partial class Paiding : Window
    {
        public Paiding()
        {
            InitializeComponent();
        }

        private void FinalizeCheckoutButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrintVatButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrintProformaButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RecordPaymentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void ApplyDiscountButton_Click(object sender, RoutedEventArgs e)
        {

        }

        // Nút đóng cửa sổ
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
