using Brainboxes.IO;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ModbusTest
{
    public class ModbusManager
    {
        Dictionary<int, EDDevice> devices = new Dictionary<int, EDDevice>();

        public ModbusManager()
        {

        }

        public void Inititalize()
        {
            if (ConnectDevice(0, "10.72.54.222"))
            {
                Console.WriteLine("Device ID: 0 - Connected successfully.");
            }
        }

        public bool ConnectDevice(int id, String IP)
        {
            try
            {
                EDDevice device;
                if (devices.ContainsKey(id))
                {
                    devices.TryGetValue(id, out device);
                    device.Connect();
                }
                else
                {
                    device = EDDevice.Create(IP);
                }

                if (device != null && device.IsConnected)
                {
                    device.Label = IP;
                    devices.Add(id, device);
                }

                return device.IsConnected;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void DisconnectDevices()
        {
            for (int i = 0; i < devices.Count; i++)
            {
                DisconnectDevice(i);
            }
        }

        public void DisconnectDevice(int id)
        {
            EDDevice device;
            if (devices.TryGetValue(id, out device))
            {
                device.Disconnect();
                devices.Remove(id);
                Console.WriteLine($"Device ID: {id} - Disconnected.");
            }
        }

        public void SetDigitalOut(int output, int value, out String result, int id = 0)
        {
            try
            {
                EDDevice device;
                if (devices.ContainsKey(id))
                {
                    devices.TryGetValue(id, out device);
                }
                else
                {
                    result = $"No modbus device found.";
                    return;
                }

                if (!device.IsConnected)
                {
                    device.Connect();
                }

                device.Outputs[output].Value = value;
                result = $"Output {output} has been set to {(device.Outputs[output].Value == 1 ? "ON" : "OFF")}";
            }
            catch (Exception ex)
            {
                result = $"ERROR: {ex.Message}";
            }

        }

        public void LightShow(int seconds)
        {
            seconds *= 10;
            Random rand = new Random();
            IOList<IOLine> outputs = devices[0].Outputs;

            for (int i = 0; i < seconds; i++)
            {
                IOLine output = outputs[rand.Next(0, 3)];
                if (output.Value == 0) output.Value = 1;
                else output.Value = 0;
                Thread.Sleep(100);
            }

            for (int i = 0; i < 3; i++)
            {
                IOLine output = devices[0].Outputs[i];
                if (output.Value == 1) output.Value = 0;
            }

        }

    }
}
