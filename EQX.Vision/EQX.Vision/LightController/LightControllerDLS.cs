using EQX.Core.Communication;
using log4net;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.Vision.LightController
{
    public class LightControllerDLS : LightControllerBase
    {
        #region Constructors & Deconstructors
        public LightControllerDLS(int id, string name, string comPort, int baudRate = 9600)
            : base(id, name)
        {
            Id = id;
            Name = name;

            serialCommunicator = new SerialCommunicator(id, name, comPort, baudRate, Parity.Odd, 8, StopBits.None);
        }
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
            serialCommunicator.Write($"[{channel:00}{value:000}");

            return true;
        }

        public override bool SetLightStatus(int channel, bool bOnOff)
        {
            int value = bOnOff ? 1 : 0;
            serialCommunicator.Write($"]{channel:00}{value}");

            return true;
        }
        #endregion

        #region Privates
        private SerialCommunicator serialCommunicator;
        #endregion

    }
}
