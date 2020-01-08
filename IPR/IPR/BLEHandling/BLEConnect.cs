using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avans.TI.BLE;
using IPR.BLEHandling;

namespace IPR
{
    public class BLEConnect
    {

        public string ergoID;
        public BLE ergoBLE { get; set; }
        public BLE heartRateBLE { get; set; }

        public BLEHandler bleHandler { get; set; }

        public BLEConnect(string ergoID)
        {
            this.ergoID = ergoID;
            this.ergoBLE = new BLE();
            this.heartRateBLE = new BLE();
            bleHandler = new BLEHandler(this);
        }

        public void Connect()
        {
            this.connectToBLE(this.ergoBLE, this.heartRateBLE, this.ergoID);
        }

        private async void connectToBLE(BLE ergoBLE, BLE heartRateBLE, string ergoID)
        {
            // Connection attempt
            int errorCodeErgo = await ergoBLE.OpenDevice($"Tacx Flux {ergoID}");
            int errorCodeHeartRate = await heartRateBLE.OpenDevice("Decathlon Dual HR");

            PrintDevices(ergoBLE);
            PrintDevices(heartRateBLE);

            // Set services
            string service1 = "6e40fec1-b5a3-f393-e0a9-e50e24dcca9e";
            errorCodeErgo = await ergoBLE.SetService(service1);
            await heartRateBLE.SetService("HeartRate");

            //Subscribe
            ergoBLE.SubscriptionValueChanged += this.sendData;
            heartRateBLE.SubscriptionValueChanged += this.sendData;
            string service2 = "6e40fec2-b5a3-f393-e0a9-e50e24dcca9e";
            errorCodeErgo = await ergoBLE.SubscribeToCharacteristic(service2);
            errorCodeHeartRate = await heartRateBLE.SubscribeToCharacteristic("HeartRateMeasurement");

            Console.WriteLine($"Error code ergo:{errorCodeErgo} \nError code heartRate: {errorCodeHeartRate}");

        }

        private static void PrintDevices(BLE ergoMeterBle)
        {
            List<string> bluetoothDeviceList = ergoMeterBle.ListDevices();
            Console.WriteLine("Devices currently found:");
            foreach (string deviceName in bluetoothDeviceList)
            {
                Console.WriteLine($"Device: {deviceName}");
            }
        }

        public void sendData(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            bleHandler.handleData(e.Data);
        }

        public void SetResistance(int res)
        {
            throw new NotImplementedException();
        }


    }
}
