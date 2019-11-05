using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR.BLEHandling
{

     
    class BLEHandler
    {
        public BLEConnect bleConnection;

        public BLEHandler(BLEConnect bleConnection)
        {
            this.bleConnection = bleConnection;
        }

        public void handleData(byte[] rawData)
        {
            if(rawData[0] == 22)
            {
                Console.WriteLine("Heartrate message");
            }
            else if(rawData[0] == 164)
            {
                Console.WriteLine("Ergo message");
            }
        }
    }
}
