
using IPR.BLEHandling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace IPR.AstrandTest
{
    class AstrandTest : IAstrandDataListener
    {
        private AstrandTestPhase current_phase = AstrandTestPhase.INACTIVE;
        private static int ROTATIONTARGET_MIN = 50;
        private static int ROTATIONTARGET_MAX = 60;
        private readonly IAstrandData data;

        private static int WARMING_UP_TIME = 5; //Time in seconds 
        private static int MAIN_TEST_TIME = 20;
        private static int COOLING_DOWN_TIME = 5;
        //private static int EXTENDED_TEST_TIME = 5;

        private TestWindow testWindow;

        private Stopwatch elapsedStopWatch = null;
        private List<DataPoint> dataPoints;
        private List<int> instantCadence;
        private Timer myTimer;
        private AstrandTestPhase nextPhase;
        bool testStarted = false;

        private struct DataPoint
        {
            public DataTypes dataType { get; set; }
            public int value { get; set; }
            public int elapsedTime { get; set; }

            public DataPoint(DataTypes dataType, int value, int elapsedTime)
            {
                this.dataType = dataType;
                this.value = value;
                this.elapsedTime = elapsedTime;
            }
        }

        
        public AstrandTest(object TestWindow, IAstrandData astrandData)
        {
            this.testWindow = TestWindow as TestWindow;
            this.data = astrandData;
            this.dataPoints = new List<DataPoint>();
            this.instantCadence = new List<int>();
            this.data.Connect(this);


        }

        public void StartTest()
        {
            this.testStarted = true;
            ChangePhase(AstrandTestPhase.WARMING_UP);
        }

        private void ChangePhase(AstrandTestPhase phase)
        {
            if (phase == AstrandTestPhase.WARMING_UP){ this.WarmingUp(); }
            else if (phase == AstrandTestPhase.MAIN_TEST) { this.MainTest(); }
            else if (phase == AstrandTestPhase.COOLING_DOWN) { this.CoolingDown(); }

            else if (phase == AstrandTestPhase.EXTENDED_TEST) { this.ExtendedTest(); }
            else if (phase == AstrandTestPhase.INACTIVE) { this.InActive(); }

            current_phase = phase;
        }

        public void SetRotation(int rotation)
        {
            if(rotation < ROTATIONTARGET_MIN)
            {
                Console.WriteLine("GO FASTER!");
            }

            else if(rotation > ROTATIONTARGET_MAX)
            {
                Console.WriteLine("SLOW DOWN!");
            }
        }




        /*  
         *  Phases
        */

        private void WarmingUp()
        {
            SetTimer(WARMING_UP_TIME, AstrandTestPhase.MAIN_TEST);
        }

        private void MainTest()
        {
            instantCadence = new List<int>();

            SetTimer(MAIN_TEST_TIME, AstrandTestPhase.COOLING_DOWN);


            Console.WriteLine("Instant Cadence Results: ");
            foreach(int ic in instantCadence)
            {
                Console.WriteLine(ic);
            }
            Console.WriteLine("---------------");


            Console.WriteLine("Main Test completed, moving on to Cooling Down");
            
        }

        private void CoolingDown()
        {
            SetTimer(COOLING_DOWN_TIME, AstrandTestPhase.INACTIVE);
           
            Console.WriteLine("Cooling Down completed, deactivating test");
          
        }

        private void ExtendedTest()
        {
            
        }

        private void InActive()
        {
            
        }

        // // // // // // // //

       

        private void SetTimer(int seconds, AstrandTestPhase nextPhase)
        {
            this.nextPhase = nextPhase;
            myTimer = new Timer(seconds * 1000);
            myTimer.Elapsed += OnTimedEvent;
            myTimer.Enabled = true;
            myTimer.Start();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            ChangePhase(nextPhase);
        }

        public void OnDataAvailable(DataTypes dataType, int value)
        {
            if (!testStarted)
            {
                StartTest();
            }


            dataPoints.Add(new DataPoint(dataType, value, this.GetElapsedTime()));
            Console.WriteLine(dataType.ToString() + " " + value);


            if(dataType == DataTypes.IC)
            {
                instantCadence.Add(value);
                SetRotation(value);
            }

        }

        public int GetElapsedTime()
        {
            int elapsedTime;

            if(elapsedStopWatch != null)
            {
                elapsedTime = (int)elapsedStopWatch.ElapsedMilliseconds;
                elapsedStopWatch.Restart();
                return elapsedTime;
            }

            return 0;
        }
    }
}
