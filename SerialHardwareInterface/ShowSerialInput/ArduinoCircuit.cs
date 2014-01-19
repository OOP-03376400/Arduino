using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace ShowSerialInput
{
    public class DataPacket
    {
        int _value;
        public string Key { get; private set; }
        public int Value { get { return _value; } }

        public DataPacket(string packet)
        {
            string value;
            string[] parts = packet.Split(':');
            if (parts.Length == 2)
            {
                Key = parts[0];
                value = parts[1];
            }
            else
            {
                throw new FormatException(string.Format(
                    "{0} cannot be parsed into a DataPacket", packet));
            }

            if (!int.TryParse(value, out _value)) return;
        }
    }

    public class ArduinoCircuit
    {
        private SerialPort comPort = new SerialPort();
        List<IDataAddon> _AddDataPacketHandlers = new List<IDataAddon>();
        private Mutex _mut = new Mutex();

        public ArduinoCircuit()
        {
            comPort.DataReceived += comPort_DataReceived;
        }

        public void AddHandler(IDataAddon func)
        {
            if (func == null) return;
            comPort.DataReceived -= comPort_DataReceived;
            _mut.WaitOne();
            _AddDataPacketHandlers.Add(func);
            _mut.ReleaseMutex();
            comPort.DataReceived += comPort_DataReceived;
        }

        void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!comPort.IsOpen) return;
            string msg = comPort.ReadExisting();
            while (msg.Length < 2 || msg.Substring(msg.Length - 2) != "\r\n")
            {
                if (!comPort.IsOpen) return;
                msg += comPort.ReadExisting();
            }
            string[] packets = msg.Replace("\r\n", "\n").Split('\n');
            var packetList = new List<DataPacket>(packets.Length);
            foreach (string packet in packets)
            {
                try
                {
                    packetList.Add(new DataPacket(packet));
                }
                catch (FormatException)
                {
                    continue;
                }
            }

            _mut.WaitOne();
            foreach (IDataAddon AddDataPacketHandler in _AddDataPacketHandlers)
            {
                AddDataPacketHandler.AddDataPackets(packetList);
            }
            _mut.ReleaseMutex();

            // Send data received acknowledgement
            comPort.Write("1");
        }

        internal void SetPort(string port)
        {
            comPort.DataReceived -= comPort_DataReceived;
            //first check if the port is already open
            //if its open then close it
            if (comPort.IsOpen)
            {
                comPort.DiscardInBuffer();
                comPort.Close();
            }

            //set the properties of our SerialPort Object
            comPort.BaudRate = 9600;    //BaudRate
            comPort.DataBits = comPort.DataBits;    //DataBits
            comPort.StopBits = StopBits.One;    //StopBits
            comPort.Parity = Parity.None;    //Parity
            comPort.PortName = port;   //PortName

            //now open the port
            comPort.Open();
            comPort.DataReceived += comPort_DataReceived;
        }
    }
}
