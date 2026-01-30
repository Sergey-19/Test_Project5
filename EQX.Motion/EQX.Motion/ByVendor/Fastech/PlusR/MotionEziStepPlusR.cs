using EQX.Core.Motion;
using EQX.ThirdParty.Fastech;

namespace EQX.Motion
{
    public class MotionEziStepPlusR : MotionEziPlusR
    {
        public MotionEziStepPlusR(int id, string name, IMotionParameter parameter) : base(id, name, parameter)
        {
        }

        public override bool AlarmReset()
        {
            return nativeLib.StepAlarmReset() == EziPlusRMotionLib.FMM_OK;
        }
    }
}
