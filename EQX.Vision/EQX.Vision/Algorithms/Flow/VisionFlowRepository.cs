using EQX.Core.Vision.Algorithms;

namespace EQX.Vision.Algorithms
{
    public class VisionFlowRepository : IVisionFlowRepository
    {
        List<IVisionFlow> _visionFlowList;
        public VisionFlowRepository()
        {
        }

        public IEnumerable<IVisionFlow> GetAll()
        {
            return _visionFlowList;
        }

        public void Init(List<IVisionFlow> visionFlows)
        {
            _visionFlowList = visionFlows;
        }
    }
}
