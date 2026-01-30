using EQX.Core.Common;

namespace EQX.Core.InOut
{
    /// <summary>
    /// Digital input device (multiple input contact, like a Input Driver)
    /// </summary>
    public interface IDInputDevice : IIdentifier, IHandleConnection
    {
        /// <summary>
        /// Return value of IDInput at specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        bool this[int index] { get; }
        /// <summary>
        /// Return IDInput by enum key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IDInput this[Enum key] { get; }
        List<IDInput> Inputs { get; }

        bool Initialize();

        void InverseStatus(IDInput input);
        void InverseStatus(IList<IDInput> inputs);
    }
}
