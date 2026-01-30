using EQX.Core.Communication;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace EQX.Vision.LightController
{
    public class LightControllerMV : LightControllerBase
    {
        #region Properties
        public override bool IsConnected => serialCommunicator.IsConnected;
        #endregion

        public LightControllerMV(int id, string name, string comPort, int baudRate = 115200)
            : base(id, name)
        {
            Id = id;
            Name = name;

            serialCommunicator = new SerialCommunicator(id, name, comPort, baudRate, Parity.Odd, 8, StopBits.One);
        }

        #region Privates
        private string[] strChanels = new string[6] { "A", "B", "C", "D", "E", "F" };
        #endregion

        #region Methods
        public override bool Connect()
        {
            return serialCommunicator.Connect();
        }

        public override bool Disconnect()
        {
            return serialCommunicator.Disconnect();
        }

        public override bool SetLightLevel(int channel, int value)
        {
            serialCommunicator.Write($"SL{strChanels[channel]}{value:0000}#");

            return serialCommunicator.Read() == $"L{strChanels[channel]}{value:0000}";
        }

        public override bool SetLightStatus(int channel, bool bOnOff)
        {
            int value = bOnOff ? 1 : 0;
            serialCommunicator.Write($"SW{strChanels[channel]}{value:0000}#");

            return serialCommunicator.Read() == $"W{strChanels[channel]}{value:0000}";
        }

        public bool SetPulseDuration(int channel, int pulseValue)
        {
            serialCommunicator.Write($"SP{strChanels[channel]}{pulseValue:0000}#");

            return serialCommunicator.Read() == $"P{strChanels[channel]}{pulseValue:0000}";
        }
        public override bool GetLightStatus(int channel)
        {
            serialCommunicator.Write($"SL{strChanels[channel]}#");

            return serialCommunicator.Read() != $"L{strChanels[channel]}0000";
        }
        #endregion

        #region Privates
        private SerialCommunicator serialCommunicator;
        #endregion
    }
}
