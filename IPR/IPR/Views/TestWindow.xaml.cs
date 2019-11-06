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

        public enum Sex
        {
            Male, Female, Other
        }

        private delegate void OneArgDelagate(object arg);
        private int patientID;
        private int age;
        private int weight;
        private int ergoID;
        private Sex sex;

        public TestWindow(int patientID, int age, int weight, int ergoID, Sex sex)
        {
            InitializeComponent();

            this.patientID = patientID;
            this.age = age;
            this.weight = weight;
            this.ergoID = ergoID;
            this.sex = sex;

            AstrandTest.AstrandTest at = new AstrandTest.AstrandTest();
            OneArgDelagate fetcher = new OneArgDelagate(at.StartTest);
            fetcher.BeginInvoke(this, null, null);
        }

        public void UpdateUI(string data)
        {

        }
    }
}
