using IPR.AstrandTest;
using IPR.BLEHandling;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace IPR
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Page
    {

        private delegate void OneArgDelagate();
        public ChartValues<ObservableValue> observableValues { get; set; }


        private int seconds = 0;
        private int minutes = 0;

        private Timer _timer;



        BLEHandler bLEHandler;

        public enum Sex
        {
            Male, Female, Other
        }


        public int patientID { get; set; }
        private int age { get; set; }
        private int weight { get; set; }
        private int ergoID { get; set; }
        private Sex sex { get; set; }


        AstrandTest.AstrandTest at = null;




        public TestWindow(int patientID, int age, int weight, int ergoID, Sex sex, BLEHandler bLEHandler)
        {
            InitializeComponent();
            this.bLEHandler = bLEHandler;

            CartesianChart ch = new CartesianChart();
            observableValues = new ChartValues<ObservableValue>();

            ch.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "HeartRate",
                    Values = observableValues
                }
            };


           _timer = new Timer(_ => OnTick(), null, 0, 1000 * 1);

            chartGrid.Children.Add(ch);

            DataContext = this;

            this.patientID = patientID;
            this.text_PatientID.Text = patientID.ToString();
            this.age = age;
            this.text_Age.Text = age.ToString();
            this.weight = weight;
            this.text_Weight.Text = weight.ToString();
            this.ergoID = ergoID;
            this.sex = sex;


            if (this.sex == Sex.Male)
            {
                this.text_Sex.Text = "Male";
            }

            if (this.sex == Sex.Female)
            {
                this.text_Sex.Text = "Female";
            }


            bool isMale = false;
            if (this.sex == Sex.Male)
            {
                isMale = true;
            }

            this.at = new AstrandTest.AstrandTest(this, this.bLEHandler, age, weight, isMale);

        }

        private void Button_StartTest_Click(object Sender, RoutedEventArgs e)
        {
            OneArgDelagate fetcher = new OneArgDelagate(at.StartTest);
            fetcher.BeginInvoke(null, null);
        }

        public void UpdateUI(int data)
        {
            observableValues.Add(new ObservableValue(data));
        }

        public void SetUIRotation(String instruction, int rotation)
        {
            if(instruction != null)
            {
                SetText(text_Instruction, instruction);
            }
            
            SetText(text_Cadence, rotation.ToString());
        }

        public void SetTimer(int s)
        {
            _timer.Dispose();
            _timer = new Timer(_ => OnTick(), null, 0, 1000 * 1);
            minutes = (int)Math.Floor((double)s / 60);
            seconds = s - (minutes * 60);
        }

        private void OnTick()
        {
            
            if (seconds == 0 && minutes > 0)
            {
                seconds = 59;
                minutes -= 1;
            }
            else if (seconds > 0)
            {
                seconds -= 1;
            }
            else if (seconds == 0 && minutes == 0)
            {
                //dt.Stop();
            }

            SetText(text_TimeLeft, string.Format("{0:00}:{1:00}",minutes, seconds));
        }


        public void SetText(TextBlock tb, string text)
        {
            tb.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate () { tb.Text = text; }));
        }
    }
}
