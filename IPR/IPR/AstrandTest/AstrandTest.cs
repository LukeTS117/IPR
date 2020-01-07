
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
        private static int HR_MAX_DIFFERENCE = 5;
        private static int HR_MIN = 130;
        private static int STEADYSTATE_INTERVAL = 15;
        private static int STEADYSTATE_TIME = 120;
        private readonly IAstrandData data;

        private static double WORKLOAD_CONSTANT = 6.11829727786744;

        private static int WARMING_UP_TIME = 5; //Time in seconds 
        private static int MAIN_TEST_TIME = 20;
        private static int COOLING_DOWN_TIME = 5;
        //private static int EXTENDED_TEST_TIME = 5;

        private TestWindow testWindow;

        private Stopwatch elapsedStopWatch = null;
        private List<DataPoint> dataPoints;
        private List<int> instantCadence;
        private List<int> heartFrequency;
        
        
        private Timer myTimer;
        private Timer steadyStateTimer;
        private Timer intervalTimer;
        private AstrandTestPhase nextPhase;
        bool testStarted = false;
        bool steadyStateTestSuccesfull = false;

        private List<int> steadyHeartFrequency;
        private List<int> steadyIC;

        private int age;
        private int weight;
        private bool male;
        private int maxheartbeat;


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

        
        
        public AstrandTest(object TestWindow, IAstrandData astrandData, int age, int weight, bool male)
        {
            this.testWindow = TestWindow as TestWindow;
            this.data = astrandData;
            this.dataPoints = new List<DataPoint>();
            this.instantCadence = new List<int>();
            this.heartFrequency = new List<int>();
            this.data.Connect(this);
            this.heartFrequency = new List<int>();

            this.age = age;
            this.weight = weight;
            this.male = male;
            this.maxheartbeat = GetMaxHeartBeat(age);
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

       



        // // // // // // // // // // // // // // // // // // // // // // // // 
        //  Phases
        // // // // // // // // // // // // // // // // // // // // // // // //

        private void WarmingUp()
        {
            SetTimer(WARMING_UP_TIME, AstrandTestPhase.MAIN_TEST);
        }

        private void MainTest()
        {
            this.instantCadence = new List<int>();
            this.steadyHeartFrequency = new List<int>();
            this.steadyIC = new List<int>();


            StartSteadyStateTimer(STEADYSTATE_TIME, STEADYSTATE_INTERVAL);

            SetTimer(MAIN_TEST_TIME, AstrandTestPhase.COOLING_DOWN);

        }

        private void CoolingDown()
        {
            steadyStateTimer.Stop();

            Console.WriteLine("Instant Cadence Results: ");
            foreach (int ic in instantCadence)
            {
                Console.WriteLine(ic);
            }
            Console.WriteLine("---------------");


            Console.WriteLine("Main Test completed, moving on to Cooling Down");


            SetTimer(COOLING_DOWN_TIME, AstrandTestPhase.INACTIVE);
           
        }

        private void ExtendedTest()
        {
            
        }

        private void InActive()
        {

            Console.WriteLine("Cooling Down completed, deactivating test");

        }


        // // // // // // // // // // // // // // // // // // // // // // // //
        // Steady State Timer
        // // // // // // // // // // // // // // // // // // // // // // // //

        private void StartSteadyStateTimer(int seconds, int interval)
        {
            STEADYSTATE_INTERVAL = interval;
            steadyStateTimer = new Timer(seconds * 1000);
            steadyStateTimer.Elapsed += OnTimedEvent_SteadyStateTestFinished;
            steadyStateTimer.Enabled = true;
            steadyStateTimer.Start();

            intervalTimer = new Timer(STEADYSTATE_INTERVAL*1000);
            intervalTimer.Elapsed += OnTimedEvent_CheckHeartRate;
            intervalTimer.Enabled = true;
            intervalTimer.Start();
        }

        private void OnTimedEvent_CheckHeartRate(Object source, ElapsedEventArgs e)
        {
            int hr = heartFrequency.Last();

            if (hr < HR_MIN)
            {
                Console.WriteLine("Heartrate to low to continue");
                IncreaseResistance();
                ExitSteadyStateTest();
                return;
            }
            if (steadyHeartFrequency.Count() > 0)
            {
                steadyHeartFrequency.Add(hr);

                if (steadyHeartFrequency.Max() - steadyHeartFrequency.Min() > HR_MAX_DIFFERENCE)
                {
                    Console.WriteLine("Heartrate to irregular to continue");
                    ExitSteadyStateTest();
                    return;
                }
            }

            if (!steadyStateTestSuccesfull)
            {
                intervalTimer.Start();
            }
        }

        public void ExitSteadyStateTest()
        {
            steadyHeartFrequency.Clear();
        }

        private void OnTimedEvent_SteadyStateTestFinished(Object source, ElapsedEventArgs e)
        {
            steadyStateTestSuccesfull = true;
            Console.WriteLine("SteadyStateTest Completed Succesfully!");
        }

        // // // // // // // // // // // // // // // // // // // // // // // //
        // Phase Timer
        // // // // // // // // // // // // // // // // // // // // // // // //
       

        private void SetTimer(int seconds, AstrandTestPhase nextPhase)
        {
            this.nextPhase = nextPhase;
            myTimer = new Timer(seconds * 1000);
            myTimer.Elapsed += OnTimedEvent_ChangePhase;
            myTimer.Enabled = true;
            myTimer.Start();
        }

        


        private void OnTimedEvent_ChangePhase(Object source, ElapsedEventArgs e)
        {
            ChangePhase(nextPhase);
        }

        // // // // // // // // // // // // // // // // // // // // // // // //
        // // // // // // // // // // // // // // // // // // // // // // // //


        private double CalculateVO2Max()
        {
            double workload = steadyIC.Last() * WORKLOAD_CONSTANT;
            double VO2max;
            if (!male)
            {
                VO2max = (0.00193 * workload + 0.326) / (0.769 * steadyHeartFrequency.Last() - 56.1) * 100;
            }
            else
            {
                VO2max = (0.00212 * workload + 0.299) / (0.769 * steadyHeartFrequency.Last() - 48.5) * 100;
            }

            if (age >= 30)
            {
                if (age < 35)
                {
                    VO2max *= 1;
                }
                else if (age < 40)
                {
                    VO2max *= 0.87;
                }
                else if (age < 45)
                {
                    VO2max *= 0.83;
                }
                else if (age < 50)
                {
                    VO2max *= 0.78;
                }
                else if (age < 55)
                {
                    VO2max *= 0.75;
                }
                else if (age < 60)
                {
                    VO2max *= 0.71;
                }
                else if (age < 65)
                {
                    VO2max *= 0.68;
                }
                else
                {
                    VO2max *= 0.65;
                }
            }

            
            return VO2max;
        }

        public int GetMaxHeartBeat(int age)
        {
            int maxheartbeat;

            if(age < 25 && age >= 15)
            {
                maxheartbeat = 210;
            }
            else if (age < 35 && age >= 25)
            {
                maxheartbeat = 200;
            }
            else if (age < 40 && age >= 35)
            {
                maxheartbeat = 190;
            }
            else if (age < 45 && age >= 40)
            {
                maxheartbeat = 180;
            }
            else if (age < 50 && age >= 45)
            {
                maxheartbeat = 170;
            }
            else if (age < 55 && age >= 50)
            {
                maxheartbeat = 160;
            }
            else if (age < 60 && age >= 55)
            {
                maxheartbeat = 150;
            }
            else maxheartbeat = 0;

            return maxheartbeat;
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

            if(dataType == DataTypes.HR)
            {
                heartFrequency.Add(value);
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

        public void IncreaseResistance()
        {
            throw new NotImplementedException();
        }

        public void DecreaseResistance()
        {
            throw new NotImplementedException();
        }

        public void SetRotation(int rotation)
        {
            if (rotation < ROTATIONTARGET_MIN)
            {
                Console.WriteLine("GO FASTER!");
            }

            else if (rotation > ROTATIONTARGET_MAX)
            {
                Console.WriteLine("SLOW DOWN!");
            }
        }

    }
}
