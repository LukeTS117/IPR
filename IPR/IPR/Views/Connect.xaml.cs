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

            bool inputCorrect = true;

            int patientID;
            int age;
            int weight;
            int ergoID;
            TestWindow.Sex sex = TestWindow.Sex.Other;

            //Reset all labels to White
            Label_PatientID.Foreground = Brushes.White;
            Label_Age.Foreground = Brushes.White;
            Label_Weight.Foreground = Brushes.White;
            Label_ErgoID.Foreground = Brushes.White;
            Label_Sex.Foreground = Brushes.White;


            //Check all fields for correct input, else change label color to red // //  
            if (!int.TryParse(TextBox_PatientID.Text, out patientID))
            {
                inputCorrect = false;
                Label_PatientID.Foreground = Brushes.Red;
            }

            if (!int.TryParse(TextBox_Age.Text, out age))
            {
                inputCorrect = false;
                Label_Age.Foreground = Brushes.Red;
            }

            if (!int.TryParse(TextBox_Weight.Text, out weight))
            {
                inputCorrect = false;
                Label_Weight.Foreground = Brushes.Red;
            }
            if (!int.TryParse(TextBox_ErgoID.Text, out ergoID))
            {
                inputCorrect = false;
                Label_ErgoID.Foreground = Brushes.Red;

                //ConnectBLE(TextBox_ErgoID.Text);
            }

            if (ComboBox_Sex.SelectedIndex != -1)
            {
                if (ComboBox_Sex.SelectedIndex == 0) { sex = TestWindow.Sex.Male; }
                if (ComboBox_Sex.SelectedIndex == 1) { sex = TestWindow.Sex.Female; }
                if (ComboBox_Sex.SelectedIndex == 2) { sex = TestWindow.Sex.Other; }

            }
            else
            {
                inputCorrect = false;
                Label_Sex.Foreground = Brushes.Red;
            }

            // // // // 


            if (inputCorrect)
            {
                this.NavigationService.Navigate(new TestWindow(patientID, age, weight, ergoID, sex));
            }
            

            



            // }
        }

        public void UpdateUI(string data)
        {
            Console.WriteLine(data);
        }

        private bool CheckInput()
        {

           


            return true;
        }
    }
}
