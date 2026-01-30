using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.LightController;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.Vision.LightController
{
    public class LightControllerBase : ObservableObject, ILightController
    {
        public int Id { get; init; }
        public string Name { get; set; }

        public virtual bool IsConnected { get; protected set; }

        public LightControllerBase(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public virtual bool Connect()
        {
            return true;
        }

        public virtual bool Disconnect()
        {
            return true;
        }

        public virtual int GetLightLevel(int channel)
        {
            return 0;
        }

        public virtual bool GetLightStatus(int channel)
        {
            return true;
        }

        public virtual bool SetLightLevel(int channel, int value)
        {
            return true;
        }

        public virtual bool SetLightStatus(int channel, bool bOnOff)
        {
            return true;
        }
    }
}
