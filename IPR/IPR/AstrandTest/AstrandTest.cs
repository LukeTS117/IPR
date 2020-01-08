
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
        private static int STEADYSTATE_INTERVAL = 10;
        private static int STEADYSTATE_TIME = 120;
        private static int CHANGE_RESISTANCE_INTERVAL = 5;
        private readonly IAstrandData data;

        private static double WORKLOAD_CONSTANT = 6.11829727786744;

        private static int WARMING_UP_TIME = 120; //Time in seconds 
        private static int MAIN_TEST_TIME = 240;
        private static int COOLING_DOWN_TIME = 120;
       

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
        private FileManager fileManager;
        bool steadyStateTestSuccesfull = false;
        bool resistanceUpdate = true;

        private List<int> steadyHeartFrequency;
        private List<int> steadyIC;

        private int age;
        private int weight;
        private bool male;
        private int maxheartbeat;
        public int resPercentage = 0;


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


            public override string ToString()
            {
                return "<" + dataType.ToString() + ">" + value + "<ET>" + elapsedTime;
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

            this.intervalTimer = new Timer(STEADYSTATE_INTERVAL * 1000);
            this.steadyStateTimer = new Timer(1 * 1000);


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
            if (phase == AstrandTestPhase.WARMING_UP) { this.WarmingUp(); testWindow.SetText(testWindow.text_Phase, "Warming Up"); }
            else if (phase == AstrandTestPhase.MAIN_TEST) { this.MainTest(); testWindow.SetText(testWindow.text_Phase, "Main Test"); }
            else if (phase == AstrandTestPhase.COOLING_DOWN) { this.CoolingDown(); testWindow.SetText(testWindow.text_Phase, "Cooling Down"); }

           
            else if (phase == AstrandTestPhase.INACTIVE) { this.InActive(); testWindow.SetText(testWindow.text_Phase, "Inactive"); }

            current_phase = phase;
           
        }

        

            // // // // // // // // // // // // // // // // // // // // // // // // 
            //  Phases
            // // // // // // // // // // // // // // // // // // // // // // // //

            private void WarmingUp()
            {
                SetTimer(WARMING_UP_TIME, AstrandTestPhase.MAIN_TEST);
                ResistanceIntervalTimer();
            }

            private void MainTest()
            {
                SetTimer(MAIN_TEST_TIME, AstrandTestPhase.COOLING_DOWN);
                this.instantCadence = new List<int>();
                this.steadyHeartFrequency = new List<int>();
                this.steadyIC = new List<int>();
               
                StartSteadyStateTimer(STEADYSTATE_TIME, STEADYSTATE_INTERVAL);
            }

            private void CoolingDown()
            {
            Console.WriteLine("Main Test completed, moving on to Cooling Down");

            if (steadyStateTestSuccesfull)
            {
                double vo2 = CalculateVO2Max();
                testWindow.SetText(testWindow.text_VO2Max, vo2.ToString());
            }
            else
            {
                testWindow.SetText(testWindow.text_VO2Max, "Test Failed");
                testWindow.SetText(testWindow.text_Instruction, "No Steady State");

            }

            steadyStateTimer.Stop();
            SetTimer(COOLING_DOWN_TIME, AstrandTestPhase.INACTIVE);
            
               


                

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
                intervalTimer.Dispose();
                steadyStateTimer.Dispose();

                STEADYSTATE_INTERVAL = interval;
                steadyStateTimer = new Timer(seconds * 1000);
                steadyStateTimer.Elapsed += OnTimedEvent_SteadyStateTestFinished;
                steadyStateTimer.Enabled = true;
                steadyStateTimer.Start();

                intervalTimer = new Timer(STEADYSTATE_INTERVAL * 1000);
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
                steadyStateTimer.Dispose();
                steadyStateTimer.Close();
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

                if(myTimer != null)
                {
                    myTimer.Dispose();
                }

                myTimer = new Timer(seconds * 1000);
                testWindow.SetTimer(seconds);
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

            private void ResistanceIntervalTimer()
            {
                intervalTimer = new Timer(CHANGE_RESISTANCE_INTERVAL * 1000);
                intervalTimer.Elapsed += OnTimedEvent_Resistance;
                intervalTimer.Enabled = true;
                intervalTimer.Start();
            }

            private void OnTimedEvent_Resistance(Object sender, EventArgs e)
            {
                resistanceUpdate = true;
                intervalTimer.Dispose();
                ResistanceIntervalTimer();
            }

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

                if (age < 25 && age >= 15)
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
                DataPoint dataPoint = new DataPoint(dataType, value, this.GetElapsedTime());

                if (!testStarted)
                {
                    StartTest();
                }


                dataPoints.Add(dataPoint);
                
                Console.WriteLine(dataType.ToString() + " " + value);


                if (dataType == DataTypes.IC)
                {
                    instantCadence.Add(value);
                    testWindow.SetText(testWindow.text_Cadence, value.ToString());
                    SetRotation(value);
                }

                if (dataType == DataTypes.HR)
                {
                    heartFrequency.Add(value);
                    testWindow.SetText(testWindow.text_HeartRate, value.ToString());
                if (value < HR_MIN)
                {
                    IncreaseResistance();
                    
                }
                if (current_phase == AstrandTestPhase.COOLING_DOWN )
                {
                    DecreaseResistance();
                }
                this.testWindow.UpdateUI(value);
                }
            }

        public void EmergencyStop()
        {
            ChangePhase(AstrandTestPhase.INACTIVE);
            testWindow.SetText(testWindow.text_Instruction, "EMERGENCY STOP");
        }

            public int GetElapsedTime()
            {
                int elapsedTime;

                if (elapsedStopWatch != null)
                {
                    elapsedTime = (int)elapsedStopWatch.ElapsedMilliseconds;
                    elapsedStopWatch.Restart();
                    return elapsedTime;
                }

                return 0;
            }

            public void IncreaseResistance()
            {
            if (resistanceUpdate == true)
            {
                resPercentage += 5;
                if (resPercentage > 100)
                {
                    resPercentage = 100;
                }
                resistanceUpdate = false;
                data.SetResistance(resPercentage);
                testWindow.SetText(testWindow.text_Resistance, this.resPercentage.ToString());
            }
            }

            public void DecreaseResistance()
            {
            if (resistanceUpdate == true)
            {
                resPercentage -= 5;
                if (resPercentage < 0)
                {
                    resPercentage = 0;
                }
                resistanceUpdate = false;
                data.SetResistance(resPercentage);
                testWindow.SetText(testWindow.text_Resistance, this.resPercentage.ToString());
            }
            }

            public void SetRotation(int rotation)
            {
                if (rotation < ROTATIONTARGET_MIN)
                {
                    Console.WriteLine("GO FASTER!");
                testWindow.SetUIRotation("GO FASTER!", rotation);
                }

                else if (rotation > ROTATIONTARGET_MAX)
                {
                    Console.WriteLine("SLOW DOWN!");
                testWindow.SetUIRotation("SLOW DOWN!", rotation);
                }
                else
                {
                testWindow.SetUIRotation("Keep Pace" , rotation);
                }
            }



            
    }

        
}

