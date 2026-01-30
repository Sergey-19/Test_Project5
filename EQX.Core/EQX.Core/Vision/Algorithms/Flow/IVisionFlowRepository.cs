namespace EQX.Core.Vision.Algorithms
{
    public interface IVisionFlowRepository
    {
        void Init(List<IVisionFlow> visionFlows);
        IEnumerable<IVisionFlow> GetAll();
    }
}
