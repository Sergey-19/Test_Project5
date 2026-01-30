using EQX.Core.Vision.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.FlowUILibraryWPF
{
    public interface IVisionToolRepository
    {
        IEnumerable<IVisionTool> GetAll();
    }
}
