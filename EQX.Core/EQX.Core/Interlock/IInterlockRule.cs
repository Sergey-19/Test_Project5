namespace EQX.Core.Interlock
{
    public interface IInterlockRule
    {
        string Key { get; }
        bool IsSatisfied();
    }
}
