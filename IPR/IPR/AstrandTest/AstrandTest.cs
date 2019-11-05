
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR.AstrandTest
{
    class AstrandTest
    {
        private AstrandTestPhase current_phase = AstrandTestPhase.INACTIVE;
        private static int ROTATIONTARGET_MIN = 50;
        private static int ROTATIONTARGET_MAX = 60;
        private readonly IAstrandData data;
        private static System.Timers.Timer timer;


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
           

        }

        private void MainTest()
        {
            
        }

        private void CoolingDown()
        {
           
        }

        private void ExtendedTest()
        {
            
        }

        private void InActive()
        {
            
        }

        // // // // // // // //

        

    }
}
