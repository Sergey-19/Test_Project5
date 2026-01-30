using EQX.Core.Motion;

namespace EQX.Motion
{
    public class MotionBase : IMotion
    {
        public IMotionStatus Status { get; protected set; }
        public IMotionParameter Parameter { get; protected set; }
        public int Id { get; init; }
        public string Name { get; init; }
        public virtual bool IsConnected { get; protected set; }

        public MotionBase(int id, string name, IMotionParameter parameter)
        {
            Id = id;
            Name = name;
            Parameter = parameter;

            Status = new MotionStatus();

            System.Timers.Timer statusUpdateTimer = new System.Timers.Timer(100);
            statusUpdateTimer.Elapsed += StatusUpdateTimer_Elapsed;
            statusUpdateTimer.AutoReset = true;
            statusUpdateTimer.Enabled = true;
        }

        public override string ToString() => Name;

        private void StatusUpdateTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (IsConnected == false) return;

            lock (searchOriginLock)
            {
                UpdateAxisStatus();
            }
        }

        public virtual bool Connect()
        {
            return true;
        }

        public virtual bool Disconnect()
        {
            return true;
        }

        public virtual bool Initialization()
        {
            return true;
        }

        public virtual bool MotionOff()
        {
            return true;
        }

        public virtual bool MotionOn()
        {
            return true;
        }
        public bool MoveAbs(double position)
        {
            return MoveAbs(position, Parameter.Velocity);
        }
        public virtual bool MoveAbs(double position, double speed)
        {
            return true;
        }
        public bool MoveInc(double position)
        {
            return MoveInc(position, Parameter.Velocity);
        }
        public virtual bool MoveInc(double position, double speed)
        {
            return true;
        }

        public virtual void MoveJog(double speed, bool isForward)
        {
        }

        public bool SearchOrigin()
        {
            // Lock to prevent IsHomeDone status Set / Clear by timer
            lock (searchOriginLock)
            {
                ((MotionStatus)Status).IsHomeDone = false;

                return ActualSearchOrigin();
            }
        }

        private object searchOriginLock = new object();

        protected virtual bool ActualSearchOrigin()
        {
            return true;
        }

        public virtual bool Stop(bool forceStop = true)
        {
            return true;
        }

        public virtual bool AlarmReset()
        {
            return true;
        }

        protected double PulseToMM(int pulse)
        {
            return pulse * 1.0 * Parameter.Unit / Parameter.Pulse;
        }

        protected int MMtoPulse(double mm)
        {
            return (int)(mm * 1.0 * Parameter.Pulse / Parameter.Unit);
        }

        protected virtual void UpdateAxisStatus() { }

        public bool IsOnPosition(double dPosition)
        {
            return (Math.Abs(Status.ActualPosition - dPosition) < allowPositionDiff) & (Status.IsMotioning == false);
        }

        public virtual bool ClearPosition()
        {
            return true;
        }

        private double allowPositionDiff = 0.02;
    }
}
