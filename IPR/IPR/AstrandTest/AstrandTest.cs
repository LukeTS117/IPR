
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR.AstrandTest
{
    class AstrandTest : IAstrandDataListener
    {
        private AstrandTestPhase current_phase = AstrandTestPhase.INACTIVE;
        private static int ROTATIONTARGET_MIN = 50;
        private static int ROTATIONTARGET_MAX = 60;
        private readonly IAstrandData data;

        private static int WARMING_UP_TIME = 5; //Time in seconds 
        private static int MAIN_TEST_TIME = 5;
        private static int COOLING_DOWN_TIME = 5;
        //private static int EXTENDED_TEST_TIME = 5;

        private TestWindow testWindow;

        private Stopwatch elapsedStopWatch = null;
        private List<DataPoint> dataPoints;

        private struct DataPoint
        {
            public ValueType valueType { get; set; }
            public int value { get; set; }
            public int elapsedTime { get; set; }

            public DataPoint(ValueType valueType, int value, int elapsedTime)
            {
                this.valueType = valueType;
                this.value = value;
                this.elapsedTime = elapsedTime;
            }
        }

        
        public AstrandTest(object TestWindow, IAstrandData astrandData)
        {
            this.testWindow = TestWindow as TestWindow;
            this.data = astrandData;
            this.dataPoints = new List<DataPoint>();
        }

        public void StartTest()
        {
            ChangePhase(AstrandTestPhase.WARMING_UP);
        }

        private void ChangePhase(AstrandTestPhase phase)
        {
            if (phase == AstrandTestPhase.WARMING_UP){ this.WarmingUp(); }
            else if (phase == AstrandTestPhase.MAIN_TEST) { this.MainTest(); }
            else if (phase == AstrandTestPhase.COOLING_DOWN) { this.CoolingDown(); }

            else if (phase == AstrandTestPhase.EXTENDED_TEST) { this.ExtendedTest(); }
            else if (phase == AstrandTestPhase.EXTENDED_TEST) { this.InActive(); }

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
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed.TotalSeconds < WARMING_UP_TIME)
            {
                //dostuff
                //testWindow.UpdateUI("I cant believe that this works");
            }
            sw.Stop();
            Console.WriteLine("StopWatch stopped at: " + sw.Elapsed.TotalSeconds);
            Console.WriteLine("Warming Up completed, moving on to Main Test");
            ChangePhase(AstrandTestPhase.MAIN_TEST);

        }

        private void MainTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed.TotalSeconds < MAIN_TEST_TIME)
            {
                //dostuff
            }
            sw.Stop();
            Console.WriteLine("StopWatch stopped at: " + sw.Elapsed.TotalSeconds);
            Console.WriteLine("Main Test completed, moving on to Cooling Down");
            ChangePhase(AstrandTestPhase.COOLING_DOWN);
        }

        private void CoolingDown()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed.TotalSeconds < COOLING_DOWN_TIME)
            {
                //dostuff
            }
            sw.Stop();
            Console.WriteLine("StopWatch stopped at: " + sw.Elapsed.TotalSeconds);
            Console.WriteLine("Cooling Down completed, deactivating test");
            ChangePhase(AstrandTestPhase.INACTIVE);
        }

        private void ExtendedTest()
        {
            
        }

        private void InActive()
        {
            
        }

        // // // // // // // //

        private void SetStopWatch(int minutes)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed.TotalMinutes < minutes)
            {
                Console.WriteLine("Stopwatch at:" + sw.Elapsed.TotalSeconds);
            }
            sw.Stop();
            
        }

        public void OnDataAvailable(ValueType valueType, int value)
        {
            dataPoints.Add(new DataPoint(valueType, value, this.GetElapsedTime()));

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
