using EQX.Core.Motion;
using EQX.ThirdParty.Fastech;

namespace EQX.Motion
{
    public class MotionEziServoPlusR : MotionEziPlusR
    {
        public MotionEziServoPlusR(int id, string name, IMotionParameter parameter)
            : base(id, name, parameter)
        {
        }

        public override bool AlarmReset()
        {
            return nativeLib.ServoAlarmReset() == EziPlusRMotionLib.FMM_OK;
        }
    }
}
