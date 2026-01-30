namespace EQX.Motion
{
    public class MotionAjinParameter : MotionParameter
    {
        //++ ?? ?? ?? ?? ??? ?????.
        //uMethod : (0)OneHighLowHigh   - 1?? ??, PULSE(Active High), ???(DIR=Low)  / ???(DIR=High)
        //          (1)OneHighHighLow   - 1?? ??, PULSE(Active High), ???(DIR=High) / ???(DIR=Low)
        //          (2)OneLowLowHigh    - 1?? ??, PULSE(Active Low),  ???(DIR=Low)  / ???(DIR=High)
        //          (3)OneLowHighLow    - 1?? ??, PULSE(Active Low),  ???(DIR=High) / ???(DIR=Low)
        //          (4)TwoCcwCwHigh     - 2?? ??, PULSE(CCW:???),  DIR(CW:???),  Active High     
        //          (5)TwoCcwCwLow      - 2?? ??, PULSE(CCW:???),  DIR(CW:???),  Active Low     
        //          (6)TwoCwCcwHigh     - 2?? ??, PULSE(CW:???),   DIR(CCW:???), Active High
        //          (7)TwoCwCcwLow      - 2?? ??, PULSE(CW:???),   DIR(CCW:???), Active Low
        //          (8)TwoPhase         - 2?(90' ???),  PULSE lead DIR(CW: ???), PULSE lag DIR(CCW:???)
        //          (9)TwoPhaseReverse  - 2?(90' ???),  PULSE lead DIR(CCW: ???), PULSE lag DIR(CW:???)
        public uint PulseOutput { get; set; }

        //++ ?? ?? Encoder ?? ??? ?????.
        // uMethod : (0)ObverseUpDownMode - ??? Up/Down
        //           (1)ObverseSqr1Mode   - ??? 1??
        //           (2)ObverseSqr2Mode   - ??? 2??
        //           (3)ObverseSqr4Mode   - ??? 4??
        //           (4)ReverseUpDownMode - ??? Up/Down
        //           (5)ReverseSqr1Mode   - ??? 1??
        //           (6)ReverseSqr2Mode   - ??? 2??
        //           (7)ReverseSqr4Mode   - ??? 4??
        public uint EncoderInput { get; set; }

        // uProfileMode : (0)SYM_TRAPEZOID_MODE  - Symmetric Trapezoid
        //                (1)ASYM_TRAPEZOID_MODE - Asymmetric Trapezoid
        //                (2)QUASI_S_CURVE_MODE  - Symmetric Quasi-S Curve
        //                (3)SYM_S_CURVE_MODE    - Symmetric S Curve
        //                (4)ASYM_S_CURVE_MODE   - Asymmetric S Curve
        public uint ProfileMode { get; set; }

        public double MinVelocity { get; set; }

        public uint AccelUnit { get; set; }
        
        public uint ServoOnLevel { get; set; }
        public uint ServoAlarmLevel { get; set; }
        public uint ServoInposLevel { get; set; }
        public uint ZPhaseLevel { get; set; }
        public uint HomeSignalLevel { get; set; }
        public uint PositiveLevel { get; set; }
        public uint NegativeLevel { get; set; }

        #region Home parameter
        public int HomeDirect { get; set; }
        public uint HomeSignal { get; set; }
        public uint HomeZPhaseUse { get; set; }
        public double HomeClearTime { get; set; }
        public double HomeOffset { get; set; }
        public double HomeVelFirst { get; set; }
        public double HomeVelSecond { get; set; }
        public double HomeVelThird { get; set; }
        public double HomeVelLast { get; set; }
        public double HomeAccFirst { get; set; }
        public double HomeAccSecond { get; set; }
        #endregion
    }
}
