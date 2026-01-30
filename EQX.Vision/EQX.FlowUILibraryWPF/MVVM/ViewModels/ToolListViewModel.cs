using CommunityToolkit.Mvvm.Input;
using EQX.Core.Vision.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EQX.FlowUILibraryWPF.MVVM.ViewModels
{
    public class ToolListViewModel
    {

        public EventHandler AddToolEvent;


        public ToolListViewModel()
        {
        }
        public ICommand AddToolCommand
        {
            get
            {
                return new RelayCommand<object>(AddTool);
            }
        }
        private void AddTool(object sender)
        {
            AddToolEvent?.Invoke(sender, EventArgs.Empty);
        }
    }
}
