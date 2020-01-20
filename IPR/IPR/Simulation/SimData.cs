using IPR.AstrandTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace IPR.Simulation
{
    public class SimData : IAstrandData, ISim
    {
#region Variables
        IAstrandDataListener listener;
        public int targetHeartrate { get; set; }
        public int targetSpeed { get; set; }
        public int Precision { get; set; }
        private static int TIMER_INTERVAL = 2000; //interval in miliseconds
        private Timer NotifyTimer;
        private string simfilepath = null;
        private Random random;
        private SimCommands simCommands;
        public bool SmoothChange;
        public int NewTargetHR { get; set; }
        public int NewTargetSP { get; set; }
#endregion


        public int Connect(IAstrandDataListener listener)
        {
            this.listener = listener;
            if(simfilepath == null)
            {
                RunSim();
            }
            else
            {
                RunSimFromFile(this.simfilepath);
            }
            return 1;
        }

        public void SetResistance(int percentage)
        {
            
        }

        private void RunSimFromFile(string filepath)
        {

        }

        private void RunSim()
        {
            this.random = new Random();
            this.targetSpeed = 40;
            this.targetHeartrate = 100;
            this.NewTargetHR = targetHeartrate;
            this.NewTargetSP = targetSpeed;
            this.Precision = 2;
            this.simCommands = new SimCommands(this);
            this.SmoothChange = true;

            SetNotifyTimer();

        }

        private void SetNotifyTimer()
        {
            NotifyTimer = new Timer(TIMER_INTERVAL);
            NotifyTimer.Elapsed += OnNotifyTimedEvent;
            NotifyTimer.AutoReset = true;
            NotifyTimer.Enabled = true;
        }

        private void OnNotifyTimedEvent(Object source, ElapsedEventArgs e)
        { 
            int simHeartrate = targetHeartrate + (int)random.Next(-Precision, Precision);
            int simSpeed = targetSpeed + (int)random.Next(-Precision, Precision);

            listener.OnDataAvailable(BLEHandling.DataTypes.HR, simHeartrate);
            listener.OnDataAvailable(BLEHandling.DataTypes.IC, simSpeed);

            if (SmoothChange)
            {
                SetSlowChangeSpeed();
            }
            else
            {
                targetSpeed = NewTargetSP;
                targetHeartrate = NewTargetHR;
            }
        }

        private void SetSlowChangeSpeed()
        {
            if (targetHeartrate != NewTargetHR)
            {
                targetHeartrate += (int)Math.Ceiling(((double)NewTargetHR - (double)targetHeartrate) / 3.0);
            }

            if (targetSpeed != NewTargetSP)
            {
                targetSpeed += (int)Math.Ceiling(((double)NewTargetSP-(double)targetSpeed)/ 3.0);
            }
        }

        public void RetryConnection()
        {
            //doesnt do anything, its a sim
        }

        public bool SendCommand(string command)
        {
            return this.simCommands.OnCommandRecieved(command);
        }
    }

    
}
