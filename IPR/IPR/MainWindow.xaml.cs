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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IPR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BLEConnect bleConnection;
        public MainWindow()
        {
            InitializeComponent();
        }

        public void BLEConnect(string ergoID)
        {
            bleConnection = new BLEConnect(ergoID);
            bleConnection.Connect();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.BLEConnect(textBox1.Text);
        }
    }
}
