using EQX.Core.Common;

namespace EQX.Core.InOut
{

    public interface ICylinder : IIdentifier
    {
        ECylinderType CylinderType { get; set; }
        Func<bool>? ForwardInterlock { get; set; }
        Func<bool>? BackwardInterlock { get; set; }
        bool IsForward { get; }
        bool IsBackward { get; }
        void Forward();
        void Backward();
    }
}
