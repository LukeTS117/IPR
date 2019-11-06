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
    /// Interaction logic for Connect.xaml
    /// </summary>
    public partial class Connect : Page
    {

        private BLEConnect bleConnection;
       

        public Connect()
        {
            InitializeComponent();
        }

        public bool ConnectBLE(string ergoID)
        {
            bleConnection = new BLEConnect(ergoID);
            bleConnection.Connect();

            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (ConnectBLE(textBox1.Text))
            //{

            this.NavigationService.Navigate(new TestWindow());

            



            // }
        }

        public void UpdateUI(string data)
        {
            Console.WriteLine(data);
        }
    }
}
