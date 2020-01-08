using IPR.AstrandTest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR.BLEHandling
{

     
    public class BLEHandler : IAstrandData
    {
        public BLEConnect bleConnection;
        IAstrandDataListener listner;

        public BLEHandler(BLEConnect bleConnection)
        {
            this.bleConnection = bleConnection;
        }

        public void Connect(IAstrandDataListener listener)
        {
            this.listner = listener;
        }

        public void handleData(byte[] rawData)
        {
            if(rawData[0] == 22)
            {
                int heartRate = rawData[1];
                listner.OnDataAvailable(DataTypes.HR, heartRate);

            }
            else if(rawData[0] == 164)
            {
                int messageLength = rawData[1];
                byte[] message = rawData.Skip(4).Take(messageLength).ToArray();
                int pageNumber = message[0];

                if(pageNumber == 25)
                {
                    int instanteousCadence = message[2];
                    listner.OnDataAvailable(DataTypes.IC, instanteousCadence);  
                }
            }
        }

  
        public void RetryConnection()
        {
            bleConnection.Connect();
        }
        public void SetResistance(int percentage)
        {
            bleConnection.SetResistance(percentage);
        }
    }

}

