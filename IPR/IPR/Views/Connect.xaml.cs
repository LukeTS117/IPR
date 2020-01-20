using IPR.AstrandTest;
using IPR.Simulation;
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
        private IAstrandData dataHandler;
        private ISim sim = null;

        public Connect()
        {
            InitializeComponent();
        }

        public bool ConnectBLE(string ergoID)
        {
            if(ergoID == "0")
            {
                
                dataHandler = CreateSim();
                
                return true;
            }
            bleConnection = new BLEConnect(ergoID);
            bleConnection.Connect();
            dataHandler = bleConnection.bleHandler;

            return true;
        }

        private IAstrandData CreateSim() 
        {
            SimData simData = new SimData();
            this.sim = simData;
            return simData; 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ConfirmSelection();
        }

        public void OnEnterPressed(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                ConfirmSelection();
            }
        }

        public void ConfirmSelection()
        {
            bool inputCorrect = true;

            TestWindow.Sex sex = TestWindow.Sex.Other;

            //Reset all labels to White
            Label_PatientID.Foreground = Brushes.White;
            Label_Age.Foreground = Brushes.White;
            Label_Weight.Foreground = Brushes.White;
            Label_ErgoID.Foreground = Brushes.White;
            Label_Sex.Foreground = Brushes.White;


            //Check all fields for correct input, else change label color to red // //  
            if (!int.TryParse(TextBox_PatientID.Text, out int patientID))
            {
                inputCorrect = false;
                Label_PatientID.Foreground = Brushes.Red;
            }

            if (!int.TryParse(TextBox_Age.Text, out int age))
            {
                inputCorrect = false;
                Label_Age.Foreground = Brushes.Red;
            }

            if (!int.TryParse(TextBox_Weight.Text, out int weight))
            {
                inputCorrect = false;
                Label_Weight.Foreground = Brushes.Red;
            }
            if (!int.TryParse(TextBox_ErgoID.Text, out int ergoID))
            {
                inputCorrect = false;
                Label_ErgoID.Foreground = Brushes.Red;
            }
            else
            {
                ConnectBLE(TextBox_ErgoID.Text);
            }

            if (ComboBox_Sex.SelectedIndex != -1)
            {
                if (ComboBox_Sex.SelectedIndex == 0) { sex = TestWindow.Sex.Male; }
                if (ComboBox_Sex.SelectedIndex == 1) { sex = TestWindow.Sex.Female; }


            }
            else
            {
                inputCorrect = false;
                Label_Sex.Foreground = Brushes.Red;
            }

            // // // // 


            if (inputCorrect)
            {
                this.NavigationService.Navigate(new TestWindow(patientID, age, weight, ergoID, sex, dataHandler, sim));
            }
        }

        public void UpdateUI(string data)
        {
            Console.WriteLine(data);
        }

    }
}
