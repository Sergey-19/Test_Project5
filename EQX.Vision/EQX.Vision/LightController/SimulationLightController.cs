using EQX.Core.Communication;
using Newtonsoft.Json.Linq;

namespace EQX.Vision.LightController
{
    public class SimulationLightController : LightControllerBase
    {
        public SimulationLightController(int id, string name) : base(id, name) { }
        public bool IsLightStatus;
        public override bool SetLightLevel(int channel, int value)
        {
            return true;
        }

        public override bool SetLightStatus(int channel, bool bOnOff)
        {
            if (bOnOff)
                IsLightStatus = true;
            else
                IsLightStatus = false;
            return true;
        }
        public override bool GetLightStatus(int channel)
        {
            return IsLightStatus;
        }
    }
}
