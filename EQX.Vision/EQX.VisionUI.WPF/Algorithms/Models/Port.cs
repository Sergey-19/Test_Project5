using EQX.FlowUILibraryWPF.Algorithms.Defines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.FlowUILibraryWPF.Algorithms.Models
{
    public class Port
    {
        #region Properties
        public int ToolId { get; set; }
        public string KeyPort { get; set; }
        public PortType PortType { get; set; }
        #endregion
        #region Constructors
        public Port(int toolId, string keyPort, PortType portType)
        {
            ToolId = toolId;
            KeyPort = keyPort;
            PortType = portType;
        }
        #endregion
    }
}
