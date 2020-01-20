using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR.Simulation
{
    

    class SimCommands
    {

     #region CommandList

        private enum eCommand
        {
            SPEED_UP, SPEED_DOWN, HEARTRATE_UP, HEARTRATE_DOWN, SPEED, HEARTRATE, PRECISION, LAVICTUS
        }

        static Dictionary<string, eCommand> _commandlist = new Dictionary<string, eCommand>
        {
            {"speedup", eCommand.SPEED_UP },
            {"slowdown", eCommand.SPEED_DOWN },
            {"heartup", eCommand.HEARTRATE_UP },
            {"heartdown", eCommand.HEARTRATE_DOWN},
            {"speed", eCommand.SPEED},
            {"heartrate", eCommand.HEARTRATE},
            {"precision", eCommand.PRECISION},
            {"lavictus", eCommand.LAVICTUS}

        };
        
     #endregion

        SimData sim;
        private static int INTERVAL = 5;

        public SimCommands(SimData sim)
        {
            this.sim = sim;
        }

        public bool OnCommandRecieved(string command)
        {

            if (command.Contains(":"))
            {
                string[] aCommand = command.Split(':');

                if (!Int32.TryParse(aCommand[1], out int value))
                {
                    return false;
                }

                if ( !_commandlist.TryGetValue(aCommand[0], out eCommand cmd))
                {
                    return false;
                }

                return ExcecuteCommand(cmd, value);
            }
            else
            {
                if(!_commandlist.TryGetValue(command, out eCommand cmd))
                {
                    return false;
                }

                return ExcecuteCommand(cmd);
            }
            
        }

        private bool ExcecuteCommand(eCommand cmd, int value)
        {
            bool succes = true;
            switch (cmd)
            {
                case eCommand.SPEED_UP:
                    sim.targetSpeed += value;
                    break;
                case eCommand.SPEED_DOWN:
                    sim.targetSpeed -= value;
                    break;
                case eCommand.SPEED:
                    sim.targetSpeed = value;
                    break;
                case eCommand.HEARTRATE_UP:
                    sim.targetHeartrate += value;
                    break;
                case eCommand.HEARTRATE_DOWN:
                    sim.targetHeartrate -= value;
                    break;
                case eCommand.HEARTRATE:
                    sim.targetHeartrate = value;
                    break;
                case eCommand.PRECISION:
                    sim.Precision = value;
                    break;
                case eCommand.LAVICTUS:
                    var prs = new ProcessStartInfo("iexplore.exe");
                    if (value == 1337)
                    {
                        prs.Arguments = "https://www.youtube.com/watch?v=iSY1qdBTHX4";
                    }
                    else if(value == 666)
                    {
                        prs.Arguments = "https://www.youtube.com/watch?v=7-iRf9AWoyE";
                    }
                    else if(value == 22)
                    {
                        prs.Arguments = "https://lavictus.nl";
                    }
                    else
                    {
                        prs.Arguments = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
                    }
                   
                    Process.Start(prs);
                    break;

                default:
                    Console.WriteLine("Command Not Found!!!");
                    succes = false;
                    break;
            }
            return succes;
        }

        private bool ExcecuteCommand(eCommand cmd)
        {
            bool succes = true;
            switch (cmd)
            {
                case eCommand.SPEED_UP:
                    sim.targetSpeed += INTERVAL;
                    break;
                case eCommand.SPEED_DOWN:
                    sim.targetSpeed -= INTERVAL;
                    break;
                case eCommand.HEARTRATE_UP:
                    sim.targetHeartrate += INTERVAL;
                    break;
                case eCommand.HEARTRATE_DOWN:
                    sim.targetHeartrate -= INTERVAL;
                    break;
                case eCommand.LAVICTUS:
                    var prs = new ProcessStartInfo("iexplore.exe");
                    prs.Arguments = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
                    Process.Start(prs);
                    break;
                default:
                    Console.WriteLine("Command Not Found!!!");
                    succes = false;
                    break;
            }
            return succes;
        }


    }
}
