using EQX.Core.Vision.Tool;
using EQX.Vision.Tool;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.FlowUILibraryWPF
{
    public class VisionToolRepository : IVisionToolRepository
    {
        private readonly VisionToolOptions _options;

        public VisionToolRepository(VisionToolOptions options)
        {
            _options = options;
        }

        public IEnumerable<IVisionTool> GetAll()
        {
            List<IVisionTool> allTool = new List<IVisionTool>
            {
                new LoadImageTool(),
                new SmoothingTool(),
                new AdaptiveThresholdTool(),
                new FindCircleTool(),
                new TemplateMatchingTool()
            };

            return allTool;
        }
    }
}
