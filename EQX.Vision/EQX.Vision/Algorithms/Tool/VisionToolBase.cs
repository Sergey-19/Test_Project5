using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.Common;
using EQX.Core.Vision.Algorithms;
using log4net;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EQX.Vision.Algorithms
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VisionToolBase : ObservableObject, IVisionTool, ILogable
    {
        #region Properties
        [JsonProperty]
        public IObjectCollection Parameters { get; internal set; }
        [JsonProperty]
        public IObjectCollection Inputs { get; internal set; }
        [JsonProperty]
        public IObjectCollection Outputs { get; internal set; }
        [JsonProperty]
        public int Id { get; init; }
        public double PixelSize { get; internal set; } = 1.0;
        public string Name
        {
            get => GetType().Name;
            init
            {
                ;
            }
        }
        public ERunState State
        {
            get
            {
                return _state;
            }
            internal set
            {
                _state = value;
                OnPropertyChanged();
            }
        }
        public long ExecuteTime
        {
            get
            {
                return _executeTime;
            }
            set
            {
                _executeTime = value;
                OnPropertyChanged(nameof(ExecuteTime));
            }
        }

        public string ErrorMessage { get; protected set; }

        public ToolRunCallback ToolRunFinished { get; set; }
        #endregion

        public ILog Log => LogManager.GetLogger(typeof(VisionToolBase));

        #region Constructor(s)
        public VisionToolBase()
        {
            Parameters = new ObjectCollection();
            Inputs = new ObjectCollection();
            Outputs = new ObjectCollection();

            State = ERunState.Idle;

            Id = GetHashCode();
        }
        #endregion

        #region Public Methods
        public async Task RunAsync(int timeoutMs)
        {
            // 1. Clear Tool Running State
            State = ERunState.Running;
            ErrorMessage = string.Empty;

            // 2. Clear Local Variable
            var stopWatch = new Stopwatch();
            int inputSetWatcher = Environment.TickCount;
            bool inputSetSuccess = true;

            // 3. Set ExecuteTime StopWatch
            stopWatch.Start();

            // TODO: Handle valid input

            // 4. Clear Inputs / Outputs
            foreach (var key in Inputs.Keys!)
            {
                Inputs[key.Key] = null;
            }
            foreach (var key in Outputs.Keys!)
            {
                Outputs[key.Key] = null;
            }

            // 5. Waiting for Inputs to be Set
            bool isThereInputNotSetYet = false;
            while (isThereInputNotSetYet == false)
            {
                if (Environment.TickCount - inputSetWatcher > timeoutMs)
                {
                    ErrorMessage = "Set Input Timeout";
                    inputSetSuccess = false;

                    break;
                }

                isThereInputNotSetYet = true;
                for (int i = 0; i < Inputs.Keys.Count(); i++)
                {
                    if (Inputs[Inputs.Keys[i].Key] == null) isThereInputNotSetYet = false;
                }

                await Task.Delay(2);
            }

            // 6. Run DIPFunction
            if (inputSetSuccess) DIPFunction();

            // 7. Stop ExecuteTime StopWatch
            stopWatch.Stop();

            // 8. Set Tool Running State
            ExecuteTime = stopWatch.ElapsedMilliseconds;
            State = ERunState.Done;

            // 9. Invoke ToolRunFinished
            ToolRunFinished?.Invoke(ErrorMessage, Outputs);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Digital Image Process Function
        /// </summary>
        internal virtual void DIPFunction() { }

        internal virtual bool ValidInputs()
        {
            return true;
        }
        #endregion

        #region Privates
        private long _cost;
        private ERunState _state;
        private long _executeTime;

        internal Thread ThisThread;
        #endregion
    }
}
