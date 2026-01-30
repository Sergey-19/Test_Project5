using System.Windows.Input;

namespace EQX.Core.Common
{
    public class NavigationButton
    {
        public string Label { get; set; }
        public ICommand Command { get; set; }
        public EPermission RequiredRole { get; set; }

        public string ImageKey { get; set; }
        public string DisabledImageKey { get; set; }
    }
}
