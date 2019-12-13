using IPR.AstrandTest;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IPR
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Page
    {

        private delegate void OneArgDelagate();
        private RealTimeData realTimeData;

        public SeriesCollection seriesView { get; set; }


        public enum Sex
        {
            Male, Female, Other
        }

        
        private int patientID;
        private int age;
        private int weight;
        private int ergoID;
        private Sex sex;


        AstrandTest.AstrandTest at = null;



        public TestWindow(int patientID, int age, int weight, int ergoID, Sex sex)
        {
            InitializeComponent();


            seriesView = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                }
            };

            DataContext = this;

            this.patientID = patientID;
            this.age = age;
            this.weight = weight;
            this.ergoID = ergoID;
            this.sex = sex;


            this.at = new AstrandTest.AstrandTest(this, realTimeData);
            
        }

        private void Button_StartTest_Click(object Sender, RoutedEventArgs e)
        {
            OneArgDelagate fetcher = new OneArgDelagate(at.StartTest);
            fetcher.BeginInvoke(null, null);
        }

        public void UpdateUI(string data)
        {

        }
    }
}
