using EQX.Core.Motion;
using EQX.ThirdParty.Fastech;

namespace EQX.Motion
{
    public class MotionEziPlusR : MotionBase
    {
        public string PortName { get; set; }
        public int Baudrate { get; set; }

        public MotionEziPlusR(int id, string name, IMotionParameter parameter)
            : base(id, name, parameter)
        {
        }

        public override bool Initialization()
        {
            return true;
        }

        public override bool Connect()
        {
            bool result = nativeLib.Connect();
            IsConnected = result;
            return result;
        }

        public override bool Disconnect()
        {
            bool result = nativeLib.Disconnect();
            IsConnected = false;
            return result;
        }

        public override bool MotionOff()
        {
            return nativeLib.ServoEnable(0) == EziPlusRMotionLib.FMM_OK;
        }
        
        public override bool MotionOn()
        {
            return nativeLib.ServoEnable(1) == EziPlusRMotionLib.FMM_OK;
        }

        protected override bool ActualSearchOrigin()
        {
            return nativeLib.SearchOrigin() == EziPlusRMotionLib.FMM_OK;
        }

        public override bool MoveAbs(double position, double velocity)
        {
            return nativeLib.MoveAbs((int)(position * 1000), (uint)(velocity * 1000)) == EziPlusRMotionLib.FMM_OK;
        }

        public override bool MoveInc(double position, double velocity)
        {
            return nativeLib.MoveInc((int)(position * 1000), (uint)(velocity * 1000)) == EziPlusRMotionLib.FMM_OK;
        }

        public override void MoveJog(double speed, bool isForward)
        {
        }

        public override bool Stop(bool forceStop = true)
        {
            return forceStop ? nativeLib.EMGStop() == EziPlusRMotionLib.FMM_OK
                             : nativeLib.SoftStop() == EziPlusRMotionLib.FMM_OK;
        }

        #region Privates
        internal EziPlusRMotionLib nativeLib;
        #endregion
    }
}
